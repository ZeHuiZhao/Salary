using Salary_MVC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salary_MVC.DataModel
{
    /// <summary>
    /// 社保主表(深圳)
    /// </summary>
    [Table("GZ_SocialMoneyMasterSZ")]
    public class GZ_SocialMoneyMasterSZ: CreateDateEntity
    {
        /// <summary>
        /// 缴存月份
        /// </summary>
        [StringLength(50)]
        [Required]
        public string Month { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        [Required]
        public Salary_MVC.Enum.ApproveStatus Status { get; set; }

        /// <summary>
        /// 源文件路径（file/社保/2018-10/2018_10_09_09_27_335_xxxx.xlsx）
        /// </summary>
        [StringLength(500)]
        [Required]
        public string FilePath { get; set; }

    }
}