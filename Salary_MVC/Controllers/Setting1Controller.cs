using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Salary_MVC.Common;
using Salary_MVC.Models.User;

namespace Salary_MVC.Controllers
{
    public partial class SettingController : ZlController
    {
        Services.FunctionService _fs = new Services.FunctionService();


        // GET: Setting1
        public ActionResult Index()
        {
            return View();
        }

        public string CheckDto()
        {
            var msg = string.Empty;
            foreach (var val in ModelState.Values)
            {
                if (val.Errors.Count > 0)
                {
                    foreach (var error in val.Errors)
                    {
                        msg = msg + error.ErrorMessage;
                    }
                }
            }
            return msg;
        }

        Services.UserService obj_user_service = new Services.UserService();
        public ActionResult AddUser1(AddUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                var msg = string.Empty;
                foreach (var val in ModelState.Values)
                {
                    if (val.Errors.Count > 0)
                    {
                        foreach (var error in val.Errors)
                        {
                            msg = msg + error.ErrorMessage;
                        }
                    }
                }

                if (msg != string.Empty)
                {
                    return Json(new OperationResult(OperationResultType.Error, msg));
                }           
            }

            var count = obj_user_service.AddUser(dto);
            if (count > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "新增成功"));
            }
            else
            {
                return Json(new OperationResult(OperationResultType.Error, "新增失败"));
            }

        }



        //public ActionResult AddFunction(Models.FunctionCreateDto dto)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        var msg = string.Empty;
        //        foreach (var val in ModelState.Values)
        //        {
        //            if (val.Errors.Count > 0)
        //            {
        //                foreach (var error in val.Errors)
        //                {
        //                    msg = msg + error.ErrorMessage;
        //                }
        //            }
        //        }
        //        return Json(new OperationResult(OperationResultType.Error, msg));
        //    }

        //    var a = _fs.AddFunction(dto);
        //    if (a == 1)
        //    {
        //        return Json(new OperationResult(OperationResultType.Success, "", "", 1));
        //    }
        //    return Json(new OperationResult(OperationResultType.Error, "添加失败"));
        //}

    }
}