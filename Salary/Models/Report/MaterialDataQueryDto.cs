using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary.Common;

namespace Salary.Models.Report
{
    public class MaterialDataQueryDto
    {
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }
    }

    public class MaterialSalesDataQueryDto: MaterialDataQueryDto
    {
        /// <summary>
        /// 分享排序 1升序 2倒序
        /// </summary>
        public OrderEnum ShareOrder { get; set; }

        /// <summary>
        /// 浏览排序  1升序 2倒序
        /// </summary>
        public OrderEnum BrowseOrder { get; set; }

        /// <summary>
        /// 报名排序 1升序 2倒序
        /// </summary>
        public OrderEnum EnrollOrder { get; set; }

        /// <summary>
        /// 参课排序 1升序 2倒序
        /// </summary>
        public OrderEnum ParticipateOrder { get; set; }
    }
}