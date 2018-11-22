using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.User
{
    public class CreateWechatUser
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        [Required]
        public string TrueName { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        [Required]
        [RegularExpression(@"^1[3458][0-9]{9}$", ErrorMessage = "手机号格式不正确")]
        public string PhoneNum { get; set; }
        /// <summary>
        /// 用户角色  1. 中力渠道管理员 2.渠道管理员 3.渠道销售员
        /// </summary>
        [Required]
        public int UserType { get; set; }
        /// <summary>
        /// 用户状态  0.取消开通 1 .正常 2.停用 3.删除 4.未审核
        /// </summary>
        [Required]
        public int UserStatus { get; set; } 
        /// <summary>
        /// 渠道id
        /// </summary>
        [Required]
        public int ChannelId { get; set; }
        /// <summary>
        /// 微信openid
        /// </summary>
        [StringLength(100)]
        [Required]
        public string OpenId { get; set; }
    }
}