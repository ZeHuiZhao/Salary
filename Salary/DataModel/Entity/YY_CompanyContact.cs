using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Salary.DataModel.Entity
{
    /// <summary>
    /// 客户联系人表
    /// </summary>
    [Table("YY_CompanyContact")]
    public class YY_CompanyContact:DataEntity
    {
        /// <summary>
        /// 联系人姓名
        /// </summary>
        [StringLength(100)]
        [Required]
        public string ContactName { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        [StringLength(50)]
        [Required]
        public string ContactPhone { get; set; }
        /// <summary>
        /// 联系人职位
        /// </summary>
        [StringLength(100)]
        [Required]
        public string ContactJob { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        [StringLength(100)]
        public string WechatNum { get; set; }
        /// <summary>
        /// qq号
        /// </summary>
        [StringLength(100)]
        public string QQNum { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [StringLength(100)]
        public string Email { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        [StringLength(100)]
        public string IdCard { get; set; }
        /// <summary>
        /// 客户表id（公司Id）
        /// </summary>
        [Required]
        [ForeignKey("Company")]
        public int CId { get; set; }
        public virtual YY_Company Company { get; set; }
        /// <summary>
        /// 是否第一联系人
        /// </summary>
        public int? IsFirst { get; set; }

        //public virtual List<YY_ContactRecord> ContactRecord { get; set; }
    }
}