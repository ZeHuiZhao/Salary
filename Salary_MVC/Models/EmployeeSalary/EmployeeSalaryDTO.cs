using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models
{
    public class EmployeeSalaryAdd
    {
        /// <summary>
        /// 员工id
        /// </summary>

        public Guid EmployeeId { get; set; }



        public decimal Money { get; set; }


        /// <summary>
        /// 生效日期
        /// </summary>

        public DateTime EffectedDate { get; set; }

        /// <summary>
        /// 说明
        /// </summary>

        public string Comment { get; set; }

        public string FilePath { get; set; }


    }

    public class EmployeeSalaryEdit : UpdateDto
    {
        public decimal Money { get; set; }



        public DateTime EffectedDate { get; set; }


        public string Comment { get; set; }

        public string FilePath { get; set; }

    }
}