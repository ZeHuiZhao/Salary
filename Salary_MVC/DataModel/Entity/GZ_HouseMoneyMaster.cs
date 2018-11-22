using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace Salary_MVC.DataModel
{
    [Table("GZ_HouseMoneyMaster")]
    public partial class GZ_HouseMoneyMaster: CreateDateEntity
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
        public Salary_MVC.Enum.ApproveStatus Status { get; set; }

        /// <summary>
        /// 源文件路径（file/公积金/2018-10/2018_10_09_09_27_335_xxxx.xlsx）
        /// </summary>
        [StringLength(512)]
        [Required]
        public string FilePath { get; set; }

        public TemplateIndex Template { get; set; }

        public enum TemplateIndex
        {
            深圳=0,
            广州=1
        }
    }
}