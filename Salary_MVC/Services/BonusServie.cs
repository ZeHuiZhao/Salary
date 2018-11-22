using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary_MVC.DataModel;
using Salary_MVC.Models;

namespace Salary_MVC.Services
{
    public class BonusServie:Data.Service<DataModel.GZ_Bonus>
    {
        /// <summary>
        /// 扩展名对应的附件类别
        /// </summary>
        static Dictionary<string, DataModel.GZ_Attachment.CategoryEnum> DictCategory = new Dictionary<string, DataModel.GZ_Attachment.CategoryEnum>() {
            {"jpg",DataModel.GZ_Attachment.CategoryEnum.图片 }
            ,{"jpeg",DataModel.GZ_Attachment.CategoryEnum.图片 }
            ,{"png",DataModel.GZ_Attachment.CategoryEnum.图片 }
            ,{"bmp",DataModel.GZ_Attachment.CategoryEnum.图片 }
            ,{"gif",DataModel.GZ_Attachment.CategoryEnum.图片 }
            ,{"pdf",DataModel.GZ_Attachment.CategoryEnum.PDF }
        };

         SMSService _sms = new SMSService();
         UserService _user = new UserService();

        public List<Tuple<GZ_Bonus, GZ_Employee,GZ_Attachment>> GetEntity(BonusQuery parameter)
        {
            
            var query = this.DbContext.GZ_Bonus.Join(this.DbContext.GZ_Employee, x => x.EmployeeId, y => y.Id, (x, y) => new { Bonus = x, Employee = y })
                .GroupJoin(this.DbContext.GZ_Attachment,a=>a.Bonus.Id,b=>b.SourceId,(a,b)=> new {a.Bonus,a.Employee,Attachment=b.FirstOrDefault() });
            if (!string.IsNullOrEmpty(parameter.Name))
                query = query.Where(x => x.Employee.Name.Contains(parameter.Name));
            var lstEnum = System.Enum.GetValues(typeof(Salary_MVC.Enum.ApproveStatus)).Cast<Salary_MVC.Enum.ApproveStatus>().ToList();
            if (lstEnum.Contains(parameter.Status))
                query = query.Where(x => x.Bonus.Status == parameter.Status);
            DateTime firstday = new DateTime(parameter.Month.Year, parameter.Month.Month, 1);
            var lastday = firstday.AddMonths(1).AddDays(-1);
            if (parameter.Month != new DateTime(1, 1, 1))
                query = query.Where(x => x.Bonus.StartDate <= lastday && x.Bonus.EndDate >= firstday);
            if ((parameter.CompanyId ?? Guid.Empty) != Guid.Empty)
                query = query.Where(x => x.Bonus.CompanyId == parameter.CompanyId.Value);
            if ((parameter.DepartmentId ?? Guid.Empty) != Guid.Empty)
                query = query.Where(x => x.Bonus.DepartmentId == parameter.DepartmentId.Value);
            var lst = query.OrderByDescending(x=> new { x.Bonus.Status,x.Bonus.CreateDate}).ToList().Select(x=>Tuple.Create(x.Bonus,x.Employee,x.Attachment)).ToList();
            return lst;
        }

        /// <summary>
        /// 新增津贴记录
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public int Add(Models.BonusAdd parameter)
        {
            var employee= this.DbContext.GZ_Employee.Where(x => x.Id == parameter.EmployeeId && x.StatusJob == 1).FirstOrDefault();
            if (employee == null)
                throw new ArgumentException("未找到指定的员工信息，该员工可能已经离职或者辞退");
            DataModel.GZ_Bonus bonus = new DataModel.GZ_Bonus() {
                Id = Guid.NewGuid(),
                Comment = parameter.Comment??string.Empty,
                CreateDate = DateTime.Now,
                CreateUser = this.UserInfo.Id,
                EmployeeId = parameter.EmployeeId,
                EndDate = parameter.EndDate,
                LastUpdateDate = DateTime.Now,
                LastUpdateUser = this.UserInfo.Id,
                Money_pwd = parameter.Money,
                StartDate = parameter.StartDate,
                Status = Enum.ApproveStatus.待发起审核,
                 DepartmentId=employee.DepartmentId,
                  FinancailUnitId=employee.FinacialUnitId??Guid.Empty,
                   CompanyId=employee.CorpId
                    
            };
            if(!string.IsNullOrEmpty(parameter.Attachment))
            {
                DataModel.GZ_Attachment attachment = new DataModel.GZ_Attachment() {
                    Id = Guid.NewGuid(),
                    FilePath = parameter.Attachment,
                    SourceId = bonus.Id
                };
                var extension = System.IO.Path.GetExtension(attachment.FilePath).ToLower();
                attachment.Category = DictCategory.ContainsKey(extension) ? DictCategory[extension] : DataModel.GZ_Attachment.CategoryEnum.其他;
                this.DbContext.GZ_Attachment.Add(attachment);
            }
            
            this.DbContext.GZ_Bonus.Add(bonus);
            
            return this.DbContext.SaveChanges();
        }

