using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Common
{
    /// <summary>
    /// 日志帮助类，封装日志的常用操作方法
    /// </summary>
    public class LogHelper
    {
        private static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");
        private static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");

        public static void SetConfig()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// 普通的文件记录日志
        /// </summary>
        /// <param name="info"></param>
        public static void WriteInfoLog(string info)
        {
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info);
            }
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="error"></param>
        public static void WriteErrorLog(string error)
        {
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Error(error);
            }
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="error"></param>
        /// <param name="ex"></param>
        public static void WriteErrorLog(string error, Exception ex)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(error, ex);
            }
        }
    }
}