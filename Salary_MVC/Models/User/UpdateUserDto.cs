using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models.User
{
    public class UpdateUserDto : UpdateDto
    {
        /// <summary>
        /// 登录名(手机号码)
        /// </summary>
        [StringLength(50)]
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [StringLength(500)]
        [Required]
        public string Password { get; set; }


        /// <summary>
        /// 功能组ID
        /// </summary>
        [Required]
        public Guid FunctionGroup { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [Required]
        public string Role { get; set; }
    }
}