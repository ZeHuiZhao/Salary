using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Salary.DataModel.Entity
{
    /// <summary>
    /// 推广活动表
    /// </summary>
    [Table("YY_Activity")]
    public class YY_Activity:DataEntity
    {
        /// <summary>
        /// 活动名称
        /// </summary>
        [Required]
        [StringLength(200)]
        public string  ActivityName { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Required]
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 封面图片
        /// </summary>
        [Required]
        [StringLength(500)]
        public string CoverImg { get; set; }
        /// <summary>
        /// 主讲老师
        /// </summary>
        [StringLength(50)]
        public string LeadTeacher { get; set; } 
        /// <summary>
        /// 详细地址
        /// </summary>
        [StringLength(50)]
        public string Address { get; set; }
        /// <summary>
        /// 活动负责人
        /// </summary>
        [StringLength(50)]
        public string Principal { get; set; }
    }
}