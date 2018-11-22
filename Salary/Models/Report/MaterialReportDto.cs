using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.Report
{
    public class MaterialReportDto
    {
        /// <summary>
        /// 素材标题
        /// </summary>
        public string MaterialTitle { get; set; }
        /// <summary>
        /// 树状图数据
        /// </summary>
        public object BarData { get; set; }

        ///// <summary>
        ///// 销售员数据
        ///// </summary>
        //public object SalesData { get; set; }
    }
}