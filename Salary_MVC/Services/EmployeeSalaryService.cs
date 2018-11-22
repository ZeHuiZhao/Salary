using Salary_MVC.DataModel;
using Salary_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary_MVC.Common;
namespace Salary_MVC.Services
{
    public class EmployeeSalaryService:Data.Service<DataModel.GZ_EmployeeSalary>
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

        public List<Tuple<GZ_EmployeeSalary, GZ_Employee, GZ_Attachment>> GetEntity(BonusQuery parameter)
        {
            var query = this.DbContext.GZ_EmployeeSalary.AsQueryable();
            var lstEnum = System.Enum.GetValues(typeof(Salary_MVC.Enum.ApproveStatus)).Cast<Salary_MVC.Enum.ApproveStatus>().ToList();
            if (lstEnum.Contains(parameter.Status))
                query = query.Where(x => x.Status == parameter.Status);
            if ((parameter.CompanyId ?? Guid.Empty) != Guid.Empty)
                query = query.Where(x=>x.CompanyId==parameter.CompanyId.Value);
            if ((parameter.DepartmentId ?? Guid.Empty) != Guid.Empty)
                query = query.Where(x => x.DepartmentId == parameter.DepartmentId.Value);
            //var firstday = new DateTime(parameter.Month.Year, parameter.Month.Month, 1);
            //var lastday = firstday.AddMonths(1).AddDays(-1);
            //if (parameter.Month.Date != new DateTime(1, 1, 1))
            //    query = query.Where(x=>x.EffectedDate>=firstday && x.EffectedDate<=lastday);
            var queryEmployee = this.DbContext.GZ_Employee.AsQueryable();
            if (!string.IsNullOrEmpty(parameter.Name))
                queryEmployee = queryEmployee.Where(x => x.Name.Contains(parameter.Name));
            var lst = query.Join(queryEmployee, x => x.EmployeeId, y => y.Id, (x, y) => new { Salary = x, Employee = y })
                .GroupJoin(this.DbContext.GZ_Attachment, x => x.Salary.Id, y => y.SourceId, (x, y) => new { x.Salary, x.Employee, Attachment = y.FirstOrDefault() })
                .OrderByDescending(x => new {x.Salary.Status, x.Salary.CreateDate })
                .ToList().Select(x => Tuple.Create(x.Salary, x.Employee, x.Attachment)).ToList();
            return lst;
        }

        /// <summary>
        /// 新增调薪记录
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public int Add(Models.EmployeeSalaryAdd parameter)
        {
            var employee = this.DbContext.GZ_Employee.Where(x => x.Id == parameter.EmployeeId && x.StatusJob == 1).FirstOrDefault();
            if (employee == null)
                throw new ArgumentException("未找到指定的员工信息，该员工可能已经离职或者辞退");
            var lstEmployeeSalary= this.DbContext.GZ_EmployeeSalary.Where(x => x.EmployeeId == employee.Id).ToList();
            var maxDate= lstEmployeeSalary.Where(x=>x.Status== Enum.ApproveStatus.财务同意).Max(x => x.EffectedDate);
            if (parameter.EffectedDate.Date <= maxDate.Date)
                throw new ArgumentException(string.Format("财务已同意生效日期为{0}的调薪记录，不能再创建{1}生效的调薪记录",maxDate.ToString2(DateTimeFormat.yyyyMMdd),parameter.EffectedDate.ToString2(DateTimeFormat.yyyyMMdd)));
            var countTmp = lstEmployeeSalary.Count(x => x.Status == Enum.ApproveStatus.待财务审核);
            if (countTmp > 0)
                throw new ArgumentException("已经存在待财务审核的调薪记录，请处理完再添加新的调薪记录");
            DataModel.GZ_EmployeeSalary employeeSalary = new DataModel.GZ_EmployeeSalary() {
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
                ,EffectedDate = parameter.EffectedDate
                ,DepartmentId = employee.DepartmentId
                ,FinancailUnitId = employee.FinacialUnitId ?? Guid.Empty
                , CompanyId=employee.CorpId
            };
            if (!string.IsNullOrEmpty(parameter.FilePath))
            {
                DataModel.GZ_Attachment attachment = new DataModel.GZ_Attachment() {
                    Id = Guid.NewGuid(),
                    FilePath = parameter.FilePath,
                    SourceId = employeeSalary.Id
                };
                var extension = System.IO.Path.GetExtension(attachment.FilePath).ToLower();
                attachment.Category = DictCategory.ContainsKey(extension) ? DictCategory[extension] : DataModel.GZ_Attachment.CategoryEnum.其他;
                this.DbContext.GZ_Attachment.Add(attachment);
            }
            this.DbContext.GZ_EmployeeSalary.Add(employeeSalary);
            return this.DbContext.SaveChanges();
        }

