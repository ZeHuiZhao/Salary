using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models.Employee
{
    public class EmployeeSalaryDto:UpdateDto
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string CheckCode { get; set; }
    }
}