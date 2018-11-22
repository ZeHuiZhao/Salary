
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
    /// 用户角色关系表
    /// </summary>
    [Table("GZ_UserRole")]
    public class GZ_UserRole: CreateDateEntity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [Required]
        public Guid RoleId { get; set; }
    }
}