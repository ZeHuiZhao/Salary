using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Salary.Common;

namespace Salary.Models.Material
{
    public class QueryWechatMaterialDto
    {
        /// <summary>
        /// 素材标题
        /// </summary>
        public string MaterialTitle { get; set; }
        /// <summary>
        /// 素材类别 1.中力素材 2. 渠道素材 3活动推广素材
        /// </summary>
        [Required]
        public MaterialTypeEnum MaterialType { get; set; }

        /// <summary>
        /// 素材类别 股权激励 商业创新
        /// </summary>
        [Required]
        public int MTId { get; set; }

        /// <summary>
        /// 微信openid
        /// </summary>
        [Required]
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