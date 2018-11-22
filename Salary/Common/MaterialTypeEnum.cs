using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Salary.Common
{
    public enum MaterialTypeEnum
    {
        //1.中力素材 2. 渠道素材 3活动推广素材
        [Description("全部")]
        All=0,
        [Description("中力素材")]
        ZLMaterial = 1,
        [Description("渠道素材")]
        ChannelMaterial = 2,
        [Description("活动素材")]
        ActivityMaterial = 3
    }
}