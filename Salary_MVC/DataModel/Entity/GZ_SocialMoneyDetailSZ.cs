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
    /// 社保明细表(深圳)
    /// </summary>
    [Table("GZ_SocialMoneyDetailSZ")]
    public class GZ_SocialMoneyDetailSZ: BaseEntity
    {
        /// <summary>
        /// 个人编号（社保账号）
        /// </summary>
        [StringLength(50)]
        [Required]
        public string Account { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        [StringLength(50)]
        [Required]
        public string IDCard { get; set; }

        /// <summary>
        /// 应收合计
        /// </summary>
        [DecimalPrecision(18, 4)]
        [Required]
        public decimal TotalMoney { get; set; }

        /// <summary>
        /// 个人合计
        /// </summary>
        [DecimalPrecision(18, 4)]
        [Required]
        public decimal TotalPesonal { get; set; }

        /// <summary>
        /// 单位合计
        /// </summary>
        [DecimalPrecision(18, 4)]
        [Required]
        public decimal TotalCorp { get; set; }

        ///// <summary>
        ///// 缴费基数
        ///// </summary>
        //[DecimalPrecision(18, 4)]
        //[Required]
        //public decimal PaymentStandard { get; set; }

        /// <summary>
        /// 养老保险个人交
        /// </summary>
        [DecimalPrecision(18, 4)]
        [Required]
        public decimal YangLaoGeRen { get; set; }

        /// <summary>
        /// 养老保险单位交
        /// </summary>
        [DecimalPrecision(18, 4)]
        [Required]
        public decimal YangLaoDanWei { get; set; }

        /// <summary>
        /// 养老保险基数
        /// </summary>
        [DecimalPrecision(18, 4)]
        [Required]
        public decimal YangLaoJiShu { get; set; }

        /// <summary>
        /// 工伤保险个人交
        /// </summary>
        [DecimalPrecision(18, 4)]
        [Required]
        public decimal GongShangGeRen { get; set; }

        /// <summary>
        /// 工伤保险单位交
        /// </summary>
        [DecimalPrecision(18, 4)]
        [Required]
        public decimal GongShangDanWei { get; set; }

        /// <summary>
        /// 工伤保险基数
        /// </summary>
        [DecimalPrecision(18, 4)]
        [Required]
        public decimal GongShangJiShu { get; set; }

        /// <summary>
        /// 失业保险个人交
        /// </summary>
        [DecimalPrecision(18, 4)]
        [Required]
        public decimal ShiYeGeRen { get; set; }


        /// <summary>
        /// 失业保险单位交
        /// </summary>
        [DecimalPrecision(18, 4)]
        [Required]
        public decimal ShiYeDanWei { get; set; }

        /// <summary>
        /// 失业保险基数
        /// </summary>
        [DecimalPrecision(18, 4)]
        [Required]
        public decimal ShiYeJiShu { get; set; }

        /// <summary>
        /// 医疗保险个人交
        /// </summary>
        [DecimalPrecision(18, 4)]
        [Required]
        public decimal YiLiaoGeRen { get; set; }

        /// <summary>
        /// 医疗保险单位交
        /// </summary>
        [DecimalPrecision(18, 4)]
        [Required]
        public decimal YiLiaoDanWei { get; set; }

        /// <summary>
        /// 医疗保险基数
        /// </summary>
        [DecimalPrecision(18, 4)]
        [Required]
        public decimal YiLiaoJiShu { get; set; }

        /// <summary>
        ///  生育医疗个人交
        /// </summary>
        [DecimalPrecision(18, 4)]
        [Required]
        public decimal ShenYuGeRen { get; set; }

        /// <summary>
        ///  生育医疗单位交
        /// </summary>
        [DecimalPrecision(18, 4)]
        [Required]
        public decimal ShenYuDanWei { get; set; }

        /// <summary>
        ///  生育医疗基数
        /// </summary>
        [DecimalPrecision(18, 4)]
        [Required]
        public decimal ShenYuJiShu { get; set; }

        /// <summary>
        /// 社保主表Id
        /// </summary>
        [Required]
        public Guid MasterId { get; set; }

    }
}