using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Salary.DataModel.Entity
{
    /// <summary>
    /// 客户联系记录表
    /// </summary>
    [Table("YY_ContactRecord")]
    public class YY_ContactRecord : DataEntity
    {
        /// <summary>
        /// 联系时间
        /// </summary>
        [Required]
        public DateTime ContactTime { get; set; }
        /// <summary>
        /// 联系摘要
        /// </summary>
        [StringLength(1000)]
        public string ContactSummary { get; set; }
        /// <summary>
        /// 联系人id
        /// </summary>
        [Required]
        public int ContactId { get; set; }
        //[ForeignKey("ContactId")]
        //public virtual YY_CompanyContact CompanyContact { get; set; }
        /// <summary>
        /// 销售员id
        /// </summary>
        [Required]
        [ForeignKey("Userinfo")]
        public int SalesId { get; set; }
         public YY_Userinfo Userinfo { get; set; }
        /// <summary>
        /// 客户id（公司表id）
        /// </summary>
        [Required]
        [ForeignKey("Company")]
        public int CId { get; set; }
        public virtual YY_Company Company { get; set; }
    }
}