using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Salary.DataModel.Entity
{
    /// <summary>
    /// 素材浏览表
    /// </summary>
    [Table("YY_MaterialBrower")]
    public class YY_MaterialBrower:DataEntity
    {
        /// <summary>
        /// 销售员id
        /// </summary>
        [Required]
        [ForeignKey("User")]
        public int SalesId { get; set; }
        public virtual YY_Userinfo User { get; set; }
        /// <summary>
        /// 素材id
        /// </summary>
        [Required]
        [ForeignKey("Material")]
        public int MId { get; set; }
        public virtual YY_Material Material { get; set; }
        /// <summary>
        /// 浏览器码
        /// </summary>
        [Required]
        [StringLength(100)]
        public string DeviceCode { get; set; }
        /// <summary>
        /// 渠道id
        /// </summary>
        [Required]
        public int ChannelId { get; set; }

    }
}