        /// <summary>
        /// 获取指定Id的津贴记录
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Tuple<GZ_Bonus, GZ_Employee, GZ_Attachment> GetEntityById(Guid id)
        {
            var query = this.DbContext.GZ_Bonus.Where(x=>x.Id== id).Join(this.DbContext.GZ_Employee, x => x.EmployeeId, y => y.Id, (x, y) => new { Bonus = x, Employee = y })
                .GroupJoin(this.DbContext.GZ_Attachment, a => a.Bonus.Id, b => b.SourceId, (a, b) => new { a.Bonus, a.Employee, Attachment = b.FirstOrDefault() });
            var tmp = query.FirstOrDefault();
            return Tuple.Create(tmp.Bonus,tmp.Employee,tmp.Attachment);
        }

        /// <summary>
        /// 修改指定Id的津贴记录
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public int Edit(BonusEdit parameter)
        {
            var bonus= this.DbContext.GZ_Bonus.AsNoTracking().Where(x => x.Id == parameter.Id).FirstOrDefault();
            if (bonus == null)
                throw new ArgumentException("未找到要修改的津贴主记录");
            if (bonus.Status != Enum.ApproveStatus.待发起审核)
                throw new ArgumentException(string.Format("不能对【{0}】状态的津贴数据进行修改",bonus.Status.ToString()));
            //附件信息
            var attachment = this.DbContext.GZ_Attachment.AsNoTracking().Where(x => x.SourceId == parameter.Id).FirstOrDefault();
            //if (attachment == null)
            //    throw new ArgumentException("修改津贴，未找到津贴的附件信息");
            //修改记录表
            List<GZ_UpdateHistory> lstHistory = new List<GZ_UpdateHistory>();
            DataModel.UpdateHistoryActivator<GZ_Bonus> activator = new UpdateHistoryActivator<GZ_Bonus>(Cookies.User);
            lstHistory.Add(activator.Create( bonus,x => x.Money_pwd,parameter.Money));
            lstHistory.Add(activator.Create(bonus, x => x.StartDate, parameter.StartDate));
            lstHistory.Add(activator.Create(bonus, x => x.EndDate, parameter.EndDate));
            lstHistory.Add(activator.Create(bonus, x => x.Comment, parameter.Comment));
            DataModel.UpdateHistoryActivator<GZ_Attachment> activatorAttachment = new UpdateHistoryActivator<GZ_Attachment>(Cookies.User);
            lstHistory.Add(activatorAttachment.Create(attachment??new GZ_Attachment(), x => x.FilePath, parameter.FilePath));
            lstHistory= lstHistory.Where(x => x.OldValue != x.NewValue).ToList();
            this.DbContext.GZ_UpdateHistory.AddRange(lstHistory);
            //修改津贴主记录 和附件信息
            bonus.Money_pwd = parameter.Money;
            bonus.StartDate = parameter.StartDate;
            bonus.EndDate = parameter.EndDate;
            bonus.Comment = parameter.Comment??string.Empty;
            bonus.LastUpdateDate = DateTime.Now;
            bonus.LastUpdateUser = this.UserInfo.Id;
            var bonusWrapper= this.DbContext.Entry(bonus);
            bonusWrapper.State = System.Data.Entity.EntityState.Unchanged;
            bonusWrapper.Property(x => x.Money).IsModified = true;
            bonusWrapper.Property(x => x.StartDate).IsModified = true;
            bonusWrapper.Property(x => x.EndDate).IsModified = true;
            bonusWrapper.Property(x => x.Comment).IsModified = true;
            bonusWrapper.Property(x => x.LastUpdateDate).IsModified = true;
            bonusWrapper.Property(x => x.LastUpdateUser).IsModified = true;

            if (attachment != null)
                this.DbContext.Entry(attachment).State = System.Data.Entity.EntityState.Deleted;
            if(!string.IsNullOrEmpty(parameter.FilePath))
            {
               var  attachment2 = new DataModel.GZ_Attachment() {
                    Id = Guid.NewGuid(),
                    FilePath = parameter.FilePath,
                    SourceId = bonus.Id
                };
                var extension = System.IO.Path.GetExtension(attachment2.FilePath).ToLower();
                attachment2.Category = DictCategory.ContainsKey(extension) ? DictCategory[extension] : DataModel.GZ_Attachment.CategoryEnum.其他;
                this.DbContext.GZ_Attachment.Add(attachment2);
            }
            
            return this.DbContext.SaveChanges();
        }

