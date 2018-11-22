using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.Home
{
    public class RoleModel
    {
        /// <summary>
        /// 电话号码
        /// </summary>
        [Required]
        public string PhoneNum { get; set; }
        /// <summary>
        /// 切换到的角色  2.渠道管理员  3. 渠道销售员
        /// </summary>
        [Required]
        public int UserType { get; set; }
        
    }
}