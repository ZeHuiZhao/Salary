using Salary_MVC.Data;
using Salary_MVC.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary_MVC.Models;
using Salary.Common;

namespace Salary_MVC.Services
{
    public class HouseMoneyService : Service<DataModel.GZ_HouseMoneyMaster>
    {

        public SMSService _sms = new SMSService();
        public UserService _user = new UserService();
        /// <summary>
        /// 导入excel数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="fullFilePath"></param>
        /// <param name="houseMoneyTemplate"></param>
        /// <returns></returns>
        public int Import(ImportInput parameter, string fullFilePath,GZ_HouseMoneyMaster.TemplateIndex houseMoneyTemplate)
        {
            var master = this.DbContext.GZ_HouseMoneyMaster.Where(x => x.Month == parameter.Month && x.Template == houseMoneyTemplate).FirstOrDefault();
            if (master != null)
                throw new ArgumentException(string.Format("已经存在{0}月的公积金数据，请删除以后再上传", parameter.Month));
            Common.AsposeSet<DataModel.GZ_HouseMoneyDetail> asposeSet = new Common.AsposeSet<GZ_HouseMoneyDetail>();
            var dictEmployee= this.DbContext.GZ_Employee.ToList().Where(x=>!string.IsNullOrEmpty(x.IDCard)).ToDictionary(x=>x.IDCard,x=>x);
            asposeSet.Map("个人公积金账号", (entity, cell) => entity.Account = cell.StringValue);
            asposeSet.Map("姓名", (entity, cell) => entity.Name = cell.StringValue);
            asposeSet.Map("身份证号码", (entity, cell) => {
                if (string.IsNullOrEmpty(cell.StringValue))
                    throw new ArgumentException("身份证号码为不能空值");
                if (!dictEmployee.ContainsKey(cell.StringValue))
                    throw new ArgumentException(string.Format("Excel【{0}-{1}】在员工信息中找不到匹配的记录",entity.Name, cell.StringValue));
                if(dictEmployee[cell.StringValue].Name!=entity.Name)
                    throw new ArgumentException(string.Format("Excel【{0}-{1}】,员工信息中【{2}-{1}】,两处信息不一致！", entity.Name, cell.StringValue, dictEmployee[cell.StringValue].Name));
                entity.IDCard = cell.StringValue;
            });
            //asposeSet.Map("缴存基数", (entity, cell) => entity.PaymentStandard=(decimal)cell.DoubleValue);//这个太严格
            asposeSet.Map("缴存基数", (entity, cell) => entity.PaymentStandard = decimal.Parse(cell.StringValue));
            asposeSet.Map("单位缴存比例", (entity, cell) => entity.CorpRate= decimal.Parse(cell.StringValue));
            asposeSet.Map("个人缴存比例", (entity, cell) => entity.PersonalRate= decimal.Parse(cell.StringValue));
            asposeSet.Map("缴存额", (entity, cell) => entity.Total= decimal.Parse(cell.StringValue));
            Common.ListWrapper<DataModel.GZ_HouseMoneyDetail> rtWrapper;
            using (Aspose.Cells.Workbook book=new Aspose.Cells.Workbook(fullFilePath))
            {
                rtWrapper = asposeSet.ToListByTitle(book.Worksheets[0]);
            }
            if (!rtWrapper.OnError && rtWrapper.RT.Count < 1)
                rtWrapper.AddErrorMessage("没有找到数据，请检查Excel");
            if (rtWrapper.OnError)
            {//写入到错误消息文件，同名的txt文件
                string errfileName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(fullFilePath),System.IO.Path.GetFileNameWithoutExtension(fullFilePath)+".txt");
                using (System.IO.StreamWriter sw=new System.IO.StreamWriter(errfileName,false))
                {
                    rtWrapper.Message.ForEach(x=>sw.WriteLine(x));
                }
                return -1;//
            }
            master = new GZ_HouseMoneyMaster() {//公积金主记录
                Id = Guid.NewGuid(),
                FilePath = parameter.FilePath,
                Month = parameter.Month,
                Status = Enum.ApproveStatus.待发起审核,
                Template = houseMoneyTemplate,
                CreateDate = DateTime.Now,
                CreateUser = this.UserInfo.Id
            };
            rtWrapper.RT.ForEach(x => {//完善数据
                x.Id = Guid.NewGuid();
                x.SubjectId = master.Id;
                x.CorpMoney = x.CorpRate * x.PaymentStandard;
                x.PersonalMoney = x.Total - x.CorpMoney;
            });
            this.DbContext.GZ_HouseMoneyMaster.Add(master);
            this.DbContext.GZ_HouseMoneyDetail.AddRange(rtWrapper.RT);
            var rt= this.DbContext.SaveChanges();
            return rt;
        }


