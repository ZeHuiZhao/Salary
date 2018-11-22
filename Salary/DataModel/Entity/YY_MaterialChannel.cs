using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Salary.DataModel.Entity
{
    /// <summary>
    /// 素材渠道表
    /// </summary>
    public class YY_MaterialChannel:DataEntity
    {
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