using Salary_MVC.Common;
using Salary_MVC.Models;
using Salary_MVC.Models.Employee;
using Salary_MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Salary_MVC.Controllers
{
    public class EmployeeController : ZlController
    {
        private readonly EmployeeService _employee = new EmployeeService();
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 查询员工列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEntity(EmployeeQueryDto dto)
        {
            var obj=  _employee.GetEmployeeList(dto);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 单个员工查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetEntityById(Guid id)
        {
            var obj = _employee.GetEntityById(id);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 更新带薪假期和是否免打卡
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdatePaidHoliday(UpdateHolidayDto dto)
        {
            var obj = _employee.UpdatePaidHoliday(dto);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }



        /// <summary>
        /// 更新身份证，银行卡，银行卡身份
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateIDCard(UpdateIdCardDto dto)
        {
            var obj = _employee.UpdateIDCard(dto);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 更新财务核算单位，发薪公司
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateFinancialUnit(UpdateFinancialDto dto)
        {
            var obj = _employee.UpdateFinancialUnit(dto);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 发送校验码，查看工资
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendSalaryCheckCode()
        {
            var obj = _employee.SendSalaryCheckCode();
            return Json(new OperationResult(OperationResultType.Success, "", "", obj?1:0));
        }

        /// <summary>
        /// 获取员工工资变化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSalaryHistory(EmployeeSalaryDto dto)
        {
            var obj = _employee.GetSalaryHistory(dto);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 辞退
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Discharge(Guid id)
        {
            var obj = _employee.Discharge(id);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 辞退当月的考勤和基本工资
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAttendanceByDischarge(Guid id)
        {
            var obj = _employee.GetAttendanceByDischarge(id);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 导出辞退的工资表excel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExportAttendanceByDischarge(Guid id)
        {
            var obj = _employee.ExportAttendanceByDischarge(id);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 撤销辞退
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UnDischarge(Guid id)
        {
            var obj = _employee.UnDischarge(id);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        #region 员工信息状态变化

        /// <summary>
        /// 发起解锁申请
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApproveUnlockByHR(ApplyApproveDto dto)
        {
            var obj = _employee.ApproveUnlockByHR(dto);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 发起锁定申请
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApproveLockByHR(ApplyApproveDto dto)
        {
            var obj = _employee.ApproveLockByHR(dto);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }
        /// <summary>
        /// 财务审核解锁申请
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ApproveUnlockByFinance(ApproveDto dto)
        {
            var obj = _employee.ApproveUnlockByFinance(dto);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 财务审核解锁申请
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApproveUnlockMultiByFinance(ApproveMultipleDto dto)
        {
            var obj = _employee.ApproveUnlockMultiByFinance(dto);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 财务审核锁定申请
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ApproveLockByFinance(ApproveDto dto)
        {
            var obj = _employee.ApproveLockByFinance(dto);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 财务审核锁定申请
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApproveLockMultiByFinance(ApproveMultipleDto dto)
        {
            var obj = _employee.ApproveLockMultiByFinance(dto);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        #endregion

        public ActionResult GetEntityWithKeyValueByDepartmentId(Guid id)
        {
            List<DataModel.GZ_Employee> lst= this._employee.GetEntityWithKeyValueByDepartmentId(id);
            var lstRT= lst.Select(x => new { x.Id, x.Name });
            return this.Json(new Common.OperationResult( OperationResultType.Success,string.Empty,lstRT));
        }

        public ActionResult EmployeeApprove()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetEmployeeStatus(Guid id)
        {
            var obj = _employee.GetEmployeeStatus(id);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj), JsonRequestBehavior.AllowGet);
        }
    }
}