        /// <summary>
        /// 获取指定Id的调薪记录
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Tuple<GZ_EmployeeSalary, GZ_Employee, GZ_Attachment> GetEntityById(Guid id)
        {
            var query = this.DbContext.GZ_EmployeeSalary.Where(x => x.Id == id)
                .Join(this.DbContext.GZ_Employee, x => x.EmployeeId, y => y.Id, (x, y) => new { EmployeeSalary = x, Employee = y })
                .GroupJoin(this.DbContext.GZ_Attachment, a => a.EmployeeSalary.Id, b => b.SourceId, (a, b) => new { a.EmployeeSalary, a.Employee, Attachment = b.FirstOrDefault() });
            var tmp = query.FirstOrDefault();
            if (tmp == null)
                throw new ArgumentException("未找到指定的Id的调薪记录");
            return Tuple.Create(tmp.EmployeeSalary, tmp.Employee, tmp.Attachment);
        }

        /// <summary>
        /// 修改指定Id的调薪记录
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public int Edit(EmployeeSalaryEdit parameter)
        {
            var master = this.DbContext.GZ_EmployeeSalary.AsNoTracking().Where(x => x.Id == parameter.Id).FirstOrDefault();
            if (master == null)
                throw new ArgumentException("未找到要修改的调薪主记录");
            if (master.Status != Enum.ApproveStatus.待发起审核)
                throw new ArgumentException(string.Format("不能对【{0}】状态的调薪数据进行修改", master.Status.ToString()));
            //附件信息
            var attachment = this.DbContext.GZ_Attachment.AsNoTracking().Where(x => x.SourceId == parameter.Id).FirstOrDefault();
            //if (attachment == null)
            //    throw new ArgumentException("修改调薪，未找到调薪的附件信息");
            //修改记录表
            List<GZ_UpdateHistory> lstHistory = new List<GZ_UpdateHistory>();
            DataModel.UpdateHistoryActivator<GZ_EmployeeSalary> activator = new UpdateHistoryActivator<GZ_EmployeeSalary>(Cookies.User);
            lstHistory.Add(activator.Create(master, x => x.Money, parameter.Money));
            lstHistory.Add(activator.Create(master, x => x.EffectedDate, parameter.EffectedDate));
            lstHistory.Add(activator.Create(master, x => x.Comment, parameter.Comment));
            DataModel.UpdateHistoryActivator<GZ_Attachment> activatorAttachment = new UpdateHistoryActivator<GZ_Attachment>(Cookies.User);
            lstHistory.Add(activatorAttachment.Create(attachment??new GZ_Attachment(), x => x.FilePath, parameter.FilePath));
            lstHistory = lstHistory.Where(x => x.OldValue != x.NewValue).ToList();
            this.DbContext.GZ_UpdateHistory.AddRange(lstHistory);
            //修改调薪主记录 和附件信息
            master.Money = parameter.Money;
            //master.StartDate = parameter.StartDate;
            //master.EndDate = parameter.EndDate;
            master.EffectedDate = parameter.EffectedDate;
            //master.Category = parameter.Category;
            master.Comment = parameter.Comment??string.Empty;
            master.LastUpdateDate = DateTime.Now;
            master.LastUpdateUser = this.UserInfo.Id;
            var masterWrapper = this.DbContext.Entry(master);
            masterWrapper.State = System.Data.Entity.EntityState.Unchanged;
            masterWrapper.Property(x => x.Money_Pwd).IsModified = true;
            //masterWrapper.Property(x => x.StartDate).IsModified = true;
            //masterWrapper.Property(x => x.EndDate).IsModified = true;
            masterWrapper.Property(x => x.EffectedDate).IsModified = true;
            //masterWrapper.Property(x => x.Category).IsModified = true;
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
            //this.DbContext.Configuration.ValidateOnSaveEnabled = false;
            return this.DbContext.SaveChanges();
        }