        /// <summary>
        /// 删除指定Id的津贴记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteById(Guid id)
        {
            var bonus = this.DbContext.GZ_Bonus.Where(x => x.Id == id).FirstOrDefault();
            if (bonus == null)
                throw new ArgumentException("未找到要删除的津贴记录");
            if (bonus.Status != Enum.ApproveStatus.待发起审核)
                throw new ArgumentException(string.Format("不能删除【{0}】状态的津贴记录", bonus.Status.ToString()));
            var attachment = this.DbContext.GZ_Attachment.Where(x => x.SourceId == bonus.Id).FirstOrDefault();
            //if (attachment == null)
            //    throw new ArgumentException("未找到津贴记录的附件信息");
            if (attachment != null)
            {
                this.DbContext.GZ_Attachment.Remove(attachment);
            }
            this.DbContext.GZ_Bonus.Remove(bonus);
            return this.DbContext.SaveChanges();
        }

        /// <summary>
        /// HR对一个或多个津贴记录发起审核
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public int ApproveByHR(StartApproveBatch parameter)
        {
            var lstWrapper= this.DbContext.GZ_Bonus.AsNoTracking().Where(x => parameter.TargetIds.Contains(x.Id))
                .Join(this.DbContext.GZ_Employee,x=>x.EmployeeId,y=>y.Id,(x,y)=> new { Bonus=x,Employee=y})
                .ToList();
            var lstId =lstWrapper.Select(x => x.Bonus.Id).ToList();
            var lstEx= parameter.TargetIds.Except(lstId).ToList();
            if (lstEx.Count > 0)
                throw new ArgumentException(string.Format("指定的津贴记录不存在，Id如下【{0}】",string.Join(",",lstEx)));
            var lstStatus= lstWrapper.Where(x => x.Bonus.Status != Enum.ApproveStatus.待发起审核).ToList();
            if(lstStatus.Count>0)
                throw new ArgumentException(string.Format("不能对【{0}】状态的津贴记录发起审核", string.Join(",", lstStatus.Select(x=>x.Bonus.Status.ToString()).Distinct())));
            //津贴记录状态
            this.DbContext.Configuration.ValidateOnSaveEnabled = false;
            lstWrapper.ForEach(x => {
                x.Bonus.Status = Enum.ApproveStatus.待财务审核;
                var tmp = this.DbContext.Entry(x.Bonus);
                tmp.State = System.Data.Entity.EntityState.Unchanged;
                tmp.Property(a => a.Status).IsModified = true;
            });
            //审核记录
            var lstApprove= lstWrapper.Select(x => new DataModel.GZ_ApproveLog() {  Id=Guid.NewGuid(),
             Category= GZ_ApproveLog.ApproveLogCategory.Through,
             Comment=string.Empty,
             OperatorId=this.UserInfo.Id,
             OperatorTime=DateTime.Now,
             TargetId=x.Bonus.Id,
             TargetStatus=(int)x.Bonus.Status,
             TargetTable=nameof(DataModel.GZ_Bonus)}).ToList();
            this.DbContext.GZ_ApproveLog.AddRange(lstApprove);
            //短信通知
            //{ TemplateIdEnum.基本工资发起审核,"中力薪酬管家，HR已提交员工{1}的{2}审核申请，请登陆薪酬管家进行审核。" },
            int max = 3;
            var lstPhone = this._user.GetPhoneList(Enum.RoleEnum.Finance);
            var lstParameter = new List<string>();
            lstParameter.Add(string.Join(",", lstWrapper.Take(max).Select(x => x.Employee.Name))+(lstWrapper.Count>max?"等":string.Empty));
            lstParameter.Add("津贴");
            if (lstPhone != null && lstPhone.Count > 0)
                this._sms.SendSms(lstPhone, GZ_SMS.TemplateIdEnum.基本工资发起审核, lstParameter);
            return this.DbContext.SaveChanges();
        }

