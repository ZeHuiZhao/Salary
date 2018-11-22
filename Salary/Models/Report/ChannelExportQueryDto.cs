using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary.Common;

namespace Salary.Models.Report
{
    public class ChannelExportQueryDto
    {
        /// <summary>
        /// 渠道的id
        /// </summary>
        public int? ChannelId { get; set; }

        /// <summary>
        /// 销售员id
        /// </summary>
        public int? SalesId { get; set; }
    }

    public class ChannelSalesQueryDto : ChannelExportQueryDto
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