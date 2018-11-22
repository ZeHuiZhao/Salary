using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Salary.DataModel.Entity
{
    /// <summary>
    /// 素材表
    /// </summary>
    [Table("YY_Material")]
    public class YY_Material:DataEntity
    {
        /// <summary>
        /// 素材标题
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string MaterialTitle { get; set; }
        /// <summary>
        /// 素材内容
        /// </summary>
        [Required]
        [Column(TypeName ="text")]
        public string MaterialContent { get; set; }
        /// <summary>
        /// 素材摘要
        /// </summary>
        [StringLength(1000)]
        [Required]
        public string MaterialSummary { get; set; }
        /// <summary>
        /// 素材封面
        /// </summary>
        [StringLength(500)]
        [Required]
        public string CoverImg { get; set; }
        /// <summary>
        /// 素材小提示
        /// </summary>
        [StringLength(1000)]
        public string MaterialTips { get; set; }
        /// <summary>
        /// 素材类别
        /// </summary>
        [Required]
        public int MTId { get; set; }
        [ForeignKey("MTId")]
        public virtual YY_MaterialType MaterialTypes { get; set; }

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
        /// 分享来源
        /// </summary>
        [Required]
        public int ShareSource { get; set; }

        /// <summary>
        /// 显示的时间
        /// </summary>
        [Required]
        public DateTime DisplayTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 虚拟点击量
        /// </summary>
        [Required]
        public int VirtualTimes { get; set; } = 0;

        public virtual List<YY_Enroll> Enroll { get; set; }

        public virtual List<YY_MaterialBrower> MaterialBrower { get; set; }

        public virtual List<YY_MaterialChannel> MaterialChannel { get; set; }

        public virtual List<YY_MaterialShare> YY_MaterialShare { get; set; }

    }
}