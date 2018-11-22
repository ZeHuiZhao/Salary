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
    /// 权限组功能关系表
    /// </summary>
    [Table("GZ_FunctionGroupRight")]
    public class GZ_FunctionGroupRight : CreateDateEntity
    {
        /// <summary>
        /// 权限组Id
        /// </summary>
        [Required]
        public Guid FunctionGroupId { get; set; }

        /// <summary>
        /// 功能Id
        /// </summary>
        [Required]
        public Guid FunctionId { get; set; }

    }
}