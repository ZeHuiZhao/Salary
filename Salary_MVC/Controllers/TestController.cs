using Salary.Common;
using Salary_MVC.Models;
using Salary_MVC.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Salary_MVC.Data;

namespace Salary_MVC.Controllers
{
    //public class TestController : Controller
    //{
    //    protected GZDbContext DbContext { set; get; }

    //    // GET: Test
    //    public ActionResult Index()
    //    {
    //        //blank.Password_Encrypt_ASC p = new blank.Password_Encrypt_ASC();
    //        //string str = blank.Password_Encrypt_ASC.set_password_ASC("111111");
    //        //string str1 = blank.Password_Encrypt_ASC.get_password_ASC("xxx");


    //        return View();
    //    }

    //    public ActionResult Approve()
    //    {
    //        return View();
    //    }


    //    public ActionResult GetRoleName()
    //    {
    //        var list = this.DbContext.GZ_Role.ToList();
    //        var dt = list.ToDataTable();
    //        if (dt == null)
    //        {
    //            throw new InputException("没有数据");
    //        }

    //        ExportExcel exp = new ExportExcel();
    //        ExportExcel.ExcelSheet sheet = new ExportExcel.ExcelSheet("考勤数据",dt.DefaultView,ExportTypes.Custom);
    //        sheet.Add("Code","序号");
    //        sheet.Add("Name", "名称");

    //        sheet.DisableAutoSetWidth();
    //        exp.AddSheet(sheet);
    //        string directory = "~/uploadfile/export/";
    //        string url = "~/uploadfile/export/" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".xls";
    //        string result = Config.BaseAddress + url.TrimStart('~');
    //        directory = System.Web.Hosting.HostingEnvironment.MapPath(directory);

    //        if (!Directory.Exists(directory))
    //        {
    //            Directory.CreateDirectory(directory);
    //        }
    //        exp.Export(System.Web.Hosting.HostingEnvironment.MapPath(url));
    //        return result;

    //        return Json(new OperationResult(OperationResultType.Success));

    //    }



    //    public ActionResult Export()
    //    {
    //        var obj = Export1();
    //        return Json(new OperationResult(OperationResultType.Success, "", "", obj));
    //    }

    //}
}