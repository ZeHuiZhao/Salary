using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Salary.DataModel.Entity
{
    [Table("YY_EnrollAudit")]
    public class YY_EnrollAudit:DataEntity
    {
        /// <summary>
        /// 审核状态 1 渠道未审核 2渠道审核不通过 3.中力未审核 4.中力审核不通过 5未参课 6. 已参课
        /// </summary>
        [Required]
        public int AuditStatus { get; set; }

        [Required]
        [ForeignKey("Enroll")]
        public int EnrollId { get; set; }
        /// <summary>
        /// 是否审批通过 1.通过 0不通过
        /// </summary>
        [Required]
        public int IsSuccess { get; set; }

        [Required]
        [StringLength(100)]
        public string AuditName { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        [Required]
        public DateTime AuditTime { get; set; }
        /// <summary>
        /// 审核意见
        /// </summary>
        [StringLength(100)]
        public string AuditOpinion { get; set; }

        
        public virtual YY_Enroll Enroll { get; set; }
    }
}