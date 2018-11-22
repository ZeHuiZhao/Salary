using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Salary.Models.Base;

namespace Salary.Models.User
{
    public class UpdateUserDto:UpdateDto
    {
        /// <summary>
        /// 用户名字
        /// </summary>
        [Required]
        public string TrueName { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        [Required]
        public string PhoneNum { get; set; }
    }
}