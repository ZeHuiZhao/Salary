using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary.DataModel.Entity;
using Salary.Service;

namespace Salary.Common
{
    public class Cookies
    {
        public string this[string key]
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[SystemName + "_" + key] == null)
                {
                    return null;
                }
                else
                {
                    return obj.get_password_ASC(System.Web.HttpContext.Current.Server.UrlDecode(System.Web.HttpContext.Current.Request.Cookies[SystemName + "_" + key].Value));
                }
            }
            set
            {
                System.Web.HttpContext.Current.Response.Cookies[SystemName + "_" + key].Value = System.Web.HttpContext.Current.Server.UrlEncode(obj.set_password_ASC(value));
            }
        }

        public static string SystemName = "Salary";

        public static Password_Encrypt_ASC.Password_Encrypt_ASC obj = new Password_Encrypt_ASC.Password_Encrypt_ASC();

        public static Cookies cookies { get { return new Cookies(); } }

        public static List<YY_Userinfo> CacheUserList
        {
            get
            {
                //if (HttpContext.Current.Cache["cacheUser-" + DateTime.Now.ToString("yyyy-MM-dd")] != null)
                //{
                //    return (List<YY_Userinfo>)HttpContext.Current.Cache["cacheUser-" + DateTime.Now.ToString("yyyy-MM-dd")];
                //}
                var list = new UserService().GetUserList();
               // HttpContext.Current.Cache["cacheUser-" + DateTime.Now.ToString("yyyy-MM-dd")] = list;
                return list;
            }
        }

        #region 普通属性
        public static string UserName
        {
            get
            {
                if (Cookies.UserCode == null || Cookies.UserCode == "")
                {
                    return "";
                }
                else
                {
                    return Current.TrueName;
                }

            }
        }

        public static YY_Userinfo Current
        {
            get
            {
                if (Cookies.UserCode == null || Cookies.UserCode == "")
                {
                    return null;
                }
                return new UserService().GetUserinfoById(Convert.ToInt32(Cookies.UserCode)); //CacheUserList.Where(o => o.Id == Convert.ToInt32(Cookies.UserCode)).FirstOrDefault();
            }
        }

        public static string UserPhone
        {
            get
            {
                if (System.Web.HttpContext.Current.Request.Cookies[SystemName + "UserPhone"] == null)
                {
                    return string.Empty;
                }
                else
                {
                    return obj.get_password_ASC(System.Web.HttpContext.Current.Server.UrlDecode(System.Web.HttpContext.Current.Request.Cookies[SystemName + "UserPhone"].Value));
                }
            }
            set
            {
                HttpCookie obj_cookies = new HttpCookie(SystemName + "UserPhone");
                obj_cookies.Expires = DateTime.Now.AddDays(365);
                obj_cookies.Value = System.Web.HttpContext.Current.Server.UrlEncode(obj.set_password_ASC(value));
                System.Web.HttpContext.Current.Response.Cookies.Add(obj_cookies);
            }
        }

        public static int UserId
        {
            get
            {
               return Convert.ToInt32(UserCode);
            }
        }

        public static string UserCode
        {
            get
            {
                string str_value = cookies["UserCode"];
                if (string.IsNullOrEmpty(str_value))
                {
                    str_value = "18";
                }
                return str_value;
            }
            set
            {
                cookies["UserCode"] = value;
            }
        }

        public static string AutoUserID
        {
            get
            {
                if (System.Web.HttpContext.Current.Request.Cookies[SystemName + "AutoUserID"] == null)
                {
                    return string.Empty;
                }
                else
                {
                    return obj.get_password_ASC(System.Web.HttpContext.Current.Server.UrlDecode(System.Web.HttpContext.Current.Request.Cookies[SystemName + "AutoUserID"].Value));
                }
            }
            set
            {
                HttpCookie obj_cookies = new HttpCookie(SystemName + "AutoUserID");
                obj_cookies.Expires = DateTime.Now.AddDays(30);
                obj_cookies.Value = System.Web.HttpContext.Current.Server.UrlEncode(obj.set_password_ASC(value));
                System.Web.HttpContext.Current.Response.Cookies.Add(obj_cookies);
            }
        }
        

        public static string OpenID
        {
            get
            {
                return Current.OpenId;
            }
        }

        public static int UserType
        {
            get
            {
                return Current.UserType;
            }
        }
        

        public static int ChannelId
        {
            get
            {
                return Current.ChannelId;
            }
        }


        #endregion
    }
}