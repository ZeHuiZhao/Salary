using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Salary_MVC.Common
{
    //class MyClass: RedirectResult
    //{
    //    ControllerContext _context;
    //    public MyClass(string url, ControllerContext context)
    //    {
    //        _context = context;
    //    }

    //    public override void ExecuteResult(ControllerContext context)
    //    {
    //        base.ExecuteResult(context);
    //    }
    //}
    public class IsAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())//ajax的请求
                filterContext.Result = new JsonResult() { Data = new OperationResult(OperationResultType.Error, "请登录再操作"), ContentType = "application/json" };
            else
                //根据需要添加
                filterContext.Result = new RedirectResult("/login/index");
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //根据需要添加，将自动根据返回值判断用户是否通过验证
            //true：通过
            //false:未通过
            bool result = false;
           // var usercode = Cookies.UserCode;
            if (Cookies.UserCode.HasValue)
            {
                result = true;
                //var userInfo = httpContext.Session["__@UserInfo@__"] as DataModel.GZ_User;
                //if (userInfo == null)
                //    userInfo = new Data.GZDbContext().GZ_User.Where(x => x.Id == usercode.Value).First();
                //httpContext.Session["__@UserInfo@__"] = userInfo;
                //System.Runtime.Remoting.Messaging.CallContext.SetData("__@UserInfo@__", userInfo);
            }
            return result;
        }
    }
}