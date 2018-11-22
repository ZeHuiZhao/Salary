using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.Enroll
{
    public class QueryWechatEnrollResult
    {
        /// <summary>
        /// 素材类别id
        /// </summary>
        public int? MTId { get; set; }

        /// <summary>
        /// 全局搜索
        /// </summary>
        public string GlobalSearch { get; set; }

        /// <summary>
        /// 微信openid
        /// </summary>
        public string OpenId { get; set; }

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