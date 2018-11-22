using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.Home
{
    public class RolePrivilege
    {
        /// <summary>
        /// 角色id
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// 权限列表
        /// </summary>
        [Required]
        public string Privilege { get; set; }
    }
}