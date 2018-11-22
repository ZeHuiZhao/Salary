using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.User
{
    public class ToggleChannelStatus
    {
        /// <summary>
        /// id
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// 用户状态  0.取消开通 1 .正常 2.停用 3.删除 4.未审核
        /// </summary>
        [Required]
        public int UserStatus { get; set; }
    }
}