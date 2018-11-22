using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Salary.Common
{
    public enum ActivityStatusEnum
    {
        [Description("全部")]
        All=0,
        [Description("未开始")]
        NotStarted=1,
        [Description("进行中")]
        Processing=2,
        [Description("已结束")]
        Over=3
    }
}