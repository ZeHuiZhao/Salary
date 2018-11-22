using Salary_MVC.Enum;
using Salary_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models.Employee
{
    public class UpdateFinancialDto:UpdateDto
    {
        /// <summary>
        /// 发薪公司
        /// </summary>
        public SalaryGroupEnum SalaryGroup { get; set; }

        /// <summary>
        /// 财务核算单位Id
        /// </summary>
        public Guid FinacialUnitId { get; set; }
    }
}