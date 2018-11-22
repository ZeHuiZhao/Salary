using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Data;


namespace Salary.Common
{
    /// <summary>
    /// 短信类
    /// </summary>
    public class SMS
    {

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="str_phone">手机号码</param>
        /// <param name="str_message">短信内容</param>
        /// <returns></returns>
        public static bool Send(string str_phone, string str_message)
        {
            string str_url = "http://sms.4006555441.com/webservice.asmx/gxmt?Sn=qhzl&Pwd=bvzxde44&mobile=" + str_phone + "&content=" + str_message;
            string str_result = HttpGetData(str_url);
            WriteLog(str_url);
            string str_success_result = @"<?xml version=""1.0"" encoding=""utf-8""?>" + "\r\n" + @"<int xmlns=""http://tempuri.org/"">0</int>";//
            if (str_result == str_success_result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 发送Get请求
        /// </summary>
        /// <param name="str_server_link">地址</param>
        /// <returns></returns>
        private static string HttpGetData(string str_server_link)
        {
            WebRequest rq = WebRequest.Create(str_server_link);
            rq.Method = "Get";
            string str_result = new StreamReader(rq.GetResponse().GetResponseStream()).ReadToEnd();
            return str_result;
        }

        /// <summary>
        /// 写Log
        /// </summary>
        /// <param name="str_content">内容</param>
        private static void WriteLog(string str_content)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath(@"~\log\sms");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                System.IO.File.AppendAllText(path + @"\" + DateTime.Now.ToString("yyyyMMdd") + ".txt", DateTime.Now.ToString("yyyyMMdd_HHmmss") + "    " + str_content + Environment.NewLine);
            }
            catch (Exception)
            {
            }
        }
    }
}