using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Salary_MVC.Controllers
{
    [AllowAnonymous]
    public class CSSController : Controller
    {

        public ActionResult SOGSearchDropDownList()
        {
            return View();
        }


        public ActionResult Table()
        {
            return View();

        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult CEOApprove()
        {
            return View();
        }

        public ActionResult CEODetail()

        {
            return View();
        }
    }
}