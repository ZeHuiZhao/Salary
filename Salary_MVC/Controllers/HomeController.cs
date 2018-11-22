using Salary_MVC.Common;
using Salary_MVC.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Salary_MVC.Controllers
{
    public class HomeController : Controller
    {
        Services.UserService us = new Services.UserService();
        Services.SMSService _sms = new Services.SMSService();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUserFunctionRight()
        {
            var list_menu = us.GetFunctionRightDto();
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, list_menu));
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Cookies.AutoUserID = null;
            Cookies.UserCode = null;

            return Json(new OperationResult(OperationResultType.Success, "", 1));
        }

        public ActionResult GetUserInfo()
        {
            var list = us.GetUserInfo(Cookies.UserCode.Value);
            if (list != null)
            {
                return Json(new OperationResult(OperationResultType.Success, "", list));
            }
            return Json(new OperationResult(OperationResultType.Error, "", 0));
        }

        public ActionResult UpdatePwd(UpdatePwdDto dto)
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
                return Json(new OperationResult(OperationResultType.Error, msg));
            }

            dto.Id = Cookies.UserCode.Value;

            int result = us.UpdatePassword(dto);


            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "密码修改成功"));
            }

            else if (result == -1)
            {
                return Json(new OperationResult(OperationResultType.Error, "用户不存在"));
            }
            else if (result == -2)
            {
                return Json(new OperationResult(OperationResultType.Error, "旧密码不匹配"));
            }

            return Json(new OperationResult(OperationResultType.Error, "密码修改失败"));

        }


        public ActionResult ForgetPwd(UserDto dto)
        {
            var list = us.GetUserByPhone(dto.UserName);
            if (list == null)
            {
                return Json(new OperationResult(OperationResultType.Error, "请输入正确的手机号码"));
            }

            int result = us.ForgetPassword(list.Id);

            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "密码已发送到您手机"));
            }

            return Json(new OperationResult(OperationResultType.Error, "密码发送失败"));
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult SendSmsByUnsend()
        {
            var result =  _sms.SendSmsByUnsend();
            return Json(new OperationResult(OperationResultType.Success, "", "", result),JsonRequestBehavior.AllowGet);
        }
    }
}