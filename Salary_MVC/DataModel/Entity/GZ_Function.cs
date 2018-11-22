using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary_MVC.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Salary_MVC.DataModel
{
    [Description("功能表")]
    /// <summary>
    /// 功能表
    /// </summary>
    [Table("GZ_Function")]
    public class GZ_Function: UpdateDateEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(50)]
        [Required]
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// URI
        /// </summary>
        [StringLength(500)]
        [Description("URI")]
        public string URL { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        //[Required]
        [Description("父级Id")]
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 图标(一个或者多个css的class的名称)
        /// </summary>
        [StringLength(500)]
        [Description("Ico")]
        public string Ico { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        [Required]
        [Description("排序")]
        public int Order { get; set; }
    }
}