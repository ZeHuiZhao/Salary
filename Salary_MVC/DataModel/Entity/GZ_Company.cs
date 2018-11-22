using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salary_MVC.DataModel
{
    /// <summary>
    /// 考勤公司表
    /// </summary>
    [Table("GZ_Company")]
    public class GZ_Company : BaseEntity
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        [StringLength(500)]
        [Required]
        public string Name { get; set; }

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

        /// <summary>
        /// 用户中心公司id
        /// </summary>
        [Required]
        public int UrCpId { get; set; }
    }
}