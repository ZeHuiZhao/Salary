using Salary_MVC.DataModel;
using Salary_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Services
{
    public class ShortSalaryServie : Data.Service<DataModel.GZ_ShortSalary>
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

        public SMSService _sms = new SMSService();
        public UserService _user = new UserService();

        public List<Tuple<GZ_ShortSalary, GZ_Employee, GZ_Attachment>> GetEntity(BonusQuery parameter)
        {
            var query = this.DbContext.GZ_ShortSalary.AsQueryable();
            var lstEnum = System.Enum.GetValues(typeof(Salary_MVC.Enum.ApproveStatus)).Cast<Salary_MVC.Enum.ApproveStatus>().ToList();
            if (lstEnum.Contains(parameter.Status))
                query = query.Where(x => x.Status == parameter.Status);
            var queryEmployee = this.DbContext.GZ_Employee.AsQueryable();
            if (!string.IsNullOrEmpty(parameter.Name))
                queryEmployee = queryEmployee.Where(x => x.Name.Contains(parameter.Name));
            if ((parameter.CompanyId ?? Guid.Empty) != Guid.Empty)
                query = query.Where(x=>x.CompanyId==parameter.CompanyId.Value);
            if ((parameter.DepartmentId ?? Guid.Empty) != Guid.Empty)
                query = query.Where(x => x.DepartmentId == parameter.DepartmentId.Value);
            parameter.Month = new DateTime(parameter.Month.Year,parameter.Month.Month,1);
            if (parameter.Month.Date != new DateTime(1, 1, 1))
                query = query.Where(x => x.EffectedDate == parameter.Month);
            var lst = query.Join(queryEmployee, x => x.EmployeeId, y => y.Id, (x, y) => new { ShortSalary = x, Employee = y })
                .GroupJoin(this.DbContext.GZ_Attachment, x => x.ShortSalary.Id, y => y.SourceId, (x, y)=> new { x.ShortSalary, x.Employee, Attachment = y.FirstOrDefault() })
                .OrderByDescending(x => new { x.ShortSalary.Status,x.ShortSalary.CreateDate})
                .ToList().Select(x=>Tuple.Create(x.ShortSalary,x.Employee,x.Attachment)).ToList();
            return lst;
        }

        /// <summary>
        /// 新增奖惩记录
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public int Add(Models.ShortSalaryAdd parameter)
        {
            var employee= this.DbContext.GZ_Employee.Where(x => x.Id == parameter.EmployeeId && x.StatusJob == 1).FirstOrDefault();
            if (employee == null)
                throw new ArgumentException("未找到指定的员工信息，该员工可能已经离职或者辞退");
            DataModel.GZ_ShortSalary shortSalary = new DataModel.GZ_ShortSalary() {
                Id = Guid.NewGuid(),
                Comment = parameter.Comment??string.Empty,
                CreateDate = DateTime.Now,
                CreateUser = this.UserInfo.Id,
                EmployeeId = parameter.EmployeeId,
                //EndDate = parameter.EndDate,
                LastUpdateDate = DateTime.Now,
                LastUpdateUser = this.UserInfo.Id,
                Money = parameter.Money,
                //StartDate = parameter.StartDate,
                Status = Enum.ApproveStatus.待发起审核
                , Category=parameter.Category
                , EffectedDate=parameter.EffectedDate
                , DepartmentId=employee.DepartmentId
                , FinancailUnitId=employee.FinacialUnitId??Guid.Empty
                , CompanyId=employee.CorpId
            };
            if (!string.IsNullOrEmpty(parameter.FilePath))
            {
                DataModel.GZ_Attachment attachment = new DataModel.GZ_Attachment() {
                    Id = Guid.NewGuid(),
                    FilePath = parameter.FilePath,
                    SourceId = shortSalary.Id
                };
                var extension = System.IO.Path.GetExtension(attachment.FilePath).ToLower();
                attachment.Category = DictCategory.ContainsKey(extension) ? DictCategory[extension] : DataModel.GZ_Attachment.CategoryEnum.其他;
                this.DbContext.GZ_Attachment.Add(attachment);
            }
            
            this.DbContext.GZ_ShortSalary.Add(shortSalary);
            this.DbContext.Configuration.ValidateOnSaveEnabled = false;
            return this.DbContext.SaveChanges();
        }

        /// <summary>
        /// 获取指定Id的奖惩记录
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Tuple<GZ_ShortSalary, GZ_Employee, GZ_Attachment> GetEntityById(Guid id)
        {
            var query = this.DbContext.GZ_ShortSalary.Where(x => x.Id == id)
                .Join(this.DbContext.GZ_Employee, x => x.EmployeeId, y => y.Id, (x, y) => new { ShortSalary = x, Employee = y })
                .GroupJoin(this.DbContext.GZ_Attachment, a => a.ShortSalary.Id, b => b.SourceId, (a, b) => new { a.ShortSalary, a.Employee, Attachment = b.FirstOrDefault() });
            var tmp = query.FirstOrDefault();
            if (tmp == null)
                throw new ArgumentException("未找到指定的Id的奖惩记录");
            return Tuple.Create(tmp.ShortSalary, tmp.Employee, tmp.Attachment);
        }

        /// <summary>
        /// 修改指定Id的奖惩记录
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public int Edit(ShortSalaryEdit parameter)
        {
            var master = this.DbContext.GZ_ShortSalary.AsNoTracking().Where(x => x.Id == parameter.Id).FirstOrDefault();
            if (master == null)
                throw new ArgumentException("未找到要修改的奖惩主记录");
            if (master.Status != Enum.ApproveStatus.待发起审核)
                throw new ArgumentException(string.Format("不能对【{0}】状态的奖惩数据进行修改", master.Status.ToString()));
            //附件信息
            var attachment = this.DbContext.GZ_Attachment.AsNoTracking().Where(x => x.SourceId == parameter.Id).FirstOrDefault();
            //if (attachment == null)
            //    throw new ArgumentException("修改奖惩，未找到奖惩的附件信息");
            //修改记录表
            List<GZ_UpdateHistory> lstHistory = new List<GZ_UpdateHistory>();
            DataModel.UpdateHistoryActivator<GZ_ShortSalary> activator = new UpdateHistoryActivator<GZ_ShortSalary>(Cookies.User);
            lstHistory.Add(activator.Create(master, x => x.Money, parameter.Money));
            lstHistory.Add(activator.Create(master, x => x.EffectedDate, parameter.EffectedDate));
            lstHistory.Add(activator.Create(master, x => x.Category, parameter.Category));
            lstHistory.Add(activator.Create(master, x => x.Comment, parameter.Comment));
            DataModel.UpdateHistoryActivator<GZ_Attachment> activatorAttachment = new UpdateHistoryActivator<GZ_Attachment>(Cookies.User);
            lstHistory.Add(activatorAttachment.Create(attachment??new GZ_Attachment(), x => x.FilePath, parameter.FilePath));
            lstHistory = lstHistory.Where(x => x.OldValue != x.NewValue).ToList();
            this.DbContext.GZ_UpdateHistory.AddRange(lstHistory);
            //修改奖惩主记录 和附件信息
            master.Money = parameter.Money;
            //master.StartDate = parameter.StartDate;
            //master.EndDate = parameter.EndDate;
            master.EffectedDate = parameter.EffectedDate;
            master.Category = parameter.Category;
            master.Comment = parameter.Comment??string.Empty;
            master.LastUpdateDate = DateTime.Now;
            master.LastUpdateUser = this.UserInfo.Id;
            var masterWrapper = this.DbContext.Entry(master);
            masterWrapper.State = System.Data.Entity.EntityState.Unchanged;
            masterWrapper.Property(x => x.Money_Pwd).IsModified = true;
            //masterWrapper.Property(x => x.StartDate).IsModified = true;
            //masterWrapper.Property(x => x.EndDate).IsModified = true;
            masterWrapper.Property(x => x.EffectedDate).IsModified = true;
            masterWrapper.Property(x => x.Category).IsModified = true;
            masterWrapper.Property(x => x.Comment).IsModified = true;
            masterWrapper.Property(x => x.LastUpdateDate).IsModified = true;
            masterWrapper.Property(x => x.LastUpdateUser).IsModified = true;

            if (attachment != null)
                this.DbContext.Entry(attachment).State = System.Data.Entity.EntityState.Deleted;
            if (!string.IsNullOrEmpty(parameter.FilePath))
            {
                attachment = new DataModel.GZ_Attachment() {
                    Id = Guid.NewGuid(),
                    FilePath = parameter.FilePath,
                    SourceId = master.Id
                };
                var extension = System.IO.Path.GetExtension(attachment.FilePath).ToLower();
                attachment.Category = DictCategory.ContainsKey(extension) ? DictCategory[extension] : DataModel.GZ_Attachment.CategoryEnum.其他;
                this.DbContext.GZ_Attachment.Add(attachment);
            }
            this.DbContext.Configuration.ValidateOnSaveEnabled = false;
            return this.DbContext.SaveChanges();
        }

        /// <summary>
        /// 删除指定Id的奖惩记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteById(Guid id)
        {
            var master = this.DbContext.GZ_ShortSalary.Where(x => x.Id == id).FirstOrDefault();
            if (master == null)
                throw new ArgumentException("未找到要删除的奖惩记录");
            if (master.Status != Enum.ApproveStatus.待发起审核)
                throw new ArgumentException(string.Format("不能删除【{0}】状态的奖惩记录", master.Status.ToString()));
            var attachment = this.DbContext.GZ_Attachment.Where(x => x.SourceId == master.Id).FirstOrDefault();
            //if (attachment == null)
            //    throw new ArgumentException("未找到奖惩记录的附件信息");
            this.DbContext.GZ_ShortSalary.Remove(master);
            if (attachment != null)
            {
                this.DbContext.GZ_Attachment.Remove(attachment);
            }
            return this.DbContext.SaveChanges();
        }

        /// <summary>
        /// HR对一个或多个奖惩记录发起审核
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public int ApproveByHR(StartApproveBatch parameter)
        {
            var lstWrapper = this.DbContext.GZ_ShortSalary.AsNoTracking().Where(x => parameter.TargetIds.Contains(x.Id))
                .Join(this.DbContext.GZ_Employee, x => x.EmployeeId, y => y.Id, (x, y) => new { ShortSalary = x, Employee = y })
                .ToList();
            var lstId = lstWrapper.Select(x => x.ShortSalary.Id).ToList();
            var lstEx = parameter.TargetIds.Except(lstId).ToList();
            if (lstEx.Count > 0)
                throw new ArgumentException(string.Format("指定的奖惩记录不存在，Id如下【{0}】", string.Join(",", lstEx)));
            var lstStatus = lstWrapper.Where(x => x.ShortSalary.Status != Enum.ApproveStatus.待发起审核).ToList();
            if (lstStatus.Count > 0)
                throw new ArgumentException(string.Format("不能对【{0}】状态的奖惩记录发起审核", string.Join(",", lstStatus.Select(x => x.ShortSalary.Status.ToString()).Distinct())));
            //奖惩记录状态
            this.DbContext.Configuration.ValidateOnSaveEnabled = false;
            lstWrapper.ForEach(x => {
                x.ShortSalary.Status = Enum.ApproveStatus.待财务审核;
                var tmp = this.DbContext.Entry(x.ShortSalary);
                tmp.State = System.Data.Entity.EntityState.Unchanged;
                tmp.Property(a => a.Status).IsModified = true;
            });
            //审核记录
            var lstApprove = lstWrapper.Select(x => new DataModel.GZ_ApproveLog() {
                Id = Guid.NewGuid(),
                Category = GZ_ApproveLog.ApproveLogCategory.Through,
                Comment = string.Empty,
                OperatorId = this.UserInfo.Id,
                OperatorTime = DateTime.Now,
                TargetId = x.ShortSalary.Id,
                TargetStatus = (int)x.ShortSalary.Status,
                TargetTable = nameof(DataModel.GZ_ShortSalary)
            }).ToList();
            this.DbContext.GZ_ApproveLog.AddRange(lstApprove);
            //短信通知
            //{ TemplateIdEnum.基本工资发起审核,"中力薪酬管家，HR已提交员工{1}的{2}审核申请，请登陆薪酬管家进行审核。" },
            int max = 3;
            var lstPhone = this._user.GetPhoneList(Enum.RoleEnum.Finance);
            var lstParameter = new List<string>();
            lstParameter.Add(string.Join(",", lstWrapper.Take(max).Select(x => x.Employee.Name))+(lstWrapper.Count>max?"等":string.Empty));
            lstParameter.Add("奖惩");
            if (lstPhone != null && lstPhone.Count > 0)
                this._sms.SendSms(lstPhone, GZ_SMS.TemplateIdEnum.基本工资发起审核, lstParameter);
            return this.DbContext.SaveChanges();
        }

        public List<Tuple<GZ_ShortSalary, GZ_Employee, GZ_Attachment>> GetEntityByFinance(BonusQueryByApprove parameter)
        {
            var query = this.DbContext.GZ_ShortSalary.AsQueryable();
            if (parameter.TabIndex == Models.TabEnum.待审核)
                query = query.Where(x => x.Status == Enum.ApproveStatus.待财务审核);
            if (parameter.TabIndex == Models.TabEnum.已审核)
                query = query.Where(x => x.Status == Enum.ApproveStatus.财务同意 || x.Status == Enum.ApproveStatus.财务否决);
            var queryEmployee = this.DbContext.GZ_Employee.AsQueryable();
            if (!string.IsNullOrEmpty(parameter.Name))
                queryEmployee = queryEmployee.Where(x => x.Name.Contains(parameter.Name));
            var lst = query.Join(queryEmployee, x => x.EmployeeId, y => y.Id, (x, y) => new { ShortSalary = x, Employee = y })
                .GroupJoin(this.DbContext.GZ_Attachment, x => x.ShortSalary.Id, y => y.SourceId, (x, y) => new { x.ShortSalary, x.Employee, Attachment = y.FirstOrDefault() })
                .OrderByDescending(x => new { x.ShortSalary.Status, x.ShortSalary.CreateDate }).ToList();
            return lst.Select(x => Tuple.Create(x.ShortSalary, x.Employee, x.Attachment)).ToList();
        }

        /// <summary>
        /// 财务审核奖惩
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public int ApproveByFinance(ApproveBatchInput parameter)
        {
            var lstWrapper = this.DbContext.GZ_ShortSalary.AsNoTracking().Where(x => parameter.TargetIds.Contains(x.Id))
                .Join(this.DbContext.GZ_Employee, x => x.EmployeeId, y => y.Id, (x, y) => new { ShortSalary = x, Employee = y })
                .ToList();
            var lstId = lstWrapper.Select(x => x.ShortSalary.Id).ToList();
            var lstEx = parameter.TargetIds.Except(lstId).ToList();
            if (lstEx.Count > 0)
                throw new ArgumentException(string.Format("指定的奖惩记录不存在，Id如下【{0}】", string.Join(",", lstEx)));
            var lstStatus = lstWrapper.Where(x => x.ShortSalary.Status != Enum.ApproveStatus.待财务审核).ToList();
            if (lstStatus.Count > 0)
                throw new ArgumentException(string.Format("不能对【{0}】状态的奖惩记录发起审核", string.Join(",", lstStatus.Select(x => x.ShortSalary.Status.ToString()).Distinct())));
            //奖惩记录状态
            this.DbContext.Configuration.ValidateOnSaveEnabled = false;
            var statusNew = parameter.Handler == GZ_ApproveLog.ApproveLogCategory.Through ? Salary_MVC.Enum.ApproveStatus.财务同意 : Enum.ApproveStatus.财务否决;
            lstWrapper.ForEach(x => {
                x.ShortSalary.Status = statusNew;
                var tmp = this.DbContext.Entry(x.ShortSalary);
                tmp.State = System.Data.Entity.EntityState.Unchanged;
                tmp.Property(a => a.Status).IsModified = true;
            });
            //审核记录
            var lstApprove = lstWrapper.Select(x => new DataModel.GZ_ApproveLog() {
                Id = Guid.NewGuid(),
                Category = parameter.Handler,
                Comment = parameter.Comment ?? "无",
                OperatorId = this.UserInfo.Id,
                OperatorTime = DateTime.Now,
                TargetId = x.ShortSalary.Id,
                TargetStatus = (int)x.ShortSalary.Status,
                TargetTable = nameof(DataModel.GZ_ShortSalary)
            }).ToList();
            this.DbContext.GZ_ApproveLog.AddRange(lstApprove);
            //短信通知
            //{ TemplateIdEnum.基本工资审核结果,"中力薪酬管家，财务{1}{2}的{3}审核申请，审核意见：{4}" },
            int max = 3;
            var lstPhone = this._user.GetPhoneList(Enum.RoleEnum.HR);
            var lstParameter = new List<string>();
            lstParameter.Add(parameter.Handler == GZ_ApproveLog.ApproveLogCategory.Through ? "同意" : "否决");
            lstParameter.Add(string.Join(",", lstWrapper.Take(max).Select(x => x.Employee.Name))+(lstWrapper.Count>max?"等":string.Empty));
            lstParameter.Add("奖惩");
            lstParameter.Add(parameter.Comment??"无");
            if (lstPhone != null && lstPhone.Count > 0)
                this._sms.SendSms(lstPhone, GZ_SMS.TemplateIdEnum.基本工资审核结果, lstParameter);
            return this.DbContext.SaveChanges();
        }
    }
}