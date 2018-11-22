using Salary_MVC.Common;
using Salary_MVC.Data;
using Salary_MVC.DataModel;
using Salary_MVC.Enum;
using Salary_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Web;
using Salary.Common;

namespace Salary_MVC.Services
{
    public class AttendanceService : Service<GZ_Attendance>
    {
        private readonly SMSService _sms = new SMSService();
        private readonly UserService _user = new UserService();
        private readonly EmployeeService _employee = new EmployeeService();
        private static object _lock = new object();
        internal object GetAttendanceList(AttendanceQueryDto dto)
        {
            //int skip = dto.PageSize * (dto.PageIndex - 1);
            var model = new BasePaging();
            var _attendance = Entities;
            if (!string.IsNullOrEmpty(dto.Month))
            {
                _attendance = _attendance.Where(o => o.Month == dto.Month);
                model.ExistsGenerateAttendance = Entities.Where(o=>o.Month==dto.Month&&o.DataSourceType==0).Count()>0?1:0;
            }
            if (dto.CompanyId.HasValue)
            {
                _attendance = _attendance.Where(o => o.CompanyId == dto.CompanyId.Value);
            }
            if (dto.DepartmentId.HasValue)
            {
                _attendance = _attendance.Where(o => o.DepartmentId == dto.DepartmentId);
            }
            if (!string.IsNullOrEmpty(dto.TrueName))
            {
                _attendance = _attendance.Where(o => o.Name.Contains(dto.TrueName));
            }
            if (dto.AttendanceStatus.HasValue)
            {
                if (dto.AttendanceStatus.Value == 1)//待审核
                {
                    _attendance = _attendance.Where(o => o.Status == (int)AttendanceStatusEnum.UserAgree);
                }
                else if (dto.AttendanceStatus.Value == 2)//人事已审核
                {
                    _attendance = _attendance.Where(o => o.Status == (int)AttendanceStatusEnum.HRPass || o.Status == (int)AttendanceStatusEnum.HRDisAgree || o.Status == (int)AttendanceStatusEnum.SystemPass);
                }
                else if (dto.AttendanceStatus.Value == 3)//强制审核
                {
                    _attendance = _attendance.Where(o => o.Status != (int)AttendanceStatusEnum.HRPass && o.Status != (int)AttendanceStatusEnum.SystemPass);
                }
            }
            //model.CurrentPage = dto.PageIndex;
            // model.PageSize = dto.PageSize;
            // model.TotalCount = _attendance.Count();
            // model.TotalPage = (int)Math.Ceiling(model.TotalCount * 1.0 / model.PageSize);
            
            model.List = _attendance.OrderByDescending(o=>o.Status).ThenBy(o => o.KProjectId)./*Skip(skip).Take(dto.PageSize).*/ToList().Select(o => new
            { o.Id, o.Name, o.DataSourceType, DepartmentName = DbContext.GZ_Department.Where(d => d.Id == o.DepartmentId).FirstOrDefault().Name, o.Mobile, o.Month, o.TotalDays, o.RealDays, o.AbsenteeismDays, o.FinalDays, o.AnnualLeave, o.BreakDown, o.CompassionateLeave, OtherLeave = o.OtherLeave, o.SickLeave, o.Remark, o.Status, StatusName = ((AttendanceStatusEnum)o.Status).GetDescription(true) }).ToList();
            
            return model;
        }

        

        internal object ApproveByEmployee(ApproveDto dto)
        {
            var model = Entities.FirstOrDefault(o => o.Id == dto.Id);
            if (model == null) throw new InputException("该考勤不存在，请刷新再试");
            if (model.Status != (int)AttendanceStatusEnum.WaitUser) throw new InputException("该考勤不是待用户确认状态，不能操作确认");
            if (dto.UserOperation == GZ_ApproveLog.ApproveLogCategory.Through)
            {
                model.Status = (int)AttendanceStatusEnum.UserAgree;
            }
            else if (dto.UserOperation == GZ_ApproveLog.ApproveLogCategory.NotThrough)
            {
                model.Status = (int)AttendanceStatusEnum.UserDisAgree;
            }

            GZ_ApproveLog approveLog = new GZ_ApproveLog()
            {
                Category = dto.UserOperation,
                Comment = dto.Opinion ?? "无",
                Id = Guid.NewGuid(),
                OperatorId = model.EmployeeId,
                OperatorTime = DateTime.Now,
                TargetId = model.Id,
                TargetStatus = model.Status,
                TargetTable = nameof(GZ_Attendance)
            };
            DbContext.GZ_ApproveLog.Add(approveLog);
            return Update(model);
        }
        public class ImportResult
        {
            /// <summary>
            /// result=1，导入成功，result=2导入失败，返回文本地址
            /// </summary>
            public int Result { get; set; }
            public string Message { get; set; }
        }

