using Salary.Common;
using Salary_MVC.Common;
using Salary_MVC.Data;
using Salary_MVC.DataModel;
using Salary_MVC.Enum;
using Salary_MVC.Models;
using Salary_MVC.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Salary_MVC.Services
{
    public class EmployeeService : Service<GZ_Employee>
    {
        private readonly SMSService _sms = new SMSService();
        private readonly UserService _user = new UserService();
        internal object GetEmployeeList(EmployeeQueryDto dto)
        {
            int skip = dto.PageSize * (dto.PageIndex - 1);
            var model = new BasePaging();
            var lastMonth = DateTime.Now.AddMonths(-1).AddDays(1 - DateTime.Now.AddMonths(-1).Day).Date;
            var _employee = Entities.Where(o => o.StatusJob == 1 || !o.QuitDate.HasValue || o.QuitDate.Value > lastMonth);
            if (dto.CompanyId.HasValue)
            {
                _employee = _employee.Where(o => o.CorpId == dto.CompanyId);
            }
            if (dto.DepartmentId.HasValue)
            {
                _employee = _employee.Where(o => o.DepartmentId == dto.DepartmentId);
            }
            if (dto.Active.HasValue)
            {
                _employee = _employee.Where(o => o.StatusJob == dto.Active.Value);
            }
            if (!string.IsNullOrEmpty(dto.TrueName))
            {
                _employee = _employee.Where(o => o.Name.Contains(dto.TrueName));
            }
            if (dto.LockStatus.HasValue)
            {
                switch (dto.LockStatus.Value)
                {
                    case 1:
                        _employee = _employee.Where(o => o.Status == (int)EmployeeStatusEnum.Locked);
                        break;
                    case 2:
                        _employee = _employee.Where(o => o.Status == (int)EmployeeStatusEnum.Locking);
                        break;
                    case 3:
                        _employee = _employee.Where(o => o.Status == (int)EmployeeStatusEnum.Unlocked);
                        break;
                    case 4:
                        _employee = _employee.Where(o => o.Status == (int)EmployeeStatusEnum.Lock);
                        break;
                    default:
                        break;
                }
            }
            model.CurrentPage = dto.PageIndex;
            model.PageSize = dto.PageSize;
            model.TotalCount = _employee.Count();
            model.TotalPage = (int)Math.Ceiling(model.TotalCount * 1.0 / model.PageSize);
            List<GZ_Department> departmentList = DbContext.GZ_Department.ToList();
            List<GZ_FinancialUnit> financialUnitList = DbContext.GZ_FinancialUnit.ToList();
            int finance = DbContext.GZ_UserRole.Where(ur => ur.UserId == UserInfo.Id && DbContext.GZ_Role.Where(r => r.Id == ur.RoleId).FirstOrDefault().Code == (int)RoleEnum.Finance).Count();
            int hr = DbContext.GZ_UserRole.Where(ur => ur.UserId == UserInfo.Id && DbContext.GZ_Role.Where(r => r.Id == ur.RoleId).FirstOrDefault().Code == (int)RoleEnum.HR).Count();
            model.List = _employee.OrderByDescending(o => o.Status).ThenBy(o => o.JoinDate).ThenBy(o => o.Name).Skip(skip).Take(dto.PageSize).ToList().Select(o => new { o.Id, o.Name, DepartmentName = departmentList.Where(d => d.Id == o.DepartmentId).FirstOrDefault().Name, o.FinacialUnitId, FinacialUnitName = o.FinacialUnitId.HasValue ? financialUnitList.FirstOrDefault(f => f.Id == o.FinacialUnitId)?.Name ?? string.Empty : string.Empty, JoinDate = o.JoinDate.ToString("yyyy-MM-dd"), QuitDate = o.QuitDate?.ToString("yyyy-MM-dd") ?? string.Empty, o.Mobile, IDCard = o.IDCard ?? string.Empty, BankArea = o.BankArea ?? string.Empty, BankCard = o.BankCard ?? string.Empty, o.PaidHoliday, IsLeader = o.IsLeader == 1 ? "是" : "", o.StatusJob, Status = ((EmployeeStatusEnum)o.Status).GetDescription(), SalaryGroup = o.SalaryGroup.HasValue ? ((SalaryGroupEnum)o.SalaryGroup).GetDescription() : string.Empty, HRFunction = hr, FinanceFunction = finance }).ToList();
            return model;
        }

        class SalaryGroupItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        internal object GetEntityById(Guid id)
        {
            FieldInfo[] fields = typeof(SalaryGroupEnum).GetFields();
            List<SalaryGroupItem> list = new List<SalaryGroupItem>();
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].Name == "value__") continue;
                list.Add(new SalaryGroupItem { Id = (int)fields[i].GetValue(null), Name = ((SalaryGroupEnum)fields[i].GetValue(null)).GetDescription() });

            }
            var financeList = DbContext.GZ_FinancialUnit.Select(f => new { f.Id, f.Name }).ToList();

            return Entities.Where(o => o.Id == id).ToList().Select(o => new { o.Status, FinanceList = financeList, SalaryGroupList = list, o.Id, o.Name, o.Mobile, IDCard = o.IDCard ?? string.Empty, o.SalaryGroup, DepartmentName = DbContext.GZ_Department.Where(d => d.Id == o.DepartmentId).FirstOrDefault().Name, o.IsLeader, o.PaidHoliday, BankArea = o.BankArea ?? string.Empty, BankCard = o.BankCard ?? string.Empty, OldBankCard = DbContext.GZ_UpdateHistory.Where(uh => uh.TargetId == o.Id && uh.TargetTable == typeof(GZ_Employee).Name && uh.ColumnName == "BankCard").OrderByDescending(uh => uh.CreateDate).Select(uh => uh.OldValue).FirstOrDefault(), OldBankArea = DbContext.GZ_UpdateHistory.Where(uh => uh.TargetId == o.Id && uh.TargetTable == typeof(GZ_Employee).Name && uh.ColumnName == "BankArea").OrderByDescending(uh => uh.CreateDate).Select(uh => uh.OldValue).FirstOrDefault(), OldIDCard = DbContext.GZ_UpdateHistory.Where(uh => uh.TargetId == o.Id && uh.TargetTable == typeof(GZ_Employee).Name && uh.ColumnName == "IDCard").OrderByDescending(uh => uh.CreateDate).Select(uh => uh.OldValue).FirstOrDefault(), FinacialUnitId = o.FinacialUnitId }).FirstOrDefault();
        }

        internal int UpdatePaidHoliday(UpdateHolidayDto dto)
        {
            HRFunctionCheck();
            var model = Entities.FirstOrDefault(o => o.Id == dto.Id);
            if (model == null) throw new InputException("该员工不存在，请刷新再试");

            List<GZ_UpdateHistory> lstHistory = new List<GZ_UpdateHistory>();
            DataModel.UpdateHistoryActivator<GZ_Employee> activator = new UpdateHistoryActivator<GZ_Employee>(Cookies.User);

            lstHistory.Add(activator.Create(model, x => x.PaidHoliday, dto.PaidHoliday));
            lstHistory.Add(activator.Create(model, x => x.IsLeader, dto.IsLeader));
            lstHistory = lstHistory.Where(o => o.NewValue != o.OldValue).ToList();
            model.PaidHoliday = dto.PaidHoliday;
            model.IsLeader = dto.IsLeader;
            DbContext.GZ_UpdateHistory.AddRange(lstHistory);

            return Update(model);
        }

        internal object ApproveLockByHR(ApplyApproveDto dto)
        {
            var model = Entities.FirstOrDefault(o => o.Id == dto.Id);
            if (model == null) throw new InputException("该员工不存在，请刷新再试");
            if (model.Status != (int)EmployeeStatusEnum.Unlocked) throw new InputException("该员工不是未锁定状态，不能申请锁定");
            model.Status = (int)EmployeeStatusEnum.Locked;

            GZ_ApproveLog approveLog = new GZ_ApproveLog()
            {
                Category = GZ_ApproveLog.ApproveLogCategory.Through,
                Comment = dto.Opinion ?? "无",
                Id = Guid.NewGuid(),
                OperatorId = this.UserInfo.Id,
                OperatorTime = DateTime.Now,
                TargetId = model.Id,
                TargetStatus = model.Status,
                TargetTable = nameof(GZ_Employee)
            };

            var phoneList = _user.GetPhoneList(RoleEnum.Finance);
            foreach (var item in phoneList)
            {
                _sms.SendSms(item, GZ_SMS.TemplateIdEnum.员工信息发起审核, ServiceHelper.GetParams(model.Name, "锁定", Config.BaseAddress + "/H5/EmployeeApproveLock_H5.html?id=" + model.Id.ToString() + "&phone=" + item));
            }
            DbContext.GZ_ApproveLog.Add(approveLog);
            return Update(model);
        }

        internal object ApproveUnlockMultiByFinance(ApproveMultipleDto dto)
        {
            var list = Entities.Where(o => dto.Ids.Contains(o.Id)).ToList();
            if (list == null || list.Count == 0) throw new InputException("该员工不存在，请刷新再试");
            List<GZ_ApproveLog> approvelogList = new List<GZ_ApproveLog>();
            foreach (var item in list)
            {
                if (item.Status != (int)EmployeeStatusEnum.Locking) throw new InputException("该员工状态不是解锁中，不能执行解锁");
                if (dto.UserOperation == GZ_ApproveLog.ApproveLogCategory.Through)
                {
                    item.Status = (int)EmployeeStatusEnum.Unlocked;
                }
                else if (dto.UserOperation == GZ_ApproveLog.ApproveLogCategory.NotThrough)
                {
                    item.Status = (int)EmployeeStatusEnum.Lock;
                }
                var entry = DbContext.Entry<GZ_Employee>(item);
                entry.State = System.Data.Entity.EntityState.Modified;
                approvelogList.Add(new GZ_ApproveLog()
                {
                    Category = dto.UserOperation,
                    Comment = dto.Opinion ?? "无",
                    Id = Guid.NewGuid(),
                    OperatorId = this.UserInfo.Id,
                    OperatorTime = DateTime.Now,
                    TargetId = item.Id,
                    TargetStatus = item.Status,
                    TargetTable = nameof(GZ_Employee)
                });
                _sms.SendSms(_user.GetPhoneList(RoleEnum.HR), GZ_SMS.TemplateIdEnum.员工信息审核结果, ServiceHelper.GetParams(dto.UserOperation == GZ_ApproveLog.ApproveLogCategory.Through ? "同意" : dto.UserOperation == GZ_ApproveLog.ApproveLogCategory.NotThrough ? "否决" : "",
                    item.Name,
                    "解锁",
                    dto.Opinion ?? "无"));
            }

            DbContext.GZ_ApproveLog.AddRange(approvelogList);
            return DbContext.SaveChanges();
        }

        internal object GetEmployeeStatus(Guid id)
        {
            var model = Entities.FirstOrDefault(o => o.Id == id);
            if (model == null) throw new InputException("该员工不存在，请刷新再试");
            List<DataModel.GZ_ApproveLog> lst = this.DbContext.GZ_ApproveLog.Where(x => x.TargetId == model.Id).OrderBy(x => x.OperatorTime).ToList();
            List<DataModel.GZ_User> lstUser = _user.GetAllUser();
            var dictUser = lstUser.ToDictionary(x => x.Id, x => x.Name);
            return lst.Select(x => new
            {
                Name = dictUser.ContainsKey(x.OperatorId) ? dictUser[x.OperatorId] : "admin",
                OperatorTime = x.OperatorTime.ToString("yyyy-MM-dd HH:mm"),
                TargetStatus = ((EmployeeStatusEnum)x.TargetStatus).GetDescription(),
                Handler = x.Category == DataModel.GZ_ApproveLog.ApproveLogCategory.Through ? "通过" : "不通过",
                x.Comment
            }).ToList();
        }

        public void FinanceCheck()
        {
            int finance = DbContext.GZ_UserRole.Where(ur => ur.UserId == UserInfo.Id && DbContext.GZ_Role.Where(r => r.Id == ur.RoleId).FirstOrDefault().Code == (int)RoleEnum.Finance).Count();
            if (finance == 0) throw new InputException("只有财务才能编辑");
        }

        internal object UpdateFinancialUnit(UpdateFinancialDto dto)
        {
            FinanceCheck();
            var model = Entities.FirstOrDefault(o => o.Id == dto.Id);
            if (model == null) throw new InputException("该员工不存在，请刷新再试");

            List<GZ_UpdateHistory> lstHistory = new List<GZ_UpdateHistory>();
            DataModel.UpdateHistoryActivator<GZ_Employee> activator = new UpdateHistoryActivator<GZ_Employee>(Cookies.User);

            lstHistory.Add(activator.Create(model, x => x.SalaryGroup, (int)dto.SalaryGroup));
            lstHistory.Add(activator.Create(model, x => x.FinacialUnitId, dto.FinacialUnitId));
            lstHistory = lstHistory.Where(o => o.NewValue != o.OldValue).ToList();
            model.SalaryGroup = (int)dto.SalaryGroup;
            model.FinacialUnitId = dto.FinacialUnitId;
            this.DbContext.GZ_UpdateHistory.AddRange(lstHistory);
            return Update(model);
        }

        internal object ApproveLockMultiByFinance(ApproveMultipleDto dto)
        {
            var list = Entities.Where(o => dto.Ids.Contains(o.Id)).ToList();
            if (list == null || list.Count == 0) throw new InputException("该员工不存在，请刷新再试");
            List<GZ_ApproveLog> approvelogList = new List<GZ_ApproveLog>();

            foreach (var item in list)
            {
                if (item.Status != (int)EmployeeStatusEnum.Locked) throw new InputException("员工状态不是锁定中，不能执行锁定申请");


                if (dto.UserOperation == GZ_ApproveLog.ApproveLogCategory.Through)
                {
                    item.Status = (int)EmployeeStatusEnum.Lock;
                }
                else if (dto.UserOperation == GZ_ApproveLog.ApproveLogCategory.NotThrough)
                {
                    item.Status = (int)EmployeeStatusEnum.Unlocked;
                }
                var entry = DbContext.Entry<GZ_Employee>(item);
                entry.State = System.Data.Entity.EntityState.Modified;
                approvelogList.Add(new GZ_ApproveLog()
                {
                    Category = dto.UserOperation,
                    Comment = dto.Opinion ?? "无",
                    Id = Guid.NewGuid(),
                    OperatorId = this.UserInfo.Id,
                    OperatorTime = DateTime.Now,
                    TargetId = item.Id,
                    TargetStatus = item.Status,
                    TargetTable = nameof(GZ_Employee)
                });
                _sms.SendSms(_user.GetPhoneList(RoleEnum.HR),
                    GZ_SMS.TemplateIdEnum.员工信息审核结果,
                    ServiceHelper.GetParams(dto.UserOperation == GZ_ApproveLog.ApproveLogCategory.Through ? "同意" : dto.UserOperation == GZ_ApproveLog.ApproveLogCategory.NotThrough ? "否决" : "", item.Name, "锁定", dto.Opinion ?? "无"));

            }
            DbContext.GZ_ApproveLog.AddRange(approvelogList);
            return DbContext.SaveChanges();
        }

        public void HRFunctionCheck()
        {
            int hr = DbContext.GZ_UserRole.Where(ur => ur.UserId == UserInfo.Id && DbContext.GZ_Role.Where(r => r.Id == ur.RoleId).FirstOrDefault().Code == (int)RoleEnum.HR).Count();
            if (hr == 0) throw new InputException("只有HR才能编辑！");
        }
        internal object UpdateIDCard(UpdateIdCardDto dto)
        {
            HRFunctionCheck();
            var model = Entities.FirstOrDefault(o => o.Id == dto.Id);
            if (model == null) throw new InputException("该员工不存在，请刷新再试");
            List<GZ_UpdateHistory> lstHistory = new List<GZ_UpdateHistory>();
            DataModel.UpdateHistoryActivator<GZ_Employee> activator = new UpdateHistoryActivator<GZ_Employee>(Cookies.User);

            lstHistory.Add(activator.Create(model, x => x.BankArea, dto.BankArea));

            lstHistory.Add(activator.Create(model, x => x.BankCard, dto.BankCard));

            lstHistory.Add(activator.Create(model, x => x.IDCard, dto.IDCard));
            lstHistory = lstHistory.Where(o => o.NewValue != o.OldValue).ToList();
            model.BankArea = dto.BankArea;
            model.BankCard = dto.BankCard;
            model.IDCard = dto.IDCard;
            DbContext.GZ_UpdateHistory.AddRange(lstHistory);

            model.BankArea = dto.BankArea;
            model.BankCard = dto.BankCard;
            model.IDCard = dto.IDCard;
            if (dto.IsLock == 1)//申请加锁
            {
                if (model.Status != (int)EmployeeStatusEnum.Unlocked) throw new InputException("该员工不是未锁定状态，不能申请锁定");
                model.Status = (int)EmployeeStatusEnum.Locked;

                GZ_ApproveLog approveLog = new GZ_ApproveLog()
                {
                    Category = GZ_ApproveLog.ApproveLogCategory.Through,
                    Comment = dto.Opinion ?? "无",
                    Id = Guid.NewGuid(),
                    OperatorId = this.UserInfo.Id,
                    OperatorTime = DateTime.Now,
                    TargetId = model.Id,
                    TargetStatus = model.Status,
                    TargetTable = nameof(GZ_Employee)
                };
                DbContext.GZ_ApproveLog.Add(approveLog);
                var phoneList = _user.GetPhoneList(RoleEnum.Finance);
                foreach (var item in phoneList)
                {
                    _sms.SendSms(item, GZ_SMS.TemplateIdEnum.员工信息发起审核, ServiceHelper.GetParams(model.Name, "锁定", Config.BaseAddress + "/H5/EmployeeApproveLock_H5.html?id=" + model.Id.ToString() + "&phone=" + item));
                }
            }
            var entry = DbContext.Entry<GZ_Employee>(model);
            entry.State = System.Data.Entity.EntityState.Modified;
            return DbContext.SaveChanges();
        }

        public List<GZ_Employee> GetEntityWithKeyValueByDepartmentId(Guid id)
        {
            var query = this.DbContext.GZ_Employee.Where(x => x.StatusJob == 1);
            if (id != Guid.Empty)
                query = query.Where(x => x.DepartmentId == id);
            var lst = query.OrderBy(x => x.Name).ToList();
            return lst;
        }


        internal object GetSalaryHistory(EmployeeSalaryDto dto)
        {
            var checkCode = SessionHelper.GetSessionString("check_code");
            if (checkCode != dto.CheckCode) throw new InputException("验证码不正确");
            var model = Entities.FirstOrDefault(o => o.Id == dto.Id);
            if (model == null) throw new InputException("该员工不存在，请刷新再试");
            return DbContext.GZ_EmployeeSalary.Where(o => o.EmployeeId == model.Id).OrderBy(o => o.EffectedDate).ToList().Select(o => new { o.Comment, EffectedDate = o.EffectedDate.ToString("yyyy-MM-dd"), o.Money, Status = o.Status.ToString() }).ToList();
        }

        internal int Discharge(Guid id)
        {
            var model = Entities.FirstOrDefault(o => o.Id == id);
            if (model == null) throw new InputException("该员工不存在，请刷新再试");
            if (model.StatusJob != 1) throw new InputException("非在职员工，不能操作辞职");
            model.StatusJob = 2;
            return Update(model);
        }

        /// <summary>
        /// 财务审核锁定申请
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal object ApproveLockByFinance(ApproveDto dto)
        {
            var model = Entities.FirstOrDefault(o => o.Id == dto.Id);
            if (model == null) throw new InputException("该员工不存在，请刷新再试");
            if (model.Status != (int)EmployeeStatusEnum.Locked) throw new InputException("该员工状态不是锁定中，不能执行锁定申请");
            var operatorModel = DbContext.GZ_User.Where(o => o.UserName == dto.Phone && o.Status == 0).FirstOrDefault();
            if (operatorModel == null) throw new InputException("该审核人不存在，请核对手机号码");
            if (dto.UserOperation == GZ_ApproveLog.ApproveLogCategory.Through)
            {
                model.Status = (int)EmployeeStatusEnum.Lock;
            }
            else if (dto.UserOperation == GZ_ApproveLog.ApproveLogCategory.NotThrough)
            {
                model.Status = (int)EmployeeStatusEnum.Unlocked;
            }

            GZ_ApproveLog approveLog = new GZ_ApproveLog()
            {
                Category = dto.UserOperation,
                Comment = dto.Opinion ?? "无",
                Id = Guid.NewGuid(),
                OperatorId = operatorModel.Id,
                OperatorTime = DateTime.Now,
                TargetId = model.Id,
                TargetStatus = model.Status,
                TargetTable = nameof(GZ_Employee)
            };
            _sms.SendSms(_user.GetPhoneList(RoleEnum.HR), GZ_SMS.TemplateIdEnum.员工信息审核结果, ServiceHelper.GetParams(dto.UserOperation == GZ_ApproveLog.ApproveLogCategory.Through ? "同意" : dto.UserOperation == GZ_ApproveLog.ApproveLogCategory.NotThrough ? "否决" : "", model.Name, "锁定", dto.Opinion ?? "无"), operatorModel.Id);
            DbContext.GZ_ApproveLog.Add(approveLog);
            return Update(model);
        }

        /// <summary>
        /// 辞退当月的考勤和基本工资
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal object GetAttendanceByDischarge(Guid id)
        {
            var model = Entities.FirstOrDefault(o => o.Id == id);
            if (model == null) throw new InputException("该员工不存在，请刷新再试");
            //员工的考勤和基本工资

            return null;
        }

        internal object UnDischarge(Guid id)
        {
            var model = Entities.FirstOrDefault(o => o.Id == id);
            if (model == null) throw new InputException("该员工不存在，请刷新再试");
            if (model.StatusJob != 2) throw new InputException("该员工没有被辞退，不能撤回");
            if (!model.QuitDate.HasValue || model.QuitDate.Value.AddDays(3) < DateTime.Now) throw new InputException("该员工超过三天，不能再撤回");
            model.StatusJob = 1;
            return Update(model);
        }

        internal object ApproveUnlockByFinance(ApproveDto dto)
        {
            var model = Entities.FirstOrDefault(o => o.Id == dto.Id);
            if (model == null) throw new InputException("该员工不存在，请刷新再试");
            var operatorModel = DbContext.GZ_User.Where(o => o.UserName == dto.Phone && o.Status == 0).FirstOrDefault();
            if (operatorModel == null) throw new InputException("该审核人不存在，请核对手机号码");
            if (model.Status != (int)EmployeeStatusEnum.Locking) throw new InputException("该员工状态不是解锁中，不能执行解锁");
            if (dto.UserOperation == GZ_ApproveLog.ApproveLogCategory.Through)
            {
                model.Status = (int)EmployeeStatusEnum.Unlocked;
            }
            else if (dto.UserOperation == GZ_ApproveLog.ApproveLogCategory.NotThrough)
            {
                model.Status = (int)EmployeeStatusEnum.Lock;
            }

            GZ_ApproveLog approveLog = new GZ_ApproveLog()
            {
                Category = dto.UserOperation,
                Comment = dto.Opinion ?? "无",
                Id = Guid.NewGuid(),
                OperatorId = operatorModel.Id,
                OperatorTime = DateTime.Now,
                TargetId = model.Id,
                TargetStatus = model.Status,
                TargetTable = nameof(GZ_Employee)
            };
            _sms.SendSms(_user.GetPhoneList(RoleEnum.HR), GZ_SMS.TemplateIdEnum.员工信息审核结果, ServiceHelper.GetParams(dto.UserOperation == GZ_ApproveLog.ApproveLogCategory.Through ? "同意" : dto.UserOperation == GZ_ApproveLog.ApproveLogCategory.NotThrough ? "否决" : "", model.Name, "解锁", dto.Opinion ?? "无"), operatorModel.Id);
            DbContext.GZ_ApproveLog.Add(approveLog);
            return Update(model);
        }

        /// <summary>
        /// 导出辞退的工资表excel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal object ExportAttendanceByDischarge(Guid id)
        {
            throw new NotImplementedException();
        }

        internal bool SendSalaryCheckCode()
        {
            if (!Cookies.UserCode.HasValue) throw new InputException("请重新登陆");
            var pwd = this.RandomPwd(6); //"123456";
            SessionHelper.SetSession("check_code", pwd, 10);
            var result = _sms.SendSmsByNow(Cookies.UserName.Substring(0, 11), GZ_SMS.TemplateIdEnum.验证码, ServiceHelper.GetParams(pwd));
            return result.errmsg.ToUpper() == "OK";
        }

        private string RandomPwd(int length)
        {
            try
            {
                string reValue = string.Empty;
                Random r = new Random();
                while (reValue.Length < length)
                {
                    string s1 = r.Next(0, 10).ToString();
                    reValue += s1;
                }
                return reValue;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return string.Empty;
            }
        }

        internal int ApproveUnlockByHR(ApplyApproveDto dto)
        {
            var model = Entities.FirstOrDefault(o => o.Id == dto.Id);
            if (model == null) throw new InputException("该员工不存在，请刷新再试");
            if (model.Status != (int)EmployeeStatusEnum.Lock) throw new InputException("该员工不是锁定状态，不能申请解锁");
            model.Status = (int)EmployeeStatusEnum.Locking;

            GZ_ApproveLog approveLog = new GZ_ApproveLog()
            {
                Category = GZ_ApproveLog.ApproveLogCategory.Through,
                Comment = dto.Opinion ?? "无",
                Id = Guid.NewGuid(),
                OperatorId = this.UserInfo.Id,
                OperatorTime = DateTime.Now,
                TargetId = model.Id,
                TargetStatus = model.Status,
                TargetTable = nameof(GZ_Employee)
            };
            var phoneList = _user.GetPhoneList(RoleEnum.Finance);
            foreach (var item in phoneList)
            {
                _sms.SendSms(item, GZ_SMS.TemplateIdEnum.员工信息发起审核, ServiceHelper.GetParams(model.Name, "解锁", Config.BaseAddress + "/H5/EmployeeApproveUnlock_H5.html?id=" + model.Id.ToString() + "&phone=" + item));
            }

            DbContext.GZ_ApproveLog.Add(approveLog);
            return Update(model);
        }

        public List<GZ_Employee> GetAllEmployee()
        {
            var lst = this.DbContext.GZ_Employee.ToList();
            return lst;
        }
    }
}