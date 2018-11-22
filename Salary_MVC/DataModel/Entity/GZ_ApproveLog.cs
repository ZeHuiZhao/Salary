using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary_MVC.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salary_MVC.DataModel
{
    /// <summary>
    /// 审核记录
    /// </summary>
    [Table("GZ_ApproveLog")]
    public class GZ_ApproveLog: BaseEntity
    {
        /// <summary>
        /// 被审核数据的Id
        /// </summary>
        [Required]
        public Guid TargetId { get; set; }

        /// <summary>
        /// 审核人Id
        /// </summary>
        [Required]
        public Guid OperatorId { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        [Required]
        public DateTime OperatorTime { get; set; }

        /// <summary>
        /// 操作类别（0通过，1未通过，2强制同意）
        /// </summary>
        [Required]
        public ApproveLogCategory Category { get; set; }

        /// <summary>
        /// 审核意见
        /// </summary>
        [StringLength(1024)]
        [Required]
        public string Comment { get; set; }

        /// <summary>
        /// 被审核数据所在的表名称
        /// </summary>
        [StringLength(50)]
        [Required]
        public string TargetTable { get; set; }

        /// <summary>
        /// 审核以后的数据状态
        /// </summary>
        [Required]
        public int TargetStatus { get; set; }

        public enum ApproveLogCategory
        {
            /// <summary>
            /// 通过
            /// </summary>
            Through = 0,
            /// <summary>
            /// 未通过
            /// </summary>
            NotThrough=1,
            /// <summary>
            /// 强制同意
            /// </summary>
            SysThrough=3
        }
    }
}