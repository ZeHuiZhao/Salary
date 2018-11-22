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
    /// 财务核算单位表
    /// </summary>
    [Table("GZ_FinancialUnit")]
    public class GZ_FinancialUnit:UpdateDateEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(500)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 状态（0正常，1作废）
        /// </summary>
        [Required]
        public FinancialUnit Status { get; set; }

        public enum FinancialUnit
        {
            正常=0,
            作废=1
        }
    }
}