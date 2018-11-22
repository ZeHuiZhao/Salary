using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Salary.Models.Base;

namespace Salary.Models.User
{
    public class UpdateChannelUserDto: UpdateDto
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

        /// <summary>
        /// 用户状态  0.取消开通 1 .正常 2.停用 3.删除 4.未审核
        /// </summary>
        [Required]
        public int UserStatus { get; set; }
    }
}