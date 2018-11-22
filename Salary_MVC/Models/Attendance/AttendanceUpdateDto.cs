using Salary_MVC.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models
{
    public class AttendanceUpdateDto:UpdateDto
    {
        /// <summary>
        /// 确认出勤天数
        /// </summary>
        [Required]
        [DecimalPrecision(5, 3)]
        public decimal FinalDays { get; set; }
    }
}