using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Salary.Common;

namespace Salary.Models.Company
{
    public class CompanyQueryDto
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 销售员id
        /// </summary>
        public int? SalesId { get; set; }
        /// <summary>
        /// 客户类型 1.我的客户 2.客户公海 3.客户回收站
        /// </summary>
        public ChannelTypeEnum ChannelType { get; set; }
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
    }
}