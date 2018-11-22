using Salary_MVC.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models
{
    public class AttendanceQueryDto
    {
        /// <summary>
        /// 公司id
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        public Guid? DepartmentId { get; set; }
        
        /// <summary>
        /// 考勤月份
        /// </summary>
        [Required]
        public string Month { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 考勤状态 1用户同意（人事经理待审核） 2 已审核 3强制审核
        /// </summary>
        public int? AttendanceStatus { get; set; }
    }
}