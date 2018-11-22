using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models.UserFunctionRight
{
    public class UpdateUserFunctionRightDto 
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// 功能Id
        /// </summary>
        [Required]
        public string FunctionId { get; set; }


    }
}