        /// <summary>
        /// 删除指定Id的调薪记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteById(Guid id)
        {
            var master = this.DbContext.GZ_EmployeeSalary.Where(x => x.Id == id).FirstOrDefault();
            if (master == null)
                throw new ArgumentException("未找到要删除的调薪记录");
            if (master.Status != Enum.ApproveStatus.待发起审核)
                throw new ArgumentException(string.Format("不能删除【{0}】状态的调薪记录", master.Status.ToString()));
            var attachment = this.DbContext.GZ_Attachment.Where(x => x.SourceId == master.Id).FirstOrDefault();
            //if (attachment == null)
            //    throw new ArgumentException("未找到调薪记录的附件信息");
            this.DbContext.GZ_EmployeeSalary.Remove(master);
            if (attachment != null)
                this.DbContext.GZ_Attachment.Remove(attachment);
            return this.DbContext.SaveChanges();
        }

        /// <summary>
        /// HR对一个或多个调薪记录发起审核
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public int ApproveByHR(StartApproveBatch parameter)
        {
            var lstWrapper = this.DbContext.GZ_EmployeeSalary.AsNoTracking().Where(x => parameter.TargetIds.Contains(x.Id))
                .Join(this.DbContext.GZ_Employee, x => x.EmployeeId, y => y.Id, (x, y) => new { ShortSalary = x, Employee = y })
                .ToList();
            var lstId = lstWrapper.Select(x => x.ShortSalary.Id).ToList();
            var lstEx = parameter.TargetIds.Except(lstId).ToList();
            if (lstEx.Count > 0)
                throw new ArgumentException(string.Format("指定的调薪记录不存在，Id如下【{0}】", string.Join(",", lstEx)));
            var lstStatus = lstWrapper.Where(x => x.ShortSalary.Status != Enum.ApproveStatus.待发起审核).ToList();
            if (lstStatus.Count > 0)
                throw new ArgumentException(string.Format("不能对【{0}】状态的调薪记录发起审核", string.Join(",", lstStatus.Select(x => x.ShortSalary.Status.ToString()).Distinct())));
            //调薪记录状态
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
                Comment = "无",
                OperatorId = this.UserInfo.Id,
                OperatorTime = DateTime.Now,
                TargetId = x.ShortSalary.Id,
                TargetStatus = (int)x.ShortSalary.Status,
                TargetTable = nameof(DataModel.GZ_EmployeeSalary)
            }).ToList();
            this.DbContext.GZ_ApproveLog.AddRange(lstApprove);
            //短信通知
            //{ TemplateIdEnum.基本工资发起审核,"中力薪酬管家，HR已提交员工{1}的{2}审核申请，请登陆薪酬管家进行审核。" },
            int max = 3;
            var lstPhone = this._user.GetPhoneList(Enum.RoleEnum.Finance);
            var lstParameter = new List<string>();
            lstParameter.Add(string.Join(",", lstWrapper.Take(max).Select(x => x.Employee.Name))+(lstWrapper.Count>max?"等":string.Empty));
            lstParameter.Add("调薪");
            if (lstPhone != null && lstPhone.Count > 0)
                this._sms.SendSms(lstPhone, GZ_SMS.TemplateIdEnum.基本工资发起审核, lstParameter);
            //this.DbContext.Configuration.ValidateOnSaveEnabled = false;
            return this.DbContext.SaveChanges();
        }

        public List<Tuple<GZ_EmployeeSalary, GZ_Employee, GZ_Attachment>> GetEntityByFinance(BonusQueryByApprove parameter)
        {
            var query = this.DbContext.GZ_EmployeeSalary.AsQueryable();
            if (parameter.TabIndex == Models.TabEnum.待审核)
                query = query.Where(x => x.Status == Enum.ApproveStatus.待财务审核);
            if (parameter.TabIndex == Models.TabEnum.已审核)
                query = query.Where(x => x.Status == Enum.ApproveStatus.财务同意 || x.Status == Enum.ApproveStatus.财务否决);
            var queryEmployee = this.DbContext.GZ_Employee.AsQueryable();
            if (!string.IsNullOrEmpty(parameter.Name))
                queryEmployee = queryEmployee.Where(x => x.Name.Contains(parameter.Name));
            var lst = query.Join(queryEmployee, x => x.EmployeeId, y => y.Id, (x, y) => new { ShortSalary = x, Employee = y })
                .GroupJoin(this.DbContext.GZ_Attachment, x => x.ShortSalary.Id, y => y.SourceId, (x, y) => new { x.ShortSalary, x.Employee, Attachment = y.FirstOrDefault() })
                .OrderByDescending(x => new {  x.ShortSalary.Status,x.ShortSalary.CreateDate }).ToList();
            return lst.Select(x => Tuple.Create(x.ShortSalary, x.Employee, x.Attachment)).ToList();
        }

        /// <summary>
        /// 财务审核调薪
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public int ApproveByFinance(ApproveBatchInput parameter)
        {
            var lstWrapper = this.DbContext.GZ_EmployeeSalary.AsNoTracking().Where(x => parameter.TargetIds.Contains(x.Id))
                .Join(this.DbContext.GZ_Employee, x => x.EmployeeId, y => y.Id, (x, y) => new { ShortSalary = x, Employee = y })
                .ToList();
            var lstId = lstWrapper.Select(x => x.ShortSalary.Id).ToList();
            var lstEx = parameter.TargetIds.Except(lstId).ToList();
            if (lstEx.Count > 0)
                throw new ArgumentException(string.Format("指定的调薪记录不存在，Id如下【{0}】", string.Join(",", lstEx)));
            var lstStatus = lstWrapper.Where(x => x.ShortSalary.Status != Enum.ApproveStatus.待财务审核).ToList();
            if (lstStatus.Count > 0)
                throw new ArgumentException(string.Format("不能对【{0}】状态的调薪记录发起审核", string.Join(",", lstStatus.Select(x => x.ShortSalary.Status.ToString()).Distinct())));
            //调薪记录状态
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
                TargetTable = nameof(DataModel.GZ_EmployeeSalary)
            }).ToList();
            this.DbContext.GZ_ApproveLog.AddRange(lstApprove);
            //短信通知
            //{ TemplateIdEnum.基本工资审核结果,"中力薪酬管家，财务{1}{2}的{3}审核申请，审核意见：{4}" },
            int max = 3;
            var lstPhone = this._user.GetPhoneList(Enum.RoleEnum.HR);
            var lstParameter = new List<string>();
            lstParameter.Add(parameter.Handler == GZ_ApproveLog.ApproveLogCategory.Through ? "同意" : "否决");
            lstParameter.Add(string.Join(",", lstWrapper.Take(max).Select(x => x.Employee.Name))+(lstWrapper.Count>max?"等":string.Empty));
            lstParameter.Add("调薪");
            lstParameter.Add(parameter.Comment);
            if (lstPhone != null && lstPhone.Count > 0)
                this._sms.SendSms(lstPhone, GZ_SMS.TemplateIdEnum.基本工资审核结果, lstParameter);
            return this.DbContext.SaveChanges();
        }
    }
}