        public bool ApproveByHR(UpdateDto parameter)
        {
            var houseMoneyMaster= this.DbContext.GZ_HouseMoneyMaster.AsNoTracking().Where(x => x.Id == parameter.Id).FirstOrDefault();
            if (houseMoneyMaster == null)
                throw new ArgumentException("未找到要审核公积金记录");
            if (houseMoneyMaster.Status != Salary_MVC.Enum.ApproveStatus.待发起审核)
                throw new ArgumentException(string.Format("不能对【{0}】状态的公积金发起审核",houseMoneyMaster.Status));
            //审核
            this.DbContext.Configuration.ValidateOnSaveEnabled = false;
            var masterWrapper= this.DbContext.Entry(new DataModel.GZ_HouseMoneyMaster() { Id = parameter.Id, Status = Salary_MVC.Enum.ApproveStatus.待财务审核 });
            masterWrapper.State = System.Data.Entity.EntityState.Unchanged;
            masterWrapper.Property(x => x.Status).IsModified = true;//只审核修改状态
            DataModel.GZ_ApproveLog approveLog = new GZ_ApproveLog() {
                Category =  GZ_ApproveLog.ApproveLogCategory.Through,//这里需要完善成枚举
                Comment = string.Empty,
                Id = Guid.NewGuid(),
                OperatorId = this.UserInfo.Id,
                OperatorTime = DateTime.Now,
                TargetId = houseMoneyMaster.Id,
                TargetStatus = (int)Salary_MVC.Enum.ApproveStatus.待财务审核,
                TargetTable = nameof(DataModel.GZ_HouseMoneyMaster)
            };
            this.DbContext.GZ_ApproveLog.Add(approveLog);//增加审核记录
            //短信记录
            //中力薪酬管家，HR已提交{1}月的{2}审核申请，请登陆薪酬管家进行审核。
            List<string> requestParameter = new List<string>();
            //requestParameter.Add(this.UserInfo.Name);
            requestParameter.Add(houseMoneyMaster.Month);
            requestParameter.Add(houseMoneyMaster.Template.ToString() + "公积金");
            var lstphone = this._user.GetPhoneList(Enum.RoleEnum.Finance);
            if(lstphone!=null &&lstphone.Count>0)
               this._sms.SendSms(lstphone, GZ_SMS.TemplateIdEnum.社保发起审核, requestParameter);

            var rt = this.DbContext.SaveChanges();
            return rt > 0;
        }

        public List<GZ_ApproveLog> GetApproveLogByMonth(string Month, GZ_HouseMoneyMaster.TemplateIndex template)
        {
            var master= this.DbContext.GZ_HouseMoneyMaster.Where(x => x.Month == Month && x.Template == template).FirstOrDefault();
            if (master == null)
                throw new Common.InputException(string.Format("查看审核进度，{0}月的公积金主记录不存在", Month));
            var lst= this.DbContext.GZ_ApproveLog.Where(x => x.TargetId == master.Id).OrderBy(x=>x.OperatorTime).ToList();
            return lst;
        }

        public bool ApproveByFinance(ApproveInput parameter)
        {
            var master= this.DbContext.GZ_HouseMoneyMaster.AsNoTracking().Where(x => x.Id == parameter.Id).FirstOrDefault();
            if (master == null)
                throw new ArgumentException("未找到要审核的公积金数据");
            if (master.Status != Salary_MVC.Enum.ApproveStatus.待财务审核)
                throw new ArgumentException(string.Format("财务不能对{0}状态的公积金数据进行审核", master.Status.ToString()));
            //改状态
            this.DbContext.Configuration.ValidateOnSaveEnabled = false;
            master.Status = parameter.Handler == GZ_ApproveLog.ApproveLogCategory.Through ? Salary_MVC.Enum.ApproveStatus.财务同意 : Salary_MVC.Enum.ApproveStatus.财务否决;
            var masterWrapper= this.DbContext.Entry(master);
            masterWrapper.State = System.Data.Entity.EntityState.Unchanged;
            masterWrapper.Property(x => x.Status).IsModified = true;
            //审核记录
            DataModel.GZ_ApproveLog approveLog = new GZ_ApproveLog() {
                Category = parameter.Handler,//这里需要完善成枚举
                Comment = parameter.Comment??"无",
                Id = Guid.NewGuid(),
                OperatorId = this.UserInfo.Id,
                OperatorTime = DateTime.Now,
                TargetId = master.Id,
                TargetStatus =(int)master.Status,
                TargetTable = nameof(DataModel.GZ_HouseMoneyMaster)
            };
            this.DbContext.GZ_ApproveLog.Add(approveLog);
            //短信记录
            //{ TemplateIdEnum.社保审核结果,"中力薪酬管家，财务{1}{2}月的{3}审核申请，审核意见：{4}" },
            List<string> requestParameter = new List<string>();
            //requestParameter.Add(this.UserInfo.Name);
            requestParameter.Add(parameter.Handler == GZ_ApproveLog.ApproveLogCategory.Through ? "通过" : "否决");
            requestParameter.Add(master.Month);
            requestParameter.Add(master.Template.ToString() + "公积金");
            requestParameter.Add(parameter.Comment ?? "无");
            var lstphone = this._user.GetPhoneList(Enum.RoleEnum.HR);
            if (lstphone != null && lstphone.Count > 0)
                this._sms.SendSms(lstphone, GZ_SMS.TemplateIdEnum.社保审核结果, requestParameter);
            var rt = this.DbContext.SaveChanges();
            return rt > 0;
        }