        public List<Tuple<GZ_Bonus, GZ_Employee, GZ_Attachment>> GetEntityByFinance(BonusQueryByApprove parameter)
        {
            var query= this.DbContext.GZ_Bonus.AsQueryable();
            if (parameter.TabIndex == Models.TabEnum.待审核)
                query = query.Where(x => x.Status == Enum.ApproveStatus.待财务审核);
            if (parameter.TabIndex == Models.TabEnum.已审核)
                query = query.Where(x => x.Status == Enum.ApproveStatus.财务同意 || x.Status == Enum.ApproveStatus.财务否决);
            var queryEmployee = this.DbContext.GZ_Employee.AsQueryable();
            if (!string.IsNullOrEmpty(parameter.Name))
                queryEmployee = queryEmployee.Where(x => x.Name.Contains(parameter.Name));
            var lst= query.Join(queryEmployee, x => x.EmployeeId, y => y.Id, (x, y) => new { Bonus = x, Employee = y })
                .GroupJoin(this.DbContext.GZ_Attachment, x => x.Bonus.Id, y => y.SourceId, (x, y) => new { x.Bonus, x.Employee, Attachment = y.FirstOrDefault() })
                .OrderByDescending(x => new { x.Bonus.Status, x.Bonus.CreateDate }).ToList();
            return lst.Select(x => Tuple.Create(x.Bonus, x.Employee, x.Attachment)).ToList();
        }

        /// <summary>
        /// 财务审核津贴
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public int ApproveByFinance(ApproveBatchInput parameter)
        {
            var lstWrapper = this.DbContext.GZ_Bonus.AsNoTracking().Where(x => parameter.TargetIds.Contains(x.Id))
                .Join(this.DbContext.GZ_Employee, x => x.EmployeeId, y => y.Id, (x, y) => new { Bonus = x, Employee = y })
                .ToList();
            var lstId = lstWrapper.Select(x => x.Bonus.Id).ToList();
            var lstEx = parameter.TargetIds.Except(lstId).ToList();
            if (lstEx.Count > 0)
                throw new ArgumentException(string.Format("指定的津贴记录不存在，Id如下【{0}】", string.Join(",", lstEx)));
            var lstStatus = lstWrapper.Where(x => x.Bonus.Status != Enum.ApproveStatus.待财务审核).ToList();
            if (lstStatus.Count > 0)
                throw new ArgumentException(string.Format("不能对【{0}】状态的津贴记录发起审核", string.Join(",", lstStatus.Select(x => x.Bonus.Status.ToString()).Distinct())));
            //津贴记录状态
            this.DbContext.Configuration.ValidateOnSaveEnabled = false;
            var statusNew = parameter.Handler == GZ_ApproveLog.ApproveLogCategory.Through ? Salary_MVC.Enum.ApproveStatus.财务同意 : Enum.ApproveStatus.财务否决;
            lstWrapper.ForEach(x => {
                x.Bonus.Status = statusNew;
                var tmp = this.DbContext.Entry(x.Bonus);
                tmp.State = System.Data.Entity.EntityState.Unchanged;
                tmp.Property(a => a.Status).IsModified = true;
            });
            //审核记录
            var lstApprove = lstWrapper.Select(x => new DataModel.GZ_ApproveLog() {
                Id = Guid.NewGuid(),
                Category = parameter.Handler,
                Comment = parameter.Comment??string.Empty,
                OperatorId = this.UserInfo.Id,
                OperatorTime = DateTime.Now,
                TargetId = x.Bonus.Id,
                TargetStatus = (int)x.Bonus.Status,
                TargetTable = nameof(DataModel.GZ_Bonus)
            }).ToList();
            this.DbContext.GZ_ApproveLog.AddRange(lstApprove);
            //短信通知
            //{ TemplateIdEnum.基本工资审核结果,"中力薪酬管家，财务{1}{2}的{3}审核申请，审核意见：{4}" },
            int max = 3;
            var lstPhone = this._user.GetPhoneList(Enum.RoleEnum.HR);
            var lstParameter = new List<string>();
            lstParameter.Add(parameter.Handler == GZ_ApproveLog.ApproveLogCategory.Through ? "同意" : "否决");
            lstParameter.Add(string.Join(",", lstWrapper.Take(max).Select(x => x.Employee.Name))+(lstWrapper.Count>max?"等":string.Empty));
            lstParameter.Add("津贴");
            lstParameter.Add(parameter.Comment??"无");
            if (lstPhone != null && lstPhone.Count > 0)
                this._sms.SendSms(lstPhone, GZ_SMS.TemplateIdEnum.基本工资审核结果, lstParameter);
            return this.DbContext.SaveChanges();
        }
    }
}