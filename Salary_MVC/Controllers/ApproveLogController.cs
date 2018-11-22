using Salary_MVC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Salary_MVC.Controllers
{
    public class ApproveLogController : Controller
    {
        Services.UserService userService = new Services.UserService();
        Services.ApproveLogService approveLogService = new Services.ApproveLogService();
        Services.EmployeeService _employeeService = new Services.EmployeeService();
        // GET: ApproveLog
        public ActionResult GetEntityByTargetId(Guid targetId)
        {
            if (targetId == Guid.Empty)
                throw new ArgumentException("必须指定被审核数据的Id");
            List<DataModel.GZ_ApproveLog> lst = this.approveLogService.GetEntityByTargetId(targetId);
            List<DataModel.GZ_User> lstUser= this.userService.GetAllUser();
            var dictUser= lstUser.ToDictionary(x => x.Id, x => x.Name);
            var tmp = lst.Select(x => new {
                Name = dictUser.ContainsKey(x.OperatorId)?dictUser[x.OperatorId]:"admin",
                OperatorTime = x.OperatorTime.ToString("yyyy-MM-dd HH:mm"),
                TargetStatus = ((Salary_MVC.Enum.ApproveStatus)x.TargetStatus).ToString(),
                Handler = x.Category == DataModel.GZ_ApproveLog.ApproveLogCategory.Through ? "通过" : "不通过",
                x.Comment
            }).ToList();

            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, tmp));
        }

        public ActionResult GetAttendanceLogByTargetId(Guid targetId)
        {
            if (targetId == Guid.Empty)
                throw new ArgumentException("必须指定被审核数据的Id");
            List<DataModel.GZ_ApproveLog> lst = this.approveLogService.GetEntityByTargetId(targetId);
            List<DataModel.GZ_User> lstUser = this.userService.GetAllUser();
            List<DataModel.GZ_Employee> empList = _employeeService.GetAllEmployee();
            var dictUser = lstUser.ToDictionary(x => x.Id, x => x.Name);
            var dictEmp = empList.ToDictionary(x => x.Id, x => x.Name);
            var tmp = lst.Select(x => new {
                Name = ((Salary_MVC.Enum.AttendanceStatusEnum)x.TargetStatus)== Enum.AttendanceStatusEnum.UserAgree&& dictEmp.ContainsKey(x.OperatorId) ? dictEmp[x.OperatorId] : dictUser.ContainsKey(x.OperatorId) ? dictUser[x.OperatorId] : "admin",
                OperatorTime = x.OperatorTime.ToString("yyyy-MM-dd HH:mm"),
                TargetStatus = ((Salary_MVC.Enum.AttendanceStatusEnum)x.TargetStatus).GetDescription(),
                Handler = x.Category == DataModel.GZ_ApproveLog.ApproveLogCategory.Through||x.Category==DataModel.GZ_ApproveLog.ApproveLogCategory.SysThrough ? "通过" : "不通过",
                x.Comment
            }).ToList();

            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, tmp));
        }
    }
}