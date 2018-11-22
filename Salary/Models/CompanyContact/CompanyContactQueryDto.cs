using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.CompanyContact
{
    public class CompanyContactQueryDto
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// 销售员id
        /// </summary>
        public int? SalesId { get; set; }

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