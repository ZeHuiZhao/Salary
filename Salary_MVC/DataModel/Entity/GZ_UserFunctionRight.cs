using System;
using System.Collections.Generic;
using Salary_MVC.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Salary_MVC.DataModel
{
    /// <summary>
    /// 用户功能关系表
    /// </summary>
    [Table("GZ_UserFunctionRight")]
    public class GZ_UserFunctionRight: CreateDateEntity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// 功能Id
        /// </summary>
        [Required]
        public Guid FunctionId { get; set; }

    }
}