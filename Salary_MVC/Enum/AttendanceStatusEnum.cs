using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Salary_MVC.Enum
{
    public enum AttendanceStatusEnum
    {
        //30系统生成，31待用户确认，12用户同意，23用户否决，10人力资本总监同意，25人力资本总监否决，11系统强制同意
        /// <summary>
        /// 人力资本总监同意
        /// </summary>
        [Description("人力资本总监同意")]
        HRPass = 10,
        /// <summary>
        /// 系统强制同意
        /// </summary>
        [Description("系统强制同意")]
        SystemPass = 11,
        /// <summary>
        /// 用户同意
        /// </summary>
        [Description("用户同意")]
        UserAgree = 12,
        /// <summary>
        /// 用户否决
        /// </summary>
        [Description("用户否决")]
        UserDisAgree = 23,
        /// <summary>
        /// 人力资本总监否决
        /// </summary>
        [Description("人力资本总监否决")]
        HRDisAgree = 25,
        /// <summary>
        /// 系统生成
        /// </summary>
        [Description("系统生成")]
        SystemGenerate = 30,
        /// <summary>
        /// 待用户确认
        /// </summary>
        [Description("待用户确认")]
        WaitUser = 31
    }
}