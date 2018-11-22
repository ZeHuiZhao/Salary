using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.User
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public class QueryUserDto
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNum { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public int? UserStatus { get; set; }

        /// <summary>
        /// 用户角色  1.中力渠道管理员  2.渠道管理员  3.渠道销售员
        /// </summary>
        [Required]
        public int UserType { get; set; }

        /// <summary>
        /// 每页显示的条数
        /// </summary>
        [Required]
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页数
        /// </summary>
        [Required]
        public int PageIndex { get; set; }
    }
}