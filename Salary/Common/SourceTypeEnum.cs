using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Salary.Common
{
    public enum SourceTypeEnum
    {
        [Description("中力友友录入")]
        Wechat=1,
        [Description("中力友友手工录入")]
        Manual=2,
        [Description("活动推广")]
        Activity=3
    }
}