using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Salary.Common
{
    public enum UserTypeEnum
    {
        /// <summary>
        /// 中力渠道管理员
        /// </summary>
        [Description("中力渠道管理员")]
        ZLChannelManager=1,
        /// <summary>
        /// 渠道管理员
        /// </summary>
        [Description("渠道管理员")]
        ChannelManager =2,
        /// <summary>
        /// 渠道销售员
        /// </summary>
        [Description("渠道销售员")]
        Sales =3,
        /// <summary>
        /// 超级管理员
        /// </summary>
        [Description("超级管理员")]
        Admin
    }
}