using Salary_MVC.Common;
using Salary_MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Salary_MVC.Controllers
{
    public class CommonController : ZlController
    {
        private readonly CommonService _common = new CommonService();
        /// <summary>
        /// 获取公司列表，用来下拉
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCompanyList()
        {
            var obj = _common.GetCompanyList();
            return Json(new OperationResult(OperationResultType.Success, "", "", obj),JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDepartmentList(Guid? companyId)
        {
            var obj = _common.GetDepartmentList(companyId);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj),JsonRequestBehavior.AllowGet);
        }
    }
}