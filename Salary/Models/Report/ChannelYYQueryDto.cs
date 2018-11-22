using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Salary.Common;

namespace Salary.Models.Report
{
    public class ChannelYYQueryNoPagingDto
    {
        /// <summary>
        /// 渠道id
        /// </summary>
        public int? ChannelId { get; set; }

        /// <summary>
        /// 销售员id
        /// </summary>
        public int? SalesId { get; set; }

        /// <summary>
        /// 素材类别Id
        /// </summary>
        public int? MTId { get; set; }

        /// <summary>
        /// 素材标题
        /// </summary>
        public string MaterialTitle { get; set; }
    }

    public class ChannelYYQueryDto
    {
        /// <summary>
        /// 渠道id
        /// </summary>
        public int? ChannelId { get; set; }

        /// <summary>
        /// 销售员id
        /// </summary>
        public int? SalesId { get; set; }

        /// <summary>
        /// 素材类别Id
        /// </summary>
        public int? MTId { get; set; }

        /// <summary>
        /// 素材标题
        /// </summary>
        public string MaterialTitle { get; set; }

        /// <summary>
        /// 每页显示的条数
        /// </summary>
        [Required]
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页数
        /// </summary>
        [Required]
        public int PageIndex { get; set; }

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