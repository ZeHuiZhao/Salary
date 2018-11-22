using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Salary_MVC.Enum
{
    public enum SalaryGroupEnum
    {
        /// <summary>
        /// 中力知识基金
        /// </summary>
        [Description("中力知识科技")]
        ZLZSJJ=1,
        /// <summary>
        /// 京鹏基金
        /// </summary>
        [Description("京鹏基金")]
        JPJJ=2
    }
}