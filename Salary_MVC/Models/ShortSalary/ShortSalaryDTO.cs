using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models
{
    public class ShortSalaryAdd
    {
        /// <summary>
        /// 员工id
        /// </summary>
       
        public Guid EmployeeId { get; set; }


       
        public decimal Money { get; set; }


        /// <summary>
        /// 类别（奖励，赔偿）
        /// </summary>
       
        public DataModel.GZ_ShortSalary.ShortSalaryCategoryEnum Category { get; set; }

        /// <summary>
        /// 生效日期
        /// </summary>
      
        public DateTime EffectedDate { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [Required(AllowEmptyStrings=true)]
        public string Comment { get; set; }

        public string FilePath { get; set; }


    }

    public class ShortSalaryEdit:UpdateDto
    {
        public decimal Money { get; set; }


        public DataModel.GZ_ShortSalary.ShortSalaryCategoryEnum Category { get; set; }

      
        public DateTime EffectedDate { get; set; }

     
        public string Comment { get; set; }

        public string FilePath { get; set; }

    }
}