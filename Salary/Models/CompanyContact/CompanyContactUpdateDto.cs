using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Salary.Models.Base;

namespace Salary.Models.CompanyContact
{
    public class CompanyContactUpdateDto:UpdateDto
    {
        /// <summary>
        /// 联系人姓名
        /// </summary>
        [Required]
        public string ContactName { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        [Required]
        public string ContactPhone { get; set; }
        /// <summary>
        /// 联系人职位
        /// </summary>
        [Required]
        public string ContactJob { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        public string WechatNum { get; set; }
        /// <summary>
        /// qq号
        /// </summary>
        public string QQNum { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 客户表id（公司Id）
        /// </summary>
        [Required]
        public int CId { get; set; }
        /// <summary>
        /// 是否第一联系人
        /// </summary>
        public int? IsFirst { get; set; }
    }
}