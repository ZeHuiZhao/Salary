using Salary_MVC.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary_MVC.DataModel
{
    public class GZ_AttendanceDetail:CreateDateEntity
    {
        
        public Guid AttendanceId { get; set; }

        /// <summary>
        /// 考勤天
        /// </summary>
        [Required]
        public DateTime TimeDay { get; set; }

        /// <summary>
        /// 病假/分钟
        /// </summary>
        [DecimalPrecision(15, 6)]
        [Required]
        public decimal SickLeave { get; set; }

        /// <summary>
        /// 事假/分钟
        /// </summary>
        [DecimalPrecision(15, 6)]
        [Required]
        public decimal CompassionateLeave { get; set; }

        /// <summary>
        /// 调休/分钟
        /// </summary>
        [DecimalPrecision(15, 6)]
        [Required]
        public decimal BreakDown { get; set; }
        

        /// <summary>
        /// 年假/分钟
        /// </summary>
        [DecimalPrecision(15, 6)]
        [Required]
        public decimal AnnualLeave { get; set; }
        

        /// <summary>
        /// 其它带薪假时/分钟
        /// </summary>
        [DecimalPrecision(15, 6)]
        [Required]
        public decimal OtherLeave { get; set; }

        /// <summary>
        /// 迟到/分钟
        /// </summary>
        [DecimalPrecision(15, 6)]
        [Required]
        public decimal Belate { get; set; }

        /// <summary>
        /// 早退/分钟
        /// </summary>
        [DecimalPrecision(15, 6)]
        [Required]
        public decimal LeaveEarly { get; set; }

    }
}