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
    /// 修改记录表
    /// </summary>
    [Table("GZ_UpdateHistory")]
    public class GZ_UpdateHistory: CreateDateEntity
    {
        /// <summary>
        /// 被修改数据的Id
        /// </summary>
        [Required]
        public Guid TargetId { get; set; }

        /// <summary>
        /// 被修改数据所在的表
        /// </summary>
        [StringLength(50)]
        [Required]
        public string TargetTable { get; set; }

        /// <summary>
        /// 修改列的名称
        /// </summary>
        [StringLength(50)]
        [Required]
        public string ColumnName { get; set; }

        /// <summary>
        /// 原值
        /// </summary>
        [StringLength(1024)]
        public string OldValue { get; set; }

        /// <summary>
        /// 新值
        /// </summary>
        [StringLength(1024)]
        public string NewValue { get; set; }
    }
}