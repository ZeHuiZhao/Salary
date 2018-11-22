using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.Material
{
    public class CreateMaterialDto
    {
        /// <summary>
        /// 素材标题
        /// </summary>
        [Required]
        public string MaterialTitle { get; set; }
        /// <summary>
        /// 素材内容
        /// </summary>
        [Required]
        public string MaterialContent { get; set; }
        /// <summary>
        /// 素材摘要
        /// </summary>
        [Required]
        public string MaterialSummary { get; set; }
        /// <summary>
        /// 素材封面
        /// </summary>
        [Required]
        public string CoverImg { get; set; }
        /// <summary>
        /// 素材小提示
        /// </summary>
        public string MaterialTips { get; set; }
        /// <summary>
        /// 素材类别
        /// </summary>
        [Required]
        public int MTId { get; set; }

        /// <summary>
        /// 素材来源类型1.中力素材 2. 渠道素材 3活动推广素材
        /// </summary>
        [Required]
        public int MaterialType { get; set; }

        /// <summary>
        /// 活动推广id
        /// </summary>
        public int? ActivityId { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        [Required]
        public int IsActive { get; set; }
        /// <summary>
        /// 渠道id
        /// </summary>
        [Required]
        public int ChannelId { get; set; }
        /// <summary>
        /// 分享来源 0.渠道添加不分享 1全部，2.中力  3.渠道
        /// </summary>
        [Required]
        public int ShareSource { get; set; }

        /// <summary>
        /// 当分享来源为3时，记录的渠道id 用,号隔开
        /// </summary>
        public string ChannelIds { get; set; }

        /// <summary>
        /// 虚拟点击量
        /// </summary>
        [Required]
        public int VirtualTimes { get; set; }
    }
}