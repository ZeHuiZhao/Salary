using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Salary.Common
{
    public enum ChannelTypeEnum
    {
        /// <summary>
        /// 我的客户
        /// </summary>
        [Description("我的客户")]
        Mine=1,
        /// <summary>
        /// 客户公海
        /// </summary>
        [Description("客户公海")]
        Public=2,
        /// <summary>
        /// 客户回收站
        /// </summary>
        [Description("客户回收站")]
        Recycle=3
    }
}