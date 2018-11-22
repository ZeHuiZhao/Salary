using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary_MVC.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salary_MVC.DataModel
{
    /// <summary>
    /// 权限组表
    /// </summary>
    [Table("GZ_FunctionGroup")]
    public class GZ_FunctionGroup: UpdateDateEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
    }
}