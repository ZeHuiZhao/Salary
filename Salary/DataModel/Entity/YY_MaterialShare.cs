using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Salary.DataModel.Entity
{
    /// <summary>
    /// 素材分享表
    /// </summary>
    [Table("YY_MaterialShare")]
    public class YY_MaterialShare:DataEntity
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
        /// 渠道id
        /// </summary>
        [Required]
        public int ChannelId { get; set; }
    }
}