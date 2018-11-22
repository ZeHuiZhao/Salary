using Salary_MVC.Data;
using Salary_MVC.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary_MVC.Models;
using Salary.Common;
using Salary_MVC.Models;

namespace Salary_MVC.Services
{
    public class SocialMoneyGZService : Service<DataModel.GZ_SocialMoneyMasterGZ>
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
        public int Import(ImportInput parameter, string fullFilePath)
        {
            var master = this.DbContext.GZ_SocialMoneyMasterGZ.Where(x => x.Month == parameter.Month).FirstOrDefault();
            if (master != null)
                throw new ArgumentException(string.Format("已经存在{0}月的社保数据，请删除以后再上传", parameter.Month));
            //把数据取出放到datatable
            var dictEmployee = this.DbContext.GZ_Employee.ToList().Where(x => !string.IsNullOrEmpty(x.IDCard)).ToDictionary(x => x.IDCard, x => x);
            Common.AsposeSet<DataModel.GZ_SocialMoneyDetailGZ> asposeSet = new Common.AsposeSet<GZ_SocialMoneyDetailGZ>();
            asposeSet.Map((detail, cell) => detail.Name = cell.StringValue);
            asposeSet.Map((detail, cell) => {
                if (string.IsNullOrEmpty(cell.StringValue))
                    throw new ArgumentException("身份证号码为不能空值");
                if (!dictEmployee.ContainsKey(cell.StringValue))
                    throw new ArgumentException(string.Format("Excel【{0}-{1}】在员工信息中找不到匹配的记录", detail.Name, cell.StringValue));
                if (dictEmployee[cell.StringValue].Name != detail.Name)
                    throw new ArgumentException(string.Format("Excel【{0}-{1}】,员工信息中【{2}-{1}】,两处信息不一致！", detail.Name, cell.StringValue, dictEmployee[cell.StringValue].Name));
                detail.IDCard= cell.StringValue;
            });
            asposeSet.Map((detail, cell) => detail.Name=detail.Name);//跳过,证件名称，
            asposeSet.Map((detail, cell) => detail.Account = cell.StringValue);
            asposeSet.Map((detail, cell) => detail.YangLaoJiShu = decimal.Parse(cell.StringValue));
            asposeSet.Map((detail, cell) => detail.YangLaoDanWei = decimal.Parse(cell.StringValue));
            asposeSet.Map((detail, cell) => detail.YangLaoGeRen = decimal.Parse(cell.StringValue));
            asposeSet.Map((detail, cell) => detail.GongShangJiShu = decimal.Parse(cell.StringValue));
            asposeSet.Map((detail, cell) => detail.GongShangDanWei = decimal.Parse(cell.StringValue));
            asposeSet.Map((detail, cell) => detail.GongShangGeRen = decimal.Parse(cell.StringValue));
            asposeSet.Map((detail, cell) => detail.ShiYeJiShu = decimal.Parse(cell.StringValue));
            asposeSet.Map((detail, cell) => detail.ShiYeDanWei = decimal.Parse(cell.StringValue));
            asposeSet.Map((detail, cell) => detail.ShiYeGeRen = decimal.Parse(cell.StringValue));
            asposeSet.Map((detail, cell) => detail.YiLiaoJiShu = decimal.Parse(cell.StringValue));
            asposeSet.Map((detail, cell) => detail.YiLiaoDanWei = decimal.Parse(cell.StringValue));
            asposeSet.Map((detail, cell) => detail.YiLiaoGeRen = decimal.Parse(cell.StringValue));
            asposeSet.Map((detail, cell) => detail.ZhongJiXianJiShu = decimal.Parse(cell.StringValue));
            asposeSet.Map((detail, cell) => detail.ZhongJiXianDanWei = decimal.Parse(cell.StringValue));
            asposeSet.Map((detail, cell) => detail.ZhongJiXianGeRen = decimal.Parse(cell.StringValue));
            asposeSet.Map((detail, cell) => detail.ShenYuJiShu = decimal.Parse(cell.StringValue));
            asposeSet.Map((detail, cell) => detail.ShenYuGeRen = decimal.Parse(cell.StringValue));
            asposeSet.Map((detail, cell) => detail.ShenYuDanWei = decimal.Parse(cell.StringValue));
            asposeSet.Map((detail, cell) => detail.TotalCorp = decimal.Parse(cell.StringValue));
            asposeSet.Map((detail, cell) => detail.TotalPesonal = decimal.Parse(cell.StringValue));
            Common.ListWrapper<DataModel.GZ_SocialMoneyDetailGZ> rtWrapper;
            using (Aspose.Cells.Workbook book=new Aspose.Cells.Workbook(fullFilePath))
            {
                rtWrapper = asposeSet.ToList(book.Worksheets[0], 2);
            }
            if (!rtWrapper.OnError && rtWrapper.RT.Count < 1)
                rtWrapper.AddErrorMessage("没有找到数据，请检查Excel");
            if (rtWrapper.OnError)
            {//写入到错误消息文件，同名的txt文件
                string errfileName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(fullFilePath), System.IO.Path.GetFileNameWithoutExtension(fullFilePath) + ".txt");
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(errfileName, false))
                {
                    rtWrapper.Message.ForEach(x => sw.WriteLine(x));
                }
                return -1;//约定
            }
            master = new GZ_SocialMoneyMasterGZ() {//广州社保主记录
                Id = Guid.NewGuid(),
                FilePath = parameter.FilePath,
                Month = parameter.Month,
                Status = Salary_MVC.Enum.ApproveStatus.待发起审核,
                CreateDate = DateTime.Now,
                CreateUser = this.UserInfo.Id
            };
            rtWrapper.RT.ForEach(x => {//完善数据
                x.Id = Guid.NewGuid();
                x.MasterId = master.Id;
            });
            this.DbContext.GZ_SocialMoneyMasterGZ.Add(master);
            this.DbContext.GZ_SocialMoneyDetailGZ.AddRange(rtWrapper.RT);
            return this.DbContext.SaveChanges();
        }

        public bool ApproveByHR(UpdateDto parameter)
        {
            var master = this.DbContext.GZ_SocialMoneyMasterGZ.AsNoTracking().Where(x => x.Id == parameter.Id).FirstOrDefault();
            if (master == null)
                throw new ArgumentException("未找到要审核社保记录");
            if (master.Status!= Salary_MVC.Enum.ApproveStatus.待发起审核)
                throw new ArgumentException(string.Format("不能对【{0}】状态的社保发起审核", master.Status));
            //审核
            this.DbContext.Configuration.ValidateOnSaveEnabled = false;
            master.Status = Salary_MVC.Enum.ApproveStatus.待财务审核;
            var masterWrapper = this.DbContext.Entry(master);
            masterWrapper.State = System.Data.Entity.EntityState.Unchanged;
            masterWrapper.Property(x => x.Status).IsModified = true;//只审核修改状态
            DataModel.GZ_ApproveLog approveLog = new GZ_ApproveLog() {
                Category = GZ_ApproveLog.ApproveLogCategory.Through,//这里需要完善成枚举
                Comment = string.Empty,
                Id = Guid.NewGuid(),
                OperatorId = this.UserInfo.Id,
                OperatorTime = DateTime.Now,
                TargetId = master.Id,
                TargetStatus = (int)master.Status,
                TargetTable = nameof(DataModel.GZ_SocialMoneyMasterGZ)
            };
            this.DbContext.GZ_ApproveLog.Add(approveLog);//增加审核记录
            //短信记录
            List<string> requestParameter = new List<string>();
            requestParameter.Add(master.Month);
            requestParameter.Add("广州社保");
            var lstphone = this._user.GetPhoneList(Enum.RoleEnum.Finance);
            if (lstphone != null && lstphone.Count > 0)
                this._sms.SendSms(lstphone, GZ_SMS.TemplateIdEnum.社保发起审核, requestParameter);
            var rt = this.DbContext.SaveChanges();
            return rt > 0;
        }

        public List<GZ_ApproveLog> GetApproveLogByMonth(string Month)
        {
            var master = this.DbContext.GZ_SocialMoneyMasterGZ.Where(x => x.Month == Month).FirstOrDefault();
            if (master == null)
                throw new Common.InputException(string.Format("查看审核进度，{0}月的社保主记录不存在", Month));
            var lst = this.DbContext.GZ_ApproveLog.Where(x => x.TargetId == master.Id).OrderBy(x => x.OperatorTime).ToList();
            return lst;
        }

        public bool ApproveByFinance(ApproveInput parameter)
        {
            var master = this.DbContext.GZ_SocialMoneyMasterGZ.AsNoTracking().Where(x => x.Id == parameter.Id).FirstOrDefault();
            if (master == null)
                throw new ArgumentException("未找到要审核的社保数据");
            if (master.Status!= Salary_MVC.Enum.ApproveStatus.待财务审核)
                throw new ArgumentException(string.Format("财务不能对{0}状态的社保数据进行审核", master.Status.ToString()));
            //改状态
            this.DbContext.Configuration.ValidateOnSaveEnabled = false;
            master.Status = parameter.Handler == GZ_ApproveLog.ApproveLogCategory.Through ? Salary_MVC.Enum.ApproveStatus.财务同意 : Salary_MVC.Enum.ApproveStatus.财务否决;
            var masterWrapper = this.DbContext.Entry(master);
            masterWrapper.State = System.Data.Entity.EntityState.Unchanged;
            masterWrapper.Property(x => x.Status).IsModified = true;
            //审核记录
            DataModel.GZ_ApproveLog approveLog = new GZ_ApproveLog() {
                Category = parameter.Handler,//这里需要完善成枚举
                Comment = parameter.Comment ?? "无",
                Id = Guid.NewGuid(),
                OperatorId = this.UserInfo.Id,
                OperatorTime = DateTime.Now,
                TargetId = master.Id,
                TargetStatus = (int)master.Status,
                TargetTable = nameof(DataModel.GZ_SocialMoneyMasterGZ)
            };
            this.DbContext.GZ_ApproveLog.Add(approveLog);
            //短信记录
            List<string> requestParameter = new List<string>();
            requestParameter.Add(parameter.Handler == GZ_ApproveLog.ApproveLogCategory.Through ? "通过" : "否决");
            requestParameter.Add(master.Month);
            requestParameter.Add("广州社保");
            requestParameter.Add(parameter.Comment ?? "无");
            var lstphone = this._user.GetPhoneList(Enum.RoleEnum.HR);
            if (lstphone != null && lstphone.Count > 0)
                this._sms.SendSms(lstphone, GZ_SMS.TemplateIdEnum.社保审核结果, requestParameter);
            var rt = this.DbContext.SaveChanges();
            return rt > 0;
        }

        /// <summary>
        /// 财务审核，综合查询
        /// 待审核，已审核
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public GZ_SocialMoneyMasterGZ GetMasterByFinance(Models.GetEntityByApprove parameter)
        {
            var query = this.DbContext.GZ_SocialMoneyMasterGZ.AsQueryable();
            if (parameter.TabIndex == TabEnum.已审核)
                query = query.Where(x => x.Month == parameter.Month && (x.Status == Salary_MVC.Enum.ApproveStatus.财务同意 || x.Status == Salary_MVC.Enum.ApproveStatus.财务否决));
            else if (parameter.TabIndex ==  TabEnum.待审核)
                query = query.Where(x => x.Status == Salary_MVC.Enum.ApproveStatus.待财务审核);
            else
                throw new Common.InputException("财务只能获取待审核或者已审核的社保主记录数据");
            var master = query.ToList().OrderByDescending(x => x.Month).FirstOrDefault();//如果存在多个月的待审核社保，只展示最新月份的
            return master;
        }

        /// <summary>
        /// 获取指定月份的主表记录
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public GZ_SocialMoneyMasterGZ GetMasterByMonth(GetEntityInput parameter)
        {
            var master = this.DbContext.GZ_SocialMoneyMasterGZ
                .Where(x => x.Month == parameter.Month).FirstOrDefault();
            return master;
        }

        /// <summary>
        /// 获取指定主表记录相关联的所有明细数据
        /// </summary>
        /// <param name="master"></param>
        /// <returns></returns>
        public List<DataModel.GZ_SocialMoneyDetailGZ> GetDetailEntity(DataModel.GZ_SocialMoneyMasterGZ master, Models.GetEntityInput parameter)
        {
            var query = this.DbContext.GZ_SocialMoneyDetailGZ.Where(x => x.MasterId == master.Id);
            if (!string.IsNullOrEmpty(parameter.Name))
                query = query.Where(x => x.Name.Contains(parameter.Name));
            var lst = query.ToList();
            return lst;
        }


        /// <summary>
        /// 删除指定月份的社保数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="houseMoneyTemplate"></param>
        /// <returns></returns>
        public int DeleteByMonth(string Month)
        {
            var master = this.DbContext.GZ_SocialMoneyMasterGZ.Where(x => x.Month == Month).FirstOrDefault();
            if (master == null)
                throw new ArgumentException(string.Format("未找到{0}月的社保数据", Month));
            if (master.Status== Enum.ApproveStatus.待财务审核|| master.Status== Enum.ApproveStatus.财务同意)
                throw new ArgumentException(string.Format("不能删除【{0}】或者【{1}】的数据", Salary_MVC.Enum.ApproveStatus.财务同意.ToString(), Salary_MVC.Enum.ApproveStatus.待财务审核.ToString()));
            var lstDetail = this.DbContext.GZ_SocialMoneyDetailGZ.Where(x => x.MasterId == master.Id).ToList();
            this.DbContext.GZ_SocialMoneyMasterGZ.Remove(master);
            this.DbContext.GZ_SocialMoneyDetailGZ.RemoveRange(lstDetail);
            return this.DbContext.SaveChanges();
        }
    }
}