using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Salary.DataModel.Entity
{
    /// <summary>
    /// 用户表
    /// </summary>
    [Table("YY_Userinfo")]
    public class YY_Userinfo:DataEntity
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        [StringLength(100)]
        [Required]
        public string TrueName { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        [StringLength(50)]
        [Required]
        public string PhoneNum { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        [StringLength(100)]
        [Required]
        public string UserPwd { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        [StringLength(200)]
        public string HeadImage { get; set; }
        /// <summary>
        /// 微信头像
        /// </summary>
        [StringLength(200)]
        public string WechatHeadImage { get; set; }
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
        /// 是否是联系人（0.不是erp联系人 1.是erp联系人）
        /// </summary>
        [Required]
        public int IsContacts { get; set; }
        /// <summary>
        /// 渠道id
        /// </summary>
        [Required]
        public int ChannelId { get; set; }
        /// <summary>
        /// 微信openid
        /// </summary>
        [StringLength(100)]
        public string OpenId { get; set; }

        public virtual List<YY_MaterialBrower> MaterialBrower { get; set; }
        public virtual List<YY_MaterialShare> YY_MaterialShare { get; set; }

        //public virtual List<YY_CompanyContact> CompanyContact { get; set; }

        //public virtual List<YY_ContactRecord> ContactRecord { get; set; }

        public virtual List<YY_Enroll> Enroll { get; set; }
    }
}