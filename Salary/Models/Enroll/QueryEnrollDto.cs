using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Salary.Common;

namespace Salary.Models.Enroll
{
    public class QueryEnrollDto
    {
        /// <summary>
        /// 报名状态
        /// </summary>
        [Required]
        public EnrollStatusEnum EnrollStatus { get; set; }

        /// <summary>
        /// 销售员
        /// </summary>
        public int? Salesman { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 素材类别
        /// </summary>
        public int? MaterialType { get; set; }

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
        /// 活动id
        /// </summary>
        public int? ActivityId { get; set; }
    }
}