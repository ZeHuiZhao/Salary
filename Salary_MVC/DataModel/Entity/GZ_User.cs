using Salary_MVC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salary_MVC.DataModel
{
    /// <summary>
    /// 用户表
    /// </summary>
    [Table("GZ_User")]
    public class GZ_User: CreateDateEntity
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
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [StringLength(500)]
        [Required]
        public string Password_Pwd { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [NotMapped]
        public string Password
        {
            get { return pass.get_password_ASC(Password_Pwd); }
            set { Password_Pwd = pass.set_password_ASC(value.ToString()); }
        }

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
            Normal=0,
            /// <summary>
            /// 禁用
            /// </summary>
            Forbid=1
        }


        /// <summary>
        /// 功能组ID
        /// </summary>
        [Required]
        public Guid FunctionGroupId { get; set; }

    }
}