using Salary_MVC.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models.FunctionGroupRight
{
    public class UpdateFunctionGroupRightDto
    {
        /// <summary>
        /// 权限组Id
        /// </summary>
        [Required]
        public Guid FunctionGroupId { get; set; }

        /// <summary>
        /// 功能Id
        /// </summary>
        [Required]
        public string FunctionId { get; set; }
    }
}