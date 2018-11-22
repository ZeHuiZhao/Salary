using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.Home
{
    public class UpdatePwd
    {
        /// <summary>
        /// 用户的id
        /// </summary>
        [Required]
        public int Id { get; set; }

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