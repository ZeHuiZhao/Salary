using Salary_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models
{
    public class EmployeeQueryDto:BaseQueryDto
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
        /// 是否可用
        /// </summary>
        public int? Active { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 1锁定中 2解锁中 3未锁定 4已锁定
        /// </summary>
        public int? LockStatus { get; set; }
    }
}