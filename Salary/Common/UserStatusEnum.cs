using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Salary.Common
{
    /// <summary>
    /// 用户状态
    /// </summary>
    public enum UserStatusEnum
    {
        [Description("没有开通")]
        NotFound = 0,
        [Description("正常")]
        Normal = 1,
        [Description("停用")]
        Disable = 2,
        [Description("已删除")]
        Deleted = 3,
        [Description("未审核")]
        Unreviewed = 4
    }
}