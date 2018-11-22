using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary_MVC.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salary_MVC.DataModel
{
    [Table("GZ_MonthlySalaryMaster")]
    public class GZ_MonthlySalaryMaster: BaseEntity
    {
        /// <summary>
        /// 月份
        /// </summary>
        [StringLength(50)]
        [Required]
        public string Month { get; set; }

        /// <summary>
        /// 审核状态（1待发起审核，2待财务经理审核，3财务经理同意，4财务经理否决，5CFO同意，6CFO否决，7董办同意，8董办否决）
        /// </summary>
        [Required]
        public MonthlySalaryStatus Status { get; set; }

        public enum MonthlySalaryStatus
        {
            待发起审核=1,
            待财务经理审核 = 2,
            财务经理同意 = 3,
            财务经理否决 = 4,
            CFO同意 = 5,
            CFO否决 = 6,
            董办同意 = 7,
            董办否决 = 8
        }
    }
}