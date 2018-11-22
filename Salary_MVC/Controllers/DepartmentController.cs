using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Salary_MVC.Controllers
{
    public class DepartmentController : ZlController
    {
        Services.DepartmentService department = new Services.DepartmentService();
        // GET: Department
        public ActionResult GetEntityWithKeyValue()
        {
            var lst= this.department.GetEntityWithKeyValue();
            var lstRT= lst.Select(x => new { x.Id, x.Name }).ToList();
            return this.Json(new Common.OperationResult( Common.OperationResultType.Success,string.Empty, lstRT));
        }
    }
}