using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;

namespace Salary
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            
            // Web API 配置和服务
            config.EnableCors(new System.Web.Http.Cors.EnableCorsAttribute("*", "*", "*"));
            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            //log4net.Config.XmlConfigurator.Configure();
            log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\log4net.config"));
        }
    }
}
