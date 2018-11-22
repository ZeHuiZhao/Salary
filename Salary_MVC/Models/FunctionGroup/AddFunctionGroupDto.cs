using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models.FunctionGroup
{
    public class AddFunctionGroupDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
    }
}