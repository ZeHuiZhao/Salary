using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Salary.Common;

namespace Salary.Models.Material
{
    public class QueryMaterialDto
    {

        /// <summary>
        /// 素材类别id 
        /// </summary>
        public int? MTId { get; set; }
        /// <summary>
        /// 素材标题
        /// </summary>
        public string MaterialTitle { get; set; }
        /// <summary>
        /// 素材来源类型0.为全部  1.中力素材 2. 渠道素材 3活动推广素材
        /// </summary>
        [Required]
        public MaterialTypeEnum MaterialType { get; set; }
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