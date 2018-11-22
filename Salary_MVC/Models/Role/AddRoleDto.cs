using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models.Role
{
    public class AddRoleDto
    {
        /// <summary>
        /// 序号
        /// </summary>
        [Required]
        public int Code { get; set; }


        /// <summary>
        /// 角色名称
        /// </summary>
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
    }
}