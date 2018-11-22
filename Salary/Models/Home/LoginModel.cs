using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.Home
{
    public class LoginModel
    {
        /// <summary>
        /// 电话号码（用户名）
        /// </summary>
        [Required]
        public string PhoneNum { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string UserPwd { get; set; }
        /// <summary>
        /// 是否自动登陆
        /// </summary>
        [Required]
        public int IsAutoLogin { get; set; }
    }
}