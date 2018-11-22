using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Salary.DataModel.Entity
{
    /// <summary>
    /// 导航表
    /// </summary>
    [Table("YY_Function")]
    public class YY_Function:DataEntity
    {
        /// <summary>
        /// 导航名称
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// 导航父亲id
        /// </summary>
        [Required]
        public int ParentId { get; set; }
        /// <summary>
        /// 导航排序
        /// </summary>
        [Required]
        public int DisOrder { get; set; }
        /// <summary>
        /// 导航是否可用
        /// </summary>
        [Required]
        public int Enable { get; set; }
        /// <summary>
        /// 导航图标
        /// </summary>
        [StringLength(100)]
        public string Icon { get; set; }
        /// <summary>
        /// 导航链接
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Url { get; set; }
    }
}