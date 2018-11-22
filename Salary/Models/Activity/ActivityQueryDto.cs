using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Salary.Common;

namespace Salary.Models.Activity
{
    public class ActivityQueryDto
    {
        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActivityName { get; set; }
        /// <summary>
        /// 活动状态
        /// </summary>
        public ActivityStatusEnum ActivityStatus { get; set; }

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