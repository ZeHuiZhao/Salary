using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Salary_MVC.Common;

namespace Salary_MVC.DataModel
{
    /// <summary>
    /// 员工信息表
    /// </summary>
    [Table("GZ_Employee")]
    public class GZ_Employee : UpdateDateEntity
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        [Required]
        public Guid CorpId { get; set; }


        /// <summary>
        /// 部门Id,用户中心给部门Id,然后再需要同步部门的数据| 用户中心直接返回部门名称
        /// </summary>
        [Required]
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// 财务核算单位Id
        /// </summary>
        public Guid? FinacialUnitId { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [StringLength(50)]
        [Required]
        public string Mobile { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        [StringLength(50)]
        public string IDCard { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        [StringLength(50)]
        public string BankCard { get; set; }

        /// <summary>
        /// 银行卡省份
        /// </summary>
        [StringLength(50)]
        public string BankArea { get; set; }

        /// <summary>
        /// 发薪公司
        /// </summary>
        public int? SalaryGroup { get; set; }

        /// <summary>
        /// 入职日期
        /// </summary>
        [Required]
        public DateTime JoinDate { get; set; }

        /// <summary>
        /// 离职日期
        /// </summary>
        public DateTime? QuitDate { get; set; }

        /// <summary>
        /// 是否领导（0正常打卡，1免打卡）
        /// </summary>
        [Required]
        public int IsLeader { get; set; }

        /// <summary>
        /// 带薪假期
        /// </summary>
        [DecimalPrecision(5, 2)]
        [Required]
        public decimal PaidHoliday { get; set; }


        /// <summary>
        /// 审核状态（21未锁定，32锁定中，11锁定，31解锁中）
        /// </summary>
        [Required]
        public int Status { get; set; }

        /// <summary>
        /// 在职状态（0离职，1在职，2辞退）
        /// </summary>
        [Required]
        public int StatusJob { get; set; }

        /// <summary>
        /// 用户中心的Id
        /// </summary>
        [StringLength(50)]
        public string OriginalId { get; set; }

        /// <summary>
        /// 1-自动计算，0-非自动计算
        /// </summary>
        public CalcSalaryEnum CalcSalary { get; set; }

        public enum CalcSalaryEnum
        {
            Auto=1,
            Other=0
        }
    }
}