using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models.User
{
    public class UserDto
    {
        /// <summary>
        /// 登录名(手机号码)
        /// </summary>
        [StringLength(50)]
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password{ get; set; }

        /// <summary>
        /// 是否自动登陆
        /// </summary>
        [Required]
        public int IsAutoLogin { get; set; }

    }
}