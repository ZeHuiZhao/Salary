using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Salary.Common;

namespace Salary.Filter
{
    public class LoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Count > 0)   // 允许匿名访问
            {
                base.OnActionExecuting(actionContext);
                return;
            }
            if (string.IsNullOrEmpty(Cookies.UserCode))
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                return;
            }
            else
            {
                base.OnActionExecuting(actionContext);
                return;
            }
        }
    }
}