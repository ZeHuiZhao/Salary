using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salary_MVC.DataModel
{
    /// <summary>
    /// 考勤部门表
    /// </summary>
    [Table("GZ_Department")]
    public class GZ_Department : BaseEntity
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        [StringLength(500)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 公司id
        /// </summary>
        [Required]
        public Guid CpId { get; set; }

        /// <summary>
        /// 用户中心的部门id
        /// </summary>
        [Required]
        public int UcDepartmentId { get; set; }

        /// <summary>
        /// 状态（0.不可用  ，1可用）
        /// </summary>
        [Required]
        public int Status { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        public int Order { get; set; }

    }
}