using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models.User
{
    public class UpdatePwdDto : UpdateDto
    {
        /// <summary>
        /// 用户密码（旧）
        /// </summary>
        [Required]
        public string OldPwd { get; set; }
        /// <summary>
        /// 用户的密码（新）
        /// </summary>
        [Required]
        public string NewPwd { get; set; }
    }
}