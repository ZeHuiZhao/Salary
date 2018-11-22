using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.Material
{
    public class QueryMaterialDetail
    {
        /// <summary>
        /// 素材id
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// 微信openid
        /// </summary>
        [Required]
        public string OpenId { get; set; }

        /// <summary>
        /// 是否是测试 0 不是，1是
        /// </summary>
        public int? IsTest { get; set; }

    }
}