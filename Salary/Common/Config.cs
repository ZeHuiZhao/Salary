using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Common
{
    public class Config
    {
        //public static string websiteUrl = "http://youyou.zhongliko.com";
        //public static string websiteUrl = "http://test.youyou.zhongliko.com";
        public static string websiteUrl = "http://192.168.23.251";

        public static readonly int sdkappid = 1400140914;

        public static readonly string appkey = "355f08f7f48528ab8c7bd21ebbcaf8d5";

        //erp地址  用来请求erp接口
        //public static string ErpAddress = "http://localhost:61001";
        //public static string ErpAddress = "http://erp.zhongliko.com";
        public static string ErpAddress = "http://os-ceshi.zhongliko.com";

        public static string FileUrl
        {
            get
            {
                string str_url = "/uploadfile/user_" + Cookies.UserCode + "/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("yyyyMMddHHmmss") + "/";
                string str_path = HttpContext.Current.Server.MapPath(str_url);
                if (!System.IO.Directory.Exists(str_path))
                {
                    System.IO.Directory.CreateDirectory(str_path);
                }
                return str_url;
            }
        }
    }
}