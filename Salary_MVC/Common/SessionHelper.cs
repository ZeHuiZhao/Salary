using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Salary_MVC.Common
{
    public class SessionHelper
    {
        private static HttpSessionState _session = HttpContext.Current.Session;

        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="minute"></param>
        public static void SetSession(string key, object value, int minute)
        {
            _session[key] = value;
            _session.Timeout = minute;
        }

        /// <summary>
        /// 获取Session对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetSessionObject(string key)
        {
            object result = null;
            if (_session[key] != null)
            {
                result = _session[key];
            }
            return result;
        }


        /// <summary>
        /// 获取Session数字
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetSessionNumber(string key)
        {
            int result = 0;
            if (_session[key] != null)
            {
                int.TryParse(_session[key].ToString(), out result);
            }
            return result;
        }

        /// <summary>
        /// 获取Session字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSessionString(string key)
        {
            string result = "";
            if (_session[key] != null)
            {
                result = _session[key].ToString();
            }
            return result;
        }

        /// <summary>
        /// 删除Session
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            if (_session[key] != null)
            {
                _session[key] = null;
            }
        }

        /// <summary>
        /// 清除Session
        /// </summary>
        public static void Clear()
        {
            _session.Clear();
        }
    }
}