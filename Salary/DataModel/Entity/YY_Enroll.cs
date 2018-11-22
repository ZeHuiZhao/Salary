using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Salary.DataModel.Entity
{
    /// <summary>
    /// 报名表
    /// </summary>
    [Table("YY_Enroll")]
    public class YY_Enroll:DataEntity
    {
        /// <summary>
        /// 客户（公司）名称
        /// </summary>
        [StringLength(100)]
        [Required]
        public string CompanyName { get; set; }
        /// <summary>
        /// 联系人名字
        /// </summary>
        [StringLength(100)]
        [Required]
        public string ContactName { get; set; }

        /// <summary>
        /// 联系手机
        /// </summary>
        [StringLength(50)]
        [Required]
        public string ContactPhone { get; set; }
        /// <summary>
        /// 联系人职位
        /// </summary>
        [StringLength(100)]
        [Required]
        public string ContactJob { get; set; }
        /// <summary>
        /// 课程时间
        /// </summary>
        [StringLength(100)]
        [Required]
        public string ClassTime { get; set; }
        /// <summary>
        /// 销售员id
        /// </summary>
        [Required]
        [ForeignKey("User")]
        public int SalesId { get; set; }
        public virtual YY_Userinfo User { get; set; }
        /// <summary>
        /// erp客户表
        /// </summary>
        public int? CCId { get; set; }
        /// <summary>
        /// erp客户表
        /// </summary>
        public int? TCId { get; set; } 
        /// <summary>
        /// 渠道id
        /// </summary>
        [Required]
        public int ChannelId { get; set; }
        /// <summary>
        /// 素材id
        /// </summary>
        [Required]
        [ForeignKey("Material")]
        public int MId { get; set; }
        public virtual YY_Material Material { get; set; }
        /// <summary>
        /// 报名状态
        /// </summary>
        [Required]
        public int EnrollStatus { get; set; }

        /// <summary>
        /// 0,不是推广活动 大于0为活动id
        /// </summary>
        [Required]
        public int IsActivity { get; set; }

        public virtual List<YY_EnrollAudit> EnrollAudit { get; set; }
    }
}