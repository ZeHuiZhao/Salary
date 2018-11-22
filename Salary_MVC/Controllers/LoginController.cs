using Salary_MVC.Common;
using Salary_MVC.Models.User;
using Salary_MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Salary_MVC.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        Services.UserService userService = new UserService();
        private readonly SMSService smsService = new SMSService();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formdata"></param>
        /// <returns></returns>
        public ActionResult CEO(Salary_MVC.Models.CeoLoginInput formdata)
        {
            this.ViewData["formdata"] = formdata;
            return View();
        }

        /// <summary>
        /// 登陆验证
        /// </summary>
        /// <param name="formdata"></param>
        /// <returns></returns>
        public ActionResult CEOValidate(Salary_MVC.Models.CeoLoginInput formdata)
        {
            formdata.Code = formdata.Code ?? string.Empty;
            //验证校验码
            if (this.Session["ValidateCode"] == null)
                return CEOValidateFault("验证码已过期,请重新发送验证码", formdata);
            if (!formdata.Code.Equals(this.Session["ValidateCode"]))
                return CEOValidateFault("验证码不正确", formdata);
            //验证密码
            var user= this.userService.Login(new UserDto() { UserName=formdata.Moblie, Password=formdata.Pwd });
            if (user == null)
                return CEOValidateFault("密码错误",formdata);
            Cookies.UserCode = user.Id;
            Cookies.UserName = user.UserName;
            return this.Redirect(string.Format("~/MonthlySalary/Approve?targetid={0}", formdata.TargetId));
        }

        private ActionResult CEOValidateFault(string msg, Salary_MVC.Models.CeoLoginInput formdata)
        {
            formdata.Msg = msg;
            this.ViewData["formdata"] = formdata;
            return this.View("~/Views/Login/ceo.cshtml");
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult ValidateCode(string mobile)
        {
            if (string.IsNullOrEmpty(mobile))
                throw new ArgumentException("必须指定接收验证码的手机号码");
            System.Random rd = new Random();
            var rdv= rd.Next(1, 999999);
            var rdvStr= rdv.ToString().PadLeft("123456".Length, '0');
            var result= this.smsService.SendSmsByNow(mobile.Substring(0, 11), DataModel.GZ_SMS.TemplateIdEnum.验证码, new List<string>() { rdvStr });
            //var pwd = this.RandomPwd(6); //"123456";
            //SessionHelper.SetSession("check_code", pwd, 10);
            //var result = _sms.SendSmsByNow(Cookies.UserName, GZ_SMS.TemplateIdEnum.验证码, ServiceHelper.GetParams(pwd));
            //smsService.SendSmsByNow()
            //this.Session["ValidateCode"] = "123456";
            Common.OperationResultType rt = result.errmsg.ToUpper() == "OK" ? OperationResultType.Success : OperationResultType.Error;
            if (rt == OperationResultType.Success)
                this.Session["ValidateCode"] = rdvStr;
            var msg = rt == OperationResultType.Error ? "发送验证码失败，请重新发送" : string.Empty;
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success));
        }


        /// <summary>
        /// 自动登录
        /// </summary>
        /// <returns></returns>
        public ActionResult AutoLogin()
        {
            if (Cookies.AutoUserID != null)
            {
                return Json(new OperationResult(OperationResultType.Success, "", 1));
            }
            return Json(new OperationResult(OperationResultType.Success, "", 0));
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ActionResult Login(UserDto dto)
        {
            
            var model = userService.Login(dto);

            if (model != null)
            {
                Cookies.UserCode = model.Id;
                Cookies.UserName = model.UserName;

                if (dto.IsAutoLogin == 1)
                {
                    Cookies.AutoUserID = model.Id;
                }
                return Json(new OperationResult(OperationResultType.Success, "", 1));
            }
            return Json(new OperationResult(OperationResultType.Error, "用户名或密码不正确", 0));
        }


        public ActionResult GetLoginInfo()
        {
            if (Cookies.UserName == null)
            {
                return Json(new OperationResult(OperationResultType.Error, "没有登录信息"));
            }
            return Json(new OperationResult(OperationResultType.Success, "", "", Cookies.UserName));
        }

        public ActionResult AdminLogin()
        {
            Cookies.UserCode = new Guid("657E12B6-3115-4EC7-A78B-55A2E3DE9DA1");
            Cookies.UserName = "赵泽辉";
            return Redirect("/Home/Index/");
        }


        protected override void OnException(ExceptionContext filterContext)
        {
            if (!this.Request.IsAjaxRequest())//ajax的请求
                return;//暂时只处理ajax的异常，视图的异常直接报黄页，
            filterContext.ExceptionHandled = true;
            Common.OperationResult rt = new Common.OperationResult(Common.OperationResultType.Error);
            rt.Message = filterContext.Exception.Message;
            var modelException = filterContext.Exception as Salary_MVC.Common.ModelBindingException;
            if (modelException != null)
            {
                var lstError = modelException.ValidateResult.Where(x => x.Errors.Count > 0).Select(x => string.Join(",", x.Errors.Select(a => a.ErrorMessage)));
                rt.Message = string.Join("；", lstError);
            }
            filterContext.Result = this.Json(rt, JsonRequestBehavior.AllowGet);
        }


    }
}