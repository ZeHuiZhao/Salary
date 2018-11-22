using Salary_MVC.Common;
using Salary_MVC.Models;
using Salary_MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Salary_MVC.Controllers
{
    /// <summary>
    /// 考勤
    /// </summary>
    public class AttendanceController : ZlController
    {
        private readonly AttendanceService _attendance = new AttendanceService();
        // GET: Attendance
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult GetAttendanceList(AttendanceQueryDto dto)
        {
            var obj = _attendance.GetAttendanceList(dto);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj),JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetEntityById(Guid id)
        {
            var obj = _attendance.GetEntityById(id);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Generate(string Month)
        {
            var obj = _attendance.Generate(Month);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        [HttpPost]
        public ActionResult GenerateById(List<Guid> Ids)
        {
            var obj = _attendance.GenerateById(Ids);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        [HttpPost]
        public ActionResult Update(AttendanceUpdateDto dto)
        {
            var obj = _attendance.UpdateAttendance(dto);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 人事发起审核
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApporveByHR(AttendanceApporveDto dto)
        {
            var obj = _attendance.ApporveByHR(dto);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 员工确认提交
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ApproveByEmployee(ApproveDto dto)
        {
            var obj = _attendance.ApproveByEmployee(dto);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 删除导入的考勤（按月不区分公司）
        /// </summary>
        /// <param name="Month"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteImportedData(List<Guid> Ids)
        {
            var obj = _attendance.DeleteImportedData(Ids);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 导出考勤
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Export(ExportDto dto)
        {
            var obj = _attendance.Export(dto);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 导入考勤
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Import(ImportInput dto)
        {
            if (dto == null)
                throw new ArgumentException("请求参数格式错误");
            if (string.IsNullOrEmpty(dto.FilePath))
                throw new ArgumentException("必须指定导入的excel文件所在的路径");
            string fullFilePath = this.Server.MapPath(dto.FilePath);
            if (!System.IO.File.Exists(fullFilePath))
                throw new ArgumentException("指定的excel文件不存在");
            DateTime datetime;
            if (!DateTime.TryParse(dto.Month, out datetime))
                throw new ArgumentException("指定的月份格式错误");
            dto.Month = datetime.ToString("yyyy-MM");
            var rt = _attendance.Import(dto, fullFilePath);
            
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, "","",rt));
        }

        /// <summary>
        /// 补发通知
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendApproveMessageToEmployee(List<Guid> Ids)
        {
            var obj = _attendance.SendApproveMessageToEmployee(Ids);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 获取链接
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetEmployeeApproveAddress(Guid id)
        {
            var obj = _attendance.GetEmployeeApproveAddress(id);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 考勤审核页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AttendanceHrManagerApprove()
        {
            return View();
        }

        /// <summary>
        /// 人事经理审核
        /// </summary>
        /// <returns></returns>
        public ActionResult ApproveByHRManager(ApproveMultipleDto dto)
        {
            var obj = _attendance.ApproveByHRManager(dto);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 人事经理全部审核
        /// </summary>
        /// <returns></returns>
        public ActionResult ApproveAllByHRManager(ApproveAllDto dto)
        {
            var obj = _attendance.ApproveAllByHRManager(dto);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 强制审核
        /// </summary>
        /// <returns></returns>
        public ActionResult ApproveAllByForce()
        {
            var obj = _attendance.ApproveAllByForce();
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 浏览考勤
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AttendanceView()
        {
            return View();
        }

        /// <summary>
        /// 下载模板
        /// </summary>
        /// <returns></returns>
        public ActionResult Template()
        {
            var filepath = System.IO.Path.Combine(TemplateFileDirectory, "考勤模板.xlsx");
            var fullpath = this.Server.MapPath(filepath);
            return this.File(fullpath, "application/vnd.ms-excel", System.IO.Path.GetFileName(fullpath));
        }
        
        [HttpGet]
        public ActionResult DownloadLogFile(string fileName)
        {
            var fullpath = this.Server.MapPath(fileName);
            return this.File(fullpath, "application/txt", System.IO.Path.GetFileName(fullpath));
        }
    }
}