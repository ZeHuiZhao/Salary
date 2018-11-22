using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salary_MVC.DataModel
{
    /// <summary>
    /// 角色表
    /// </summary>
    [Table("GZ_Role")]
    public class GZ_Role : UpdateDateEntity
    {
        /// <summary>
        /// 序号
        /// </summary>
        [Required]
        public int Code { get; set; }


        /// <summary>
        /// 角色名称
        /// </summary>
        [StringLength(50)]
        [Required]
        public string  Name { get; set; }

    }
}