        /// <summary>
        /// 获取财务待审核、已审核的公积金主记录
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public GZ_HouseMoneyMaster GetHouseMoneyMasterByFinance(Models.GetEntityByApprove parameter,GZ_HouseMoneyMaster.TemplateIndex template)
        {
            var query= this.DbContext.GZ_HouseMoneyMaster.Where(x => x.Template == template);
            if (parameter.TabIndex == TabEnum.已审核)
                query = query.Where(x => x.Month == parameter.Month && (x.Status == Salary_MVC.Enum.ApproveStatus.财务同意 || x.Status == Salary_MVC.Enum.ApproveStatus.财务否决));
            else if (parameter.TabIndex == TabEnum.待审核)
                query = query.Where(x => x.Status == Salary_MVC.Enum.ApproveStatus.待财务审核);
            else
                throw new Common.InputException("财务只能获取待审核或者已审核的公积金主记录数据");
            var master= query.ToList().OrderByDescending(x => x.Month).FirstOrDefault();//如果存在多个月的待审核公积金，只展示最新月份的
            return master;
        }

        /// <summary>
        /// 获取指定月份的公积金主表记录
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="houseMoneyTemplate"></param>
        /// <returns></returns>
        public GZ_HouseMoneyMaster GetHouseMoneyMaster(GetEntityInput parameter, GZ_HouseMoneyMaster.TemplateIndex houseMoneyTemplate)
        {
            var houseMaster = this.DbContext.GZ_HouseMoneyMaster
                .Where(x => x.Month == parameter.Month && x.Template == houseMoneyTemplate).FirstOrDefault();
            return houseMaster;
        }

        /// <summary>
        /// 获取指定主表记录相关联的所有明细数据
        /// </summary>
        /// <param name="master"></param>
        /// <returns></returns>
        public List<DataModel.GZ_HouseMoneyDetail> GetHouseMoneyDetailByMasterId(DataModel.GZ_HouseMoneyMaster master, Models.GetEntityInput parameter)
        {
            var query = this.DbContext.GZ_HouseMoneyDetail.Where(x => x.SubjectId == master.Id);
            if (!string.IsNullOrEmpty(parameter.Name))
                query = query.Where(x=>x.Name.Contains(parameter.Name));
            var lst = query.ToList();
            return lst;
        }

        /// <summary>
        /// 删除指定月份的公积金数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="houseMoneyTemplate"></param>
        /// <returns></returns>
        public int DeleteByMonth(string Month, GZ_HouseMoneyMaster.TemplateIndex houseMoneyTemplate)
        {
            var houseMaster= this.DbContext.GZ_HouseMoneyMaster.Where(x => x.Month == Month && x.Template == houseMoneyTemplate).FirstOrDefault();
            if (houseMaster == null)
                throw new ArgumentException(string.Format("未找到{0}月的公积金数据", Month));
            if(houseMaster.Status== Salary_MVC.Enum.ApproveStatus.财务同意 || houseMaster.Status== Salary_MVC.Enum.ApproveStatus.待财务审核)
                throw new ArgumentException(string.Format("不能删除【{0}】或者【{1}】的数据", Salary_MVC.Enum.ApproveStatus.财务同意.ToString(), Salary_MVC.Enum.ApproveStatus.待财务审核.ToString()));
            var lstDetail= this.DbContext.GZ_HouseMoneyDetail.Where(x => x.SubjectId == houseMaster.Id).ToList();
            this.DbContext.GZ_HouseMoneyMaster.Remove(houseMaster);
            this.DbContext.GZ_HouseMoneyDetail.RemoveRange(lstDetail);
            return this.DbContext.SaveChanges();
        }
    }
}