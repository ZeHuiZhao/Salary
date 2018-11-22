using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.Report
{
    public class ChannelReportDto
    {
        /// <summary>
        /// 顶部圆数据
        /// </summary>
        public object CircleData { get; set; }

        /// <summary>
        /// 漏斗数据
        /// </summary>
        public object FunnelData { get; set; }

        /// <summary>
        /// 销售员数据
        /// </summary>
        //public object SalesData { get; set; }
    }
}