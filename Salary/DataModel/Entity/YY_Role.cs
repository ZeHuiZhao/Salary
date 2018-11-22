using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Salary.DataModel.Entity
{
    /// <summary>
    /// 角色表
    /// </summary>
    [Table("YY_Role")]
    public class YY_Role:DataEntity
    {
        /// <summary>
        /// 角色名字
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// 导航ids
        /// </summary>
        [Required]
        [StringLength(500)]
        public string NavigateId { get; set; }
    }
}