        internal ImportResult Import(ImportInput dto, string fullFilePath)
        {
            //把数据取出放到datatable
            var ds = Salary_MVC.ImportExcel.GetExcelData(fullFilePath);
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                throw new ArgumentException("excel中没有数据，必须将要上传的数据放在第一个sheet中");
            System.Data.DataTable table = ds.Tables[0];
            //模板约定的标题
            //所属部门	姓名	手机号	应出勤天数	实际出勤天数	缺勤天数	病假/时	事假/时	调休/时	年假/时	其他带薪假/时

            string[] heardTextCollection = new string[] { "姓名", "手机号", "应出勤天数", "实际出勤天数", "缺勤天数", "病假/时", "事假/时", "调休/时", "年假/时", "其他带薪假/时", "备注" };
            var dictIndexAndHeadName = table.Columns.Cast<System.Data.DataColumn>().Where(x => heardTextCollection.Contains(x.ColumnName)).ToDictionary(x => x.ColumnName, x => x.Ordinal);
            if (dictIndexAndHeadName.Count < heardTextCollection.Length)
                throw new ArgumentException("excel中缺少必须的列，请按照模板格式上传数据");
            else if (dictIndexAndHeadName.Count > heardTextCollection.Length)
                throw new ArgumentException("excel中具有相同名称的列，请修改后再上传");

            var rows = table.Select();
            List<GZ_Attendance> attendanceList = new List<GZ_Attendance>();
            List<User> userList = DbContext.User.ToList();
            List<GZ_Employee> employeeList = DbContext.GZ_Employee.ToList();
            List<GZ_Department> departmentList = DbContext.GZ_Department.ToList();
            List<GZ_Attendance> monthAttendanceList = DbContext.GZ_Attendance.Where(o => o.Month == dto.Month && o.DataSourceType == 1).ToList();
            bool isImport = true;
            string dirc = HttpContext.Current.Server.MapPath("/log/improt/" + DateTime.Now.ToString("yyyy-MM-dd") + "/");
            if (!Directory.Exists(dirc))
            {
                Directory.CreateDirectory(dirc);
            }
            string fileName = dirc + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".txt";
            using (var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8))
                {

                    foreach (var row in rows)
                    {
                        var IsExistsAttendance = monthAttendanceList.Where(o => o.Name == row[dictIndexAndHeadName["姓名"]].ToString() && o.Mobile == row[dictIndexAndHeadName["手机号"]].ToString()).FirstOrDefault();
                        if (IsExistsAttendance != null)
                        {
                            sw.WriteLine("数据库已存在考勤记录--" + IsExistsAttendance.Name);
                            isImport = false;
                            continue;
                        }
                        var employee = employeeList.Where(o => o.Mobile == row[dictIndexAndHeadName["手机号"]].ToString()).FirstOrDefault();
                        if (employee == null)
                        {
                            sw.WriteLine("数据库不存在手机号为" + row[dictIndexAndHeadName["姓名"]].ToString() + "的用户--");
                            isImport = false;
                            continue;
                        }
                    }
                }
            }
            if (isImport)
            {
                foreach (var row in rows)
                {//解析数据
                    GZ_Attendance attendance = new GZ_Attendance();
                    Common.AsposeSet<DataModel.GZ_Attendance> asposeSet = new Common.AsposeSet<GZ_Attendance>();
                    attendance.Id = Guid.NewGuid();
                    attendance.Name = row[dictIndexAndHeadName["姓名"]].ToString();
                    attendance.Mobile = row[dictIndexAndHeadName["手机号"]].ToString();
                    var employee = employeeList.Where(o => o.Mobile == attendance.Mobile).FirstOrDefault();
                    var department = departmentList.Where(o => o.Id == employee.DepartmentId).FirstOrDefault();
                    attendance.CompanyId = department.CpId;
                    attendance.DepartmentId = department.Id;
                    attendance.EmployeeId = employee.Id;
                    attendance.OtherLeave = 60 * row[dictIndexAndHeadName["其他带薪假/时"]].ToString().DecimalValue();
                    attendance.AnnualLeave = 60 * row[dictIndexAndHeadName["年假/时"]].ToString().DecimalValue();
                    attendance.CompassionateLeave = 60 * row[dictIndexAndHeadName["事假/时"]].ToString().DecimalValue();
                    attendance.SickLeave = 60 * row[dictIndexAndHeadName["病假/时"]].ToString().DecimalValue();
                    attendance.BreakDown = 60 * row[dictIndexAndHeadName["调休/时"]].ToString().DecimalValue();
                    attendance.TotalDays = row[dictIndexAndHeadName["应出勤天数"]].ToString().DecimalValue();
                    attendance.RealDays = row[dictIndexAndHeadName["实际出勤天数"]].ToString().DecimalValue();
                    attendance.AbsenteeismDays = row[dictIndexAndHeadName["缺勤天数"]].ToString().DecimalValue();
                    attendance.FinalDays = attendance.RealDays;
                    attendance.BelateByHour = 0;
                    attendance.BelateMinute = 0;
                    attendance.Remark = row[dictIndexAndHeadName["备注"]].ToString();
                    attendance.CreateDate = DateTime.Now;
                    attendance.CreateUser = UserInfo.Id;
                    attendance.KProjectId = userList.Where(o => o.TrueName == attendance.Name && o.PhoneNum == attendance.Mobile).FirstOrDefault()?.ID ?? 0;
                    attendance.LastUpdateDate = DateTime.Now;
                    attendance.LastUpdateUser = UserInfo.Id;
                    attendance.LeaveEarlyMinute = 0;
                    attendance.LeaveMinute = 0;
                    attendance.Month = dto.Month;
                    attendance.OutsideMinute = 0;
                    attendance.Status = (int)AttendanceStatusEnum.UserAgree;
                    attendance.DataSourceType = 1;
                    attendanceList.Add(attendance);
                }
                DbContext.GZ_Attendance.AddRange(attendanceList);
            }
            ImportResult result = new ImportResult();

            if (isImport)
            {
                int r = this.DbContext.SaveChanges();
                result.Result = 1;
                result.Message = $"操作成功，总共导入{r}条记录";
                return result;
            }
            result.Result = 2;
            result.Message = "/log/improt/" + DateTime.Now.ToString("yyyy-MM-dd") + "/" + Path.GetFileName(fileName);
            return result;
        }


        internal object SendApproveMessageToEmployee(List<Guid> ids)
        {
            if (ids == null || ids.Count == 0) throw new InputException("请认真选择需要补发链接的考勤人员");
            var list = DbContext.GZ_Attendance.Where(o => ids.Contains(o.Id)).Where(o => o.Status == (int)AttendanceStatusEnum.WaitUser).ToList();
            if (list == null || list.Count == 0) throw new InputException("没有可以补发链接的考勤人员");
            List<string> phoneList = list.Select(o => o.Mobile).ToList();
            list.ForEach(o =>
            {
                o.Status = (int)AttendanceStatusEnum.WaitUser;
                _sms.SendSms(phoneList, GZ_SMS.TemplateIdEnum.考勤确认, ServiceHelper.GetParams(Convert.ToDateTime(o.Month).ToString("yyyy年MM月"), Config.BaseAddress + "/H5/AttendanceEmployeeApprove.html?id=" + o.Id.ToString()));
            });
            return DbContext.SaveChanges();
        }

        internal string GetEmployeeApproveAddress(Guid id)
        {
            var model = Entities.FirstOrDefault(o => o.Id == id);
            if (model == null) throw new InputException("该考勤不存在，请刷新再试");
            if (model.Status != (int)AttendanceStatusEnum.WaitUser) throw new InputException("该考勤不是待用户确认状态，不能获取链接");
            return Config.BaseAddress + "/H5/AttendanceEmployeeApprove.html?id=" + id.ToString();
        }

        internal string Export(ExportDto dto)
        {
            var _attendance = Entities.Where(o => o.Month == dto.Month);
            if (dto.CompanyId.HasValue)
            {
                _attendance = _attendance.Where(o => o.CompanyId == dto.CompanyId.Value);
            }
            if (dto.DepartmentId.HasValue)
            {
                _attendance = _attendance.Where(o => o.DepartmentId == dto.DepartmentId);
            }
            if (!string.IsNullOrEmpty(dto.TrueName))
            {
                _attendance = _attendance.Where(o => o.Name.Contains(dto.TrueName));
            }

            var list = _attendance.ToList().Select(o => new
            { o.Id, o.Name, DepartmentName = DbContext.GZ_Department.Where(d => d.Id == o.DepartmentId).FirstOrDefault().Name, o.Mobile, o.Month, o.TotalDays, o.RealDays, o.AbsenteeismDays, o.FinalDays, o.AnnualLeave, o.BreakDown, o.CompassionateLeave, o.OtherLeave, o.SickLeave, o.Remark, o.Status, StatusName = ((AttendanceStatusEnum)o.Status).GetDescription(true) }).ToList();
            var dt = list.ToDataTable();

            if (dt == null) throw new InputException("没有找到导出的数据");
            ExportExcel exp = new ExportExcel();
            ExportExcel.ExcelSheet sheet = new ExportExcel.ExcelSheet("考勤数据（" + dto.Month + "）", dt.DefaultView, ExportTypes.Custom);
            //     o.FinalDays,   o.Remark, o.Status, StatusName = ((AttendanceStatusEnum)o.Status).GetDescription(true)
            sheet.Add("Name", "姓名");
            sheet.Add("DepartmentName", "部门");
            sheet.Add("Mobile", "电话");
            sheet.Add("Month", "考勤月份");
            sheet.Add("TotalDays", "应出勤天数");
            sheet.Add("RealDays", "实际出勤天数");
            sheet.Add("AbsenteeismDays", "缺勤天数");
            sheet.Add("AnnualLeave", "年假/时");
            sheet.Add("BreakDown", "调休/时");
            sheet.Add("CompassionateLeave", "事假/时");
            sheet.Add("OtherLeave", "其它带薪假/时");
            sheet.Add("SickLeave", "病假/时");
            sheet.Add("StatusName", "状态");


            sheet.DisableAutoSetWidth();
            exp.AddSheet(sheet);
            string directory = "~/uploadfile/export/" + UserInfo.Id.ToString() + "/";
            string url = "~/uploadfile/export/" + UserInfo.Id.ToString() + "/" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".xls";
            string result = Config.BaseAddress + url.TrimStart('~');
            directory = System.Web.Hosting.HostingEnvironment.MapPath(directory);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            exp.Export(System.Web.Hosting.HostingEnvironment.MapPath(url));
            return result;
        }

        internal object ApproveAllByHRManager(ApproveAllDto dto)
        {
            var list = DbContext.GZ_Attendance.Where(o => o.Status == (int)AttendanceStatusEnum.UserAgree).ToList();
            if (list == null || list.Count == 0) throw new InputException("没有找到可以审核的数据");
            List<GZ_ApproveLog> approveLogList = new List<GZ_ApproveLog>();
            list.ForEach(o =>
            {
                if (dto.UserOperation == GZ_ApproveLog.ApproveLogCategory.Through)
                {
                    o.Status = (int)AttendanceStatusEnum.HRPass;
                }
                else if (dto.UserOperation == GZ_ApproveLog.ApproveLogCategory.NotThrough)
                {
                    o.Status = (int)AttendanceStatusEnum.HRDisAgree;
                }
                approveLogList.Add(new GZ_ApproveLog()
                {
                    Category = dto.UserOperation,
                    Comment = dto.Opinion ?? "无",
                    Id = Guid.NewGuid(),
                    OperatorId = this.UserInfo.Id,
                    OperatorTime = DateTime.Now,
                    TargetId = o.Id,
                    TargetStatus = o.Status,
                    TargetTable = nameof(GZ_Attendance)
                });
            });
            DbContext.GZ_ApproveLog.AddRange(approveLogList);
            return DbContext.SaveChanges();
        }

        internal object ApproveAllByForce()
        {
            var list = DbContext.GZ_Attendance.Where(o => o.Status != (int)AttendanceStatusEnum.HRPass && o.Status != (int)AttendanceStatusEnum.SystemPass).ToList();
            if (list == null || list.Count == 0) throw new InputException("没有找到可以审核的数据");
            List<GZ_ApproveLog> approveLogList = new List<GZ_ApproveLog>();
            list.ForEach(o =>
            {
                o.Status = (int)AttendanceStatusEnum.SystemPass;
                approveLogList.Add(new GZ_ApproveLog()
                {
                    Category = GZ_ApproveLog.ApproveLogCategory.SysThrough,
                    Comment = "系统强制同意",
                    Id = Guid.NewGuid(),
                    OperatorId = this.UserInfo.Id,
                    OperatorTime = DateTime.Now,
                    TargetId = o.Id,
                    TargetStatus = o.Status,
                    TargetTable = nameof(GZ_Attendance)
                });
            });
            DbContext.GZ_ApproveLog.AddRange(approveLogList);
            return DbContext.SaveChanges();
        }

        internal object ApproveByHRManager(ApproveMultipleDto dto)
        {
            if (dto.Ids == null || dto.Ids.Count == 0) throw new InputException("请认真选择需要审核的人员");
            var list = DbContext.GZ_Attendance.Where(o => dto.Ids.Contains(o.Id)).Where(o => o.Status == (int)AttendanceStatusEnum.UserAgree).ToList();
            if (list == null || list.Count == 0) throw new InputException("没有找到可以审核的数据");
            List<GZ_ApproveLog> approveLogList = new List<GZ_ApproveLog>();
            list.ForEach(o =>
            {
                if (dto.UserOperation == GZ_ApproveLog.ApproveLogCategory.Through)
                {
                    o.Status = (int)AttendanceStatusEnum.HRPass;
                }
                else if (dto.UserOperation == GZ_ApproveLog.ApproveLogCategory.NotThrough)
                {
                    o.Status = (int)AttendanceStatusEnum.HRDisAgree;
                }
                approveLogList.Add(new GZ_ApproveLog()
                {
                    Category = dto.UserOperation,
                    Comment = dto.Opinion ?? "无",
                    Id = Guid.NewGuid(),
                    OperatorId = this.UserInfo.Id,
                    OperatorTime = DateTime.Now,
                    TargetId = o.Id,
                    TargetStatus = o.Status,
                    TargetTable = nameof(GZ_Attendance)
                });
            });
            DbContext.GZ_ApproveLog.AddRange(approveLogList);
            return DbContext.SaveChanges();

        }

        internal object DeleteImportedData(List<Guid> Ids)
        {
            var list = DbContext.GZ_Attendance.Where(o => Ids.Contains(o.Id)).ToList();
            if(list==null||list.Count==0) throw new InputException("请认真勾选考勤");
            if (list.Where(o => o.Status==(int)AttendanceStatusEnum.HRPass|| o.Status == (int)AttendanceStatusEnum.SystemPass).Count() > 0) throw new InputException("已经同意，不能删除，请认真勾选考勤");
            if (list.Where(o=>o.DataSourceType==0).Count()>0) throw new InputException("生成的考勤不能删除");
            DbContext.GZ_Attendance.RemoveRange(list);
            return DbContext.SaveChanges();
        }

        internal object ApporveByHR(AttendanceApporveDto dto)
        {
            if (dto.Ids == null || dto.Ids.Count == 0) throw new InputException("请认真选择需要发起的审核人员");
            var list = DbContext.GZ_Attendance.Where(o => dto.Ids.Contains(o.Id)).ToList();//选中的考勤
            if (list.Where(o => o.Status == (int)AttendanceStatusEnum.HRPass || o.Status == (int)AttendanceStatusEnum.SystemPass || o.Status == (int)AttendanceStatusEnum.UserAgree || o.Status == (int)AttendanceStatusEnum.WaitUser).Count() > 0) throw new InputException("请认真选择需要发起的审核人员");
            if (list.Count(o => o.DataSourceType == 1) > 0) throw new InputException("导入的考勤不能发起审核");
            
            if (list == null || list.Count == 0) throw new InputException("没有可以发起审核的数据");
            List<string> phoneList = list.Select(o => o.Mobile).ToList();
            List<GZ_ApproveLog> approveList = new List<GZ_ApproveLog>();
            list.ForEach(o =>
            {
                approveList.Add(new GZ_ApproveLog {
                    Category =  GZ_ApproveLog.ApproveLogCategory.Through,
                    Comment = "无",
                    Id = Guid.NewGuid(),
                    OperatorId = this.UserInfo.Id,
                    OperatorTime = DateTime.Now,
                    TargetId = o.Id,
                    TargetStatus = (int)AttendanceStatusEnum.WaitUser,
                    TargetTable = nameof(GZ_Attendance)
                });
                o.Status = (int)AttendanceStatusEnum.WaitUser;
                _sms.SendSms(o.Mobile, GZ_SMS.TemplateIdEnum.考勤确认, ServiceHelper.GetParams(Convert.ToDateTime(o.Month).ToString("yyyy年MM月"), Config.BaseAddress + "/H5/AttendanceEmployeeApprove.html?id=" + o.Id.ToString()));
            });
            DbContext.GZ_ApproveLog.AddRange(approveList);
            return DbContext.SaveChanges();
        }

        internal object UpdateAttendance(AttendanceUpdateDto dto)
        {
            var model = Entities.FirstOrDefault(o => o.Id == dto.Id);
            if (model == null) throw new InputException("该考勤不存在，请刷新再试");
            if (model.Status == (int)AttendanceStatusEnum.HRPass || model.Status == (int)AttendanceStatusEnum.SystemPass || model.Status == (int)AttendanceStatusEnum.UserAgree|| model.Status == (int)AttendanceStatusEnum.WaitUser) throw new InputException("该考勤状态非否决状态或系统生成状态！不能编辑");
            if (model.DataSourceType == 1) throw new InputException("该考勤为用户上传，不能编辑");
            List<GZ_UpdateHistory> lstHistory = new List<GZ_UpdateHistory>();
            DataModel.UpdateHistoryActivator<GZ_Attendance> activator = new UpdateHistoryActivator<GZ_Attendance>(Cookies.User);
            lstHistory.Add(activator.Create(model, x => x.FinalDays, (int)dto.FinalDays));
            lstHistory = lstHistory.Where(o => o.NewValue != o.OldValue).ToList();
            model.FinalDays = dto.FinalDays;
            this.DbContext.GZ_UpdateHistory.AddRange(lstHistory);
            return Update(model);
        }

        internal object GetEntityById(Guid id)
        {
            return Entities.Where(o => o.Id == id).Select(o => new { o.Id, o.FinalDays, o.Name, DepartmentName = DbContext.GZ_Department.Where(d => d.Id == o.DepartmentId).FirstOrDefault().Name, o.Mobile, o.Month, o.TotalDays, o.RealDays, o.DataSourceType }).FirstOrDefault();
        }
        internal object GenerateById(List<Guid> ids)
        {
            if (ids == null || ids.Count == 0) throw new InputException("请选择考勤之后再操作");
            List<GZ_Attendance> attendanceList = DbContext.GZ_Attendance.Where(o => ids.Contains(o.Id)).Where(o=>o.DataSourceType==0).ToList();
            if(attendanceList.Count==0) throw new InputException("重新生成只能生成系统自动生成的，导入的不能重新生成");
            List<Guid> employeeIds = attendanceList.Select(o => o.EmployeeId).ToList();
            List<int> userids = attendanceList.Select(o => o.KProjectId).ToList();
            DateTime startTime = Convert.ToDateTime(attendanceList[0].Month + "-01");
            DateTime endTime = startTime.AddDays(1 - startTime.Day).AddMonths(1).AddMilliseconds(-1);
            //if (string.IsNullOrEmpty(month)) throw new InputException("请选择月份");
            List<SignData> signdataList = DbContext.SignData.Where(o => o.UserID.HasValue).Where(o => userids.Contains(o.UserID.Value)).Where(o => o.TimeDay.HasValue && o.TimeDay.Value > startTime && o.TimeDay.Value < endTime).ToList();
            List<Schedul> schedulList = DbContext.Schedul.Where(o => o.TimeDay.HasValue && o.TimeDay.Value > startTime && o.TimeDay.Value < endTime).ToList();
            ////员工先排除分公司  UrCpId=1表示中力知识科技
            List<GZ_Employee> employeeList = DbContext.GZ_Employee.Where(o => employeeIds.Contains(o.Id)).ToList();
            List<User> userList = DbContext.User.ToList();
            List<Leave> leaveList = DbContext.Leave.Where(o=>o.UserID.HasValue).Where(o => (o.StartTime < startTime && o.EndTime > startTime) || (o.StartTime > startTime && o.StartTime < o.EndTime)).Where(o => o.Status == 2).Where(o=>userids.Contains(o.UserID.Value)).ToList();
            List<OutsideApply> outsideApplyList = DbContext.OutsideApply.Where(o => o.UserID.HasValue).Where(o => userids.Contains(o.UserID.Value)).Where(o => (o.StartTime < startTime && o.EndTime > startTime) || (o.StartTime > startTime)).Where(o => o.Status == 2).ToList();
            List<AbnormalAppeal> abnormalAppealList = DbContext.AbnormalAppeal.Where(o => userids.Contains(o.UserID.Value)).Where(o => o.UserID.HasValue).Where(o => (o.StartTime < startTime && o.EndTime > startTime) || (o.StartTime > startTime)).Where(o => o.Status == 2).ToList();
            int daysInMonth = DateTime.DaysInMonth(startTime.Year, startTime.Month);//自然月天数
            employeeList.ForEach(o => GenerateByPersonal(o, signdataList.Where(sd => userList.Where(u => u.UserGuid == o.OriginalId)?.FirstOrDefault()?.ID == sd.UserID)?.ToList(), leaveList.Where(sd => userList.Where(u => u.UserGuid == o.OriginalId)?.FirstOrDefault()?.ID == sd.UserID)?.ToList(), outsideApplyList.Where(sd => userList.Where(u => u.UserGuid == o.OriginalId)?.FirstOrDefault()?.ID == sd.UserID)?.ToList(), abnormalAppealList.Where(sd => userList.Where(u => u.UserGuid == o.OriginalId)?.FirstOrDefault()?.ID == sd.UserID)?.ToList(), schedulList, userList.Where(u => u.UserGuid == o.OriginalId)?.FirstOrDefault(), startTime));

            return DbContext.SaveChanges();
        }
        internal object Generate(string month)
        {
            lock (_lock)
            {
                DateTime startTime = Convert.ToDateTime(month + "-01");
                DateTime lastTime = DateTime.Now;
                if (startTime > lastTime) throw new InputException("不能跨月份生成考勤");
                DateTime endTime = startTime.AddDays(1 - startTime.Day).AddMonths(1).AddMilliseconds(-1);
                if (string.IsNullOrEmpty(month)) throw new InputException("请选择月份");
                if (DbContext.GZ_Attendance.Where(o => o.Month == month && o.DataSourceType == 0).Count() > 0) throw new InputException("已经存在该考勤，不能在生成!");

                List<SignData> signdataList = DbContext.SignData.Where(o => o.TimeDay.HasValue && o.TimeDay.Value > startTime && o.TimeDay.Value < endTime).ToList();
                List<Schedul> schedulList = DbContext.Schedul.Where(o => o.TimeDay.HasValue && o.TimeDay.Value > startTime && o.TimeDay.Value < endTime).ToList();
                //List<GZ_Employee> employeeList = DbContext.GZ_Employee.Where(o => !o.QuitDate.HasValue || o.QuitDate.Value > startTime).ToList();
                //员工先排除分公司  UrCpId=1表示中力知识科技
                List<GZ_Employee> employeeList = DbContext.GZ_Employee.Where(o => (o.StatusJob != 1 && o.QuitDate.HasValue && o.QuitDate.Value > startTime) || (o.StatusJob == 1 && o.JoinDate <= endTime)).Where(o => o.CorpId == DbContext.GZ_Company.Where(c => c.UrCpId == 1).Select(c => c.Id).FirstOrDefault()).ToList();
                List<User> userList = DbContext.User.ToList();
                List<Leave> leaveList = DbContext.Leave.Where(o => (o.StartTime < startTime && o.EndTime > startTime) || (o.StartTime > startTime && o.StartTime < o.EndTime)).Where(o => o.Status == 2).ToList();
                List<OutsideApply> outsideApplyList = DbContext.OutsideApply.Where(o => (o.StartTime < startTime && o.EndTime > startTime) || (o.StartTime > startTime)).Where(o => o.Status == 2).ToList();
                List<AbnormalAppeal> abnormalAppealList = DbContext.AbnormalAppeal.Where(o => (o.StartTime < startTime && o.EndTime > startTime) || (o.StartTime > startTime)).Where(o => o.Status == 2).ToList();
                int daysInMonth = DateTime.DaysInMonth(startTime.Year, startTime.Month);//自然月天数
                employeeList.ForEach(o => GenerateByPersonal(o, signdataList.Where(sd => userList.Where(u => u.UserGuid == o.OriginalId)?.FirstOrDefault()?.ID == sd.UserID)?.ToList(), leaveList.Where(sd => userList.Where(u => u.UserGuid == o.OriginalId)?.FirstOrDefault()?.ID == sd.UserID)?.ToList(), outsideApplyList.Where(sd => userList.Where(u => u.UserGuid == o.OriginalId)?.FirstOrDefault()?.ID == sd.UserID)?.ToList(), abnormalAppealList.Where(sd => userList.Where(u => u.UserGuid == o.OriginalId)?.FirstOrDefault()?.ID == sd.UserID)?.ToList(), schedulList, userList.Where(u => u.UserGuid == o.OriginalId)?.FirstOrDefault(), startTime));

                return DbContext.SaveChanges();
            }
        }

        public class WorkItem
        {
            /// <summary>
            /// 当前时间分钟数
            /// </summary>
            public int Minute { get; set; }
            /// <summary>
            /// 0未默认  1表示有效出勤
            /// </summary>
            public int Value { get; set; }
        }

        public class KqDetail
        {
            /// <summary>
            /// 病假/分钟
            /// </summary>
            public decimal SickLeave { get; set; }

            /// <summary>
            /// 事假/分钟
            /// </summary>
            public decimal CompassionateLeave { get; set; }

            /// <summary>
            /// 调休/分钟
            /// </summary>
            public decimal BreakDown { get; set; }

            /// <summary>
            /// 年假/分钟
            /// </summary>
            public decimal AnnualLeave { get; set; }

            /// <summary>
            /// 其它带薪假时/分钟
            /// </summary>
            public decimal OtherLeave { get; set; }

            /// <summary>
            /// 外勤/分钟
            /// </summary>
            public decimal OutsideApply { get; set; }

            /// <summary>
            /// 申诉 /分钟
            /// </summary>
            public decimal AbnormalAppeal { get; set; }

            /// <summary>
            /// 迟到分钟
            /// </summary>
            public decimal BelateMinute { get; set; }

            /// <summary>
            /// 早退分钟
            /// </summary>
            public decimal LeaveEarlyMinute { get; set; }


        }

        public class SigndataDetail
        {
            /// <summary>
            /// 签到时间
            /// </summary>
            public DateTime? SigninTime { get; set; }
            /// <summary>
            /// 签退时间
            /// </summary>
            public DateTime? SignoutTime { get; set; }
            /// <summary>
            /// 排班
            /// </summary>
            public Schedul Schedul { get; set; }
            /// <summary>
            /// 请假
            /// </summary>
            public List<Leave> LeaveList { get; set; }
            /// <summary>
            /// 外勤
            /// </summary>
            public List<OutsideApply> OutsideApplyList { get; set; }
            /// <summary>
            /// 申诉
            /// </summary>
            public List<AbnormalAppeal> AbnormalAppealList { get; set; }

            /// <summary>
            /// 早退分钟数
            /// </summary>
            public decimal LeaveEarly { get; set; } = 0;

            /// <summary>
            /// 迟到分钟数
            /// </summary>
            public decimal Belate { get; set; } = 0;

            /// <summary>
            /// 异常的分钟数
            /// </summary>
            public decimal ErrCount { get; set; } = 0;

        }


        private void GenerateByPersonal(GZ_Employee user, List<SignData> signdataList, List<Leave> leaveList, List<OutsideApply> outsideApplyList, List<AbnormalAppeal> abnormalAppealList, List<Schedul> schedulList, User kqUser, DateTime startTime)
        {
            if (user == null) throw new InputException("员工不存在");
            if (kqUser == null) return; //throw new SalaryException("不存在考勤用户");
                                        // 过滤
                                        //if (kqUser.ID != 130) return;

            int daysInMonth = DateTime.DaysInMonth(startTime.Year, startTime.Month);//自然月天数
            string strMonth = startTime.ToString("yyyy-MM");
            bool isUpdate = false;//是否更新，如果是更新不用更新确认出勤天数
            GZ_Attendance model = new GZ_Attendance { EmployeeId = user.Id, Id = Guid.NewGuid(), CreateDate = DateTime.Now, CreateUser = UserInfo.Id, DepartmentId = user.DepartmentId, CompanyId = user.CorpId, KProjectId = kqUser.ID, Mobile = user.Mobile, Month = strMonth, Name = user.Name, Remark = string.Empty, Status = (int)AttendanceStatusEnum.SystemGenerate, TotalDays = daysInMonth, LastUpdateDate = DateTime.Now, LastUpdateUser = UserInfo.Id, DataSourceType = 0 };
            if (DbContext.GZ_Attendance.Where(o => o.EmployeeId == user.Id && o.Month == strMonth).Count() > 0)//表示修改
            {
                model = DbContext.GZ_Attendance.Where(o => o.EmployeeId == user.Id && o.Month == strMonth).FirstOrDefault();
                model.BelateByHour = 0;
                isUpdate = true;
            }

            KqDetail kqDetail = new KqDetail { AnnualLeave = 0, BreakDown = 0, CompassionateLeave = 0, OtherLeave = 0, SickLeave = 0 };


            decimal AbsenceDays = 0;//缺勤天数
            decimal AttendanceDays = 0;//应出勤天数
            decimal invaildHour = 0; //当天应扣合计取整
            if (user.StatusJob == 0 || user.StatusJob == 1)
            {
                for (int i = 1; i <= daysInMonth; i++)//循环自然月
                {
                    var days = startTime.AddDays(i - 1);
                    var schedul = schedulList.Where(o => o.TimeDay.Value.Day == i).OrderByDescending(o=>o.UserID).FirstOrDefault();
                    if (schedul == null)//没有排班
                    {
                        if (user.JoinDate > days || (user.StatusJob == 0 && user.QuitDate.HasValue && user.QuitDate.Value < days))
                        {
                            AbsenceDays++;
                        }
                        else
                        {
                            AttendanceDays++;
                        }
                        continue;
                    }

                    var signdata = signdataList.Where(o => o.SchedulID == schedul.ID.ToString()).FirstOrDefault();
                    if (signdata == null)
                    {
                        AbsenceDays++;
                        continue;
                    }
                    SigndataDetail signdataDetail = new SigndataDetail
                    {
                        SigninTime = signdata.SignInTiem,
                        SignoutTime = signdata.SignOutTime,
                        LeaveList = leaveList.Where(o => (o.StartTime < schedul.TimeDay && o.EndTime > schedul.TimeDay) || (o.StartTime > schedul.TimeDay && o.StartTime < schedul.TimeDay.Value.AddDays(1))).ToList(),
                        AbnormalAppealList = abnormalAppealList.Where(o => o.TimeDay == schedul.TimeDay).ToList(),
                        OutsideApplyList = outsideApplyList.Where(o => o.TimeDay == schedul.TimeDay).ToList(),
                        Schedul = schedul
                    };
                    if (signdata.SignInTiem.HasValue && !signdata.SignOutTime.HasValue && signdataDetail.LeaveList.Count == 0 && signdataDetail.AbnormalAppealList.Count == 0 && signdataDetail.OutsideApplyList.Count == 0)
                    {
                        AbsenceDays++;
                        continue;
                    }
                    if (!signdataDetail.SigninTime.HasValue && !signdataDetail.SignoutTime.HasValue && signdataDetail.LeaveList.Count == 0 && signdataDetail.AbnormalAppealList.Count == 0 && signdataDetail.OutsideApplyList.Count == 0)
                    {
                        AbsenceDays++;
                        continue;
                    }
                    decimal _sick = 0;
                    decimal _Compassionate = 0;
                    decimal _breakDown = 0;
                    decimal _annualLeave = 0;
                    decimal _otherLeave = 0;
                    #region 计算分钟时间
                    if (signdataDetail.LeaveList.Count > 0)//表示有请假  请假保存
                    {
                        foreach (var item in signdataDetail.LeaveList)
                        {
                            DateTime leaveStartTime = item.StartTime.Value;
                            DateTime leaveEndTime = item.EndTime.Value;
                            switch (item.TypeName)
                            {
                                case "病假":
                                    _sick += CalcTime(leaveStartTime, leaveEndTime, schedul);
                                    break;
                                case "事假":
                                    _Compassionate += CalcTime(leaveStartTime, leaveEndTime,
                                        schedul);
                                    break;
                                case "调休":
                                    _breakDown += CalcTime(leaveStartTime, leaveEndTime, schedul);
                                    break;
                                case "年假":
                                    _annualLeave += CalcTime(leaveStartTime, leaveEndTime, schedul);
                                    break;
                                default:
                                    _otherLeave += CalcTime(leaveStartTime, leaveEndTime, schedul);
                                    break;
                            }
                        }
                        kqDetail.SickLeave += Math.Ceiling(_sick / 60);
                        kqDetail.CompassionateLeave += Math.Ceiling(_Compassionate / 60);
                        kqDetail.BreakDown += Math.Ceiling(_breakDown / 60);
                        kqDetail.AnnualLeave += Math.Ceiling(_annualLeave / 60);
                        kqDetail.OtherLeave += Math.Ceiling(_otherLeave / 60);
                        invaildHour += Math.Ceiling(_sick * 4 / 10 / 60 + _Compassionate / 60);
                    }

                    if (signdataDetail.OutsideApplyList.Count > 0)//外勤保存
                    {
                        foreach (var item in signdataDetail.OutsideApplyList)
                        {
                            kqDetail.OutsideApply += CalcTime(item.StartTime.Value, item.EndTime.Value, schedul);
                        }
                    }

                    if (signdataDetail.AbnormalAppealList.Count > 0)//申诉保存
                    {
                        foreach (var item in signdataDetail.AbnormalAppealList)
                        {
                            kqDetail.AbnormalAppeal += CalcTime(item.StartTime.Value, item.EndTime.Value, schedul);
                        }
                    }
                    #endregion

                    FillingCalc(signdataDetail);


                    kqDetail.LeaveEarlyMinute += signdataDetail.LeaveEarly;
                    kqDetail.BelateMinute += signdataDetail.Belate;
                    if (signdataDetail.ErrCount > 30)//迟到和早退还有异常一起计算扣事假
                    {
                        model.BelateByHour += Math.Ceiling(signdataDetail.ErrCount / 60);
                    }

                    GZ_AttendanceDetail aDetail = new GZ_AttendanceDetail() { TimeDay = days, Id = Guid.NewGuid(), AnnualLeave = _annualLeave, AttendanceId = model.Id, Belate = signdataDetail.Belate, BreakDown = _breakDown, CompassionateLeave = _Compassionate, CreateDate = DateTime.Now, CreateUser = UserInfo.Id, LeaveEarly = signdataDetail.LeaveEarly, OtherLeave = _otherLeave, SickLeave = _sick };
                    DbContext.GZ_AttendanceDetail.RemoveRange(DbContext.GZ_AttendanceDetail.Where(o => o.AttendanceId == model.Id));
                    DbContext.GZ_AttendanceDetail.Add(aDetail);
                    AttendanceDays++;
                }
            }
            else//辞退
            {

            }

            var day = (model.BelateByHour + invaildHour) / 8;//应该扣的换成天数

            model.AbsenteeismDays = AbsenceDays;
            model.AnnualLeave = kqDetail.AnnualLeave;
            model.BelateMinute = kqDetail.BelateMinute;
            model.BreakDown = kqDetail.BreakDown;
            model.CompassionateLeave = kqDetail.CompassionateLeave;
            model.LeaveEarlyMinute = kqDetail.LeaveEarlyMinute;
            model.LeaveMinute = kqDetail.AnnualLeave + kqDetail.BreakDown + kqDetail.CompassionateLeave + kqDetail.OtherLeave + kqDetail.SickLeave;
            model.SickLeave = kqDetail.SickLeave;
            model.OtherLeave = kqDetail.OtherLeave;
            model.OutsideMinute = kqDetail.OutsideApply;
            model.RealDays = AttendanceDays - day;
            if (user.IsLeader == 1)//全勤
            {
                model.AbsenteeismDays = 0;
                model.RealDays = daysInMonth;
            }
            if (model.RealDays != daysInMonth)//表示不是全勤
            {
                model.RealDays += user.PaidHoliday;
                if (model.RealDays > daysInMonth) model.RealDays = daysInMonth;
            }

            model.TotalDays = daysInMonth;
            if (!isUpdate)//修改情况不改
            {
                model.FinalDays = model.RealDays;
            }
            var entry = DbContext.Entry<GZ_Attendance>(model);
            if (isUpdate)
            {
                entry.State = EntityState.Modified;
            }
            else
            {
                entry.State = EntityState.Added;
            }
        }

        /// <summary>
        /// 填坑计算 单条签到
        /// </summary>
        /// <param name="signdataDetail"></param>
        private void FillingCalc(SigndataDetail signdataDetail)
        {
            List<WorkItem> list = new List<WorkItem>();
            DateTime aStartTime = signdataDetail.Schedul.MstartTime.Value;
            DateTime aEndTime = signdataDetail.Schedul.MendTime.Value;
            DateTime pStartTime = signdataDetail.Schedul.AstartTime.Value;
            DateTime pEndTime = signdataDetail.Schedul.AendTime.Value;
            while (aStartTime <= aEndTime)
            {
                list.Add(new WorkItem { Minute = aStartTime.Hour * 60 + aStartTime.Minute, Value = 0 });
                aStartTime = aStartTime.AddMinutes(1);
            }
            while (pStartTime <= pEndTime)
            {
                list.Add(new WorkItem { Minute = pStartTime.Hour * 60 + pStartTime.Minute, Value = 0 });
                pStartTime = pStartTime.AddMinutes(1);
            }
            aStartTime = signdataDetail.Schedul.MstartTime.Value;
            pStartTime = signdataDetail.Schedul.AstartTime.Value;

            if (signdataDetail.SigninTime.HasValue && signdataDetail.SignoutTime.HasValue)
            {
                var signinStartTime = signdataDetail.SigninTime.Value < aStartTime ? aStartTime : signdataDetail.SigninTime.Value;
                var signoutEndTime = signdataDetail.SignoutTime.Value > pEndTime ? pEndTime : signdataDetail.SignoutTime.Value;
                while (signinStartTime <= signoutEndTime)
                {
                    var carryMinute = signinStartTime.Second > 0 ? 1 : 0;
                    var model = list.Where(o => o.Minute == signinStartTime.Hour * 60 + signinStartTime.Minute + carryMinute).FirstOrDefault();
                    if (model != null)
                    {
                        model.Value = 1;
                    }
                    signinStartTime = signinStartTime.AddMinutes(1);
                }
            }

            foreach (var item in signdataDetail.LeaveList)
            {
                DateTime leaveStartTime = item.StartTime.Value < aStartTime ? aStartTime : item.StartTime.Value;
                DateTime leaveEndTime = item.EndTime.Value > pEndTime ? pEndTime : item.EndTime.Value;
                while (leaveStartTime <= leaveEndTime)
                {
                    var model = list.Where(o => o.Minute == leaveStartTime.Hour * 60 + leaveStartTime.Minute).FirstOrDefault();
                    if (model != null)
                    {
                        model.Value = 1;
                    }
                    leaveStartTime = leaveStartTime.AddMinutes(1);
                }
            }

            foreach (var item in signdataDetail.OutsideApplyList)
            {
                DateTime outsideApplyStartTime = item.StartTime.Value < aStartTime ? aStartTime : item.StartTime.Value;
                DateTime outsideApplyEndTime = item.EndTime.Value > pEndTime ? pEndTime : item.EndTime.Value;
                while (outsideApplyStartTime <= outsideApplyEndTime)
                {
                    var model = list.Where(o => o.Minute == outsideApplyStartTime.Hour * 60 + outsideApplyStartTime.Minute).FirstOrDefault();
                    if (model != null)
                    {
                        model.Value = 1;
                    }
                    outsideApplyStartTime = outsideApplyStartTime.AddMinutes(1);
                }
            }

            foreach (var item in signdataDetail.AbnormalAppealList)
            {
                DateTime abnormalAppealStartTime = item.StartTime.Value < aStartTime ? aStartTime : item.StartTime.Value;
                DateTime abnormalAppealEndTime = item.EndTime.Value > pEndTime ? pEndTime : item.EndTime.Value;
                while (abnormalAppealStartTime <= abnormalAppealEndTime)
                {
                    var model = list.Where(o => o.Minute == abnormalAppealStartTime.Hour * 60 + abnormalAppealStartTime.Minute).FirstOrDefault();
                    if (model != null)
                    {
                        model.Value = 1;
                    }
                    abnormalAppealStartTime = abnormalAppealStartTime.AddMinutes(1);
                }
            }

            int exceed = 0;
            foreach (var item in list)
            {
                if (item.Value == 0)
                {
                    signdataDetail.Belate++;
                    if (item.Minute == aEndTime.Hour * 60 + aEndTime.Minute)
                    {
                        signdataDetail.Belate--;
                        exceed++;
                    }
                    if (item.Minute == pEndTime.Hour * 60 + pEndTime.Minute)
                    {
                        signdataDetail.Belate--;
                    }
                    continue;
                }
                break;
            }

            var workItem = list.LastOrDefault(o => o.Value == 1);
            if (workItem == null || workItem.Minute < signdataDetail.Schedul.AstartTime.Value.Hour * 60 + signdataDetail.Schedul.AstartTime.Value.Minute)
            {
                exceed++;
            }

            signdataDetail.ErrCount = list.Where(o => o.Value == 0).Count() - exceed;
            signdataDetail.LeaveEarly = signdataDetail.ErrCount - signdataDetail.Belate;
        }

        private decimal CalcTime(DateTime leaveStartTime, DateTime leaveEndTime, Schedul schedul)
        {
            if (leaveStartTime < schedul.MstartTime) leaveStartTime = schedul.MstartTime.Value;
            if (leaveEndTime > schedul.AendTime) leaveEndTime = schedul.AendTime.Value;

            TimeSpan ts = leaveEndTime - leaveStartTime;
            TimeSpan schedulTs = schedul.AendTime.Value - schedul.MstartTime.Value;//排班的包括中午休息
            TimeSpan schedulGap = schedul.AstartTime.Value - schedul.MendTime.Value;
            if ((leaveStartTime >= schedul.MstartTime && leaveEndTime <= schedul.AstartTime) || (leaveStartTime > schedul.MendTime))
            {
                //todo 考虑中午超过
                if (leaveStartTime >= schedul.MstartTime && leaveEndTime <= schedul.AstartTime && leaveEndTime > schedul.MendTime)
                    leaveEndTime = schedul.MendTime.Value;
                if (leaveStartTime > schedul.MendTime && leaveStartTime < schedul.AstartTime)
                    leaveStartTime = schedul.AstartTime.Value;
                //上午请假或下午请假
                return Convert.ToDecimal(ts.TotalMinutes);
            }
            return Convert.ToDecimal(ts.TotalMinutes - schedulGap.TotalMinutes);
        }

    }
}