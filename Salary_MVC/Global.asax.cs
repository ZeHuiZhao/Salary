using Salary_MVC.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Salary_MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(System.IO.Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, "log4net.config")));

            //添加自定义全局登录过滤器
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

        }
    }

}
