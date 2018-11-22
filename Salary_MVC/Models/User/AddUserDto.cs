using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models.User
{
    public class AddUserDto
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
        /// 状态（0正常，1停用）
        /// </summary>
        [Required]
        public UserStatusEnum Status { get; set; }

        public enum UserStatusEnum
        {
            /// <summary>
            /// 正常
            /// </summary>
            Normal = 0,
            /// <summary>
            /// 禁用
            /// </summary>
            Forbid = 1
        }


        /// <summary>
        /// 功能组ID
        /// </summary>
        [Required]
        public Guid FunctionGroupId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [Required]
        public string Role { get; set; }

    }
}