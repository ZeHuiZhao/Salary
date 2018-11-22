using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models.Employee
{
    public class ApplyApproveDto:UpdateDto
    {
        /// <summary>
        /// 用户意见
        /// </summary>
        public string Opinion { get; set; }
    }
}