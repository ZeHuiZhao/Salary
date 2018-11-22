using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Salary.DataModel.Entity
{
    /// <summary>
    /// 素材类别表
    /// </summary>
    [Table("YY_MaterialType")]
    public class YY_MaterialType:DataEntity
    {
        /// <summary>
        /// 类别名称
        /// </summary>
        [StringLength(50)]
        [Required]
        public string TypeName { get; set; }
        /// <summary>
        /// 封面图片
        /// </summary>
        [StringLength(500)]
        [Required]
        public string CoverImg { get; set; }
        /// <summary>
        /// 是否有用
        /// </summary>
        [Required]
        public int IsActive { get; set; }
        /// <summary>
        /// erp关系
        /// </summary>
        [StringLength(10)]
        public string ErpYsCode { get; set; }

        public virtual List<YY_Material> Material { get; set; }
    }
}