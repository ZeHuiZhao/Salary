using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Salary_MVC.Enum
{
    /// <summary>
    /// 状态（21未锁定，32锁定中，11锁定，31解锁中）
    /// </summary>
    public enum EmployeeStatusEnum
    {
        /// <summary>
        /// 锁定
        /// </summary>
        [Description("锁定")]
        Lock=11,
        /// <summary>
        /// 未锁定
        /// </summary>
        [Description("未锁定")]
        Unlocked =21,

        /// <summary>
        /// 解锁中
        /// </summary>
        [Description("解锁中")]
        Locking = 31,
        /// <summary>
        /// 锁定中
        /// </summary>
        [Description("锁定中")]
        Locked = 32
    }
}