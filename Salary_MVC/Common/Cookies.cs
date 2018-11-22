using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Salary_MVC
{
    /// <summary>
    /// Cookies帮助类
    /// </summary>
    public class Cookies
    {
        /// <summary>
        /// 系统名字
        /// </summary>
        protected static string SystemName = "Salary";

        /// <summary>
        ///加密类
        /// </summary>
        protected static Common.CryptographyHandler obj = new Common.CryptographyHandler();

        /// <summary>
        /// 设置cookeis
        /// </summary>
        /// <param name="str_key">键</param>
        /// <param name="str_value">值</param>
        /// <param name="date">过去时间</param>
        private static void SetCookie(string str_key, string str_value, DateTime? date = null)
        {
            HttpCookie obj_cookies = new HttpCookie(SystemName + str_key);
            if (date != null)
            {
                obj_cookies.Expires = DateTime.Now.AddDays(30);
            }
            obj_cookies.Value = System.Web.HttpContext.Current.Server.UrlEncode(obj.set_password_ASC(str_value));
            System.Web.HttpContext.Current.Response.Cookies.Add(obj_cookies);

        }

        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="str_key">键</param>
        /// <returns>值</returns>
        private static string GetCookie(string str_key)
        {
            if (System.Web.HttpContext.Current.Request.Cookies[SystemName + str_key] == null)
            {
                return string.Empty;
            }
            else
            {
                return obj.get_password_ASC(System.Web.HttpContext.Current.Server.UrlDecode(System.Web.HttpContext.Current.Request.Cookies[SystemName + str_key].Value));
            }
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public static Guid? UserCode
        {
            get
            {
                string str = GetCookie("UserCode");
                if (string.IsNullOrEmpty(str))
                {
                    return null;
                }
                else
                {
                    return new Guid(str);
                }
            }
            set
            {
                if (value == null)
                {
                    SetCookie("UserCode", string.Empty);
                }
                else
                {
                    SetCookie("UserCode", value.ToString());
                }
            }
        }

        /// <summary>
        /// 自动登录ID
        /// </summary>
        public static Guid? AutoUserID
        {
            get
            {
                string str = GetCookie("AutoUserID");
                if (string.IsNullOrEmpty(str))
                {
                    return null;
                }
                else
                {
                    return new Guid(str);
                }
            }
            set
            {
                if (value == null)
                {
                    SetCookie("AutoUserID", string.Empty, DateTime.Now.AddMonths(1));
                }
                else
                {
                    SetCookie("AutoUserID", value.ToString(), DateTime.Now.AddMonths(1));
                }
            }
        }

        /// <summary>
        /// 登录名字
        /// </summary>
        public static string UserName
        {
            get
            {
                return GetCookie("UserName");
            }
            set
            {
                if (value == null)
                {
                    SetCookie("UserName", string.Empty, DateTime.Now.AddMonths(1));
                }
                else
                {
                    SetCookie("UserName", value.ToString(), DateTime.Now.AddMonths(1));
                }

            }
        }


        public static DataModel.GZ_User User
        {
            get
            {
                Salary_MVC.Data.GZDbContext DbContext = new Salary_MVC.Data.GZDbContext();
                DataModel.GZ_User one_user = DbContext.GZ_User.Where(o => o.Id == Cookies.UserCode).FirstOrDefault();
                return one_user;
            }
        }


        public static Guid? FunctionGroupId
        {
            get
            {
                return User.FunctionGroupId;
            }
        }
    }
}