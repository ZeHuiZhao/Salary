using Salary_MVC.Common;
using Salary_MVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models.Employee
{
    public class UpdateHolidayDto:UpdateDto
    {
        /// <summary>
        /// 是否领导（0正常打卡，1免打卡）
        /// </summary>
        [Required]
        public int IsLeader { get; set; }

        /// <summary>
        /// 带薪假期
        /// </summary>
        [DecimalPrecision(5, 2)]
        [Required]
        public decimal PaidHoliday { get; set; }
    }
}