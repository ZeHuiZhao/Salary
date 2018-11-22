using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Salary_MVC.DataModel
{
    /// <summary>
    /// 工资明细表
    /// </summary>
    [Table("GZ_MonthlySalaryDetail")]
    public class GZ_MonthlySalaryDetail : BaseEntity
    {
        /// <summary>
        /// 员工id
        /// </summary>
        [Required]
        public Guid EmployeeId { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        [Required]
        public Guid DepartmentId { get; set; }
        /// <summary>
        /// 财务核算单位id
        /// </summary>
        [Required]
        public Guid FinancailUnitId { get; set; }

        /// <summary>
        /// 基本工资
        /// </summary>
        [Required]
        [StringLength(200)]
        public string BaseSalary_Pwd { get; set; }
        /// <summary>
        /// 基本工资
        /// </summary>
        [NotMapped]
        public decimal BaseSalary
        {
            get { return Convert.ToDecimal(pass.get_password_ASC(BaseSalary_Pwd)); }
            set { BaseSalary_Pwd = pass.set_password_ASC(value.ToString()); }
        }
        /// <summary>
        /// 津贴
        /// </summary>
        [Required]
        [StringLength(200)]
        public string BonusSalary_Pwd { get; set; }
        /// <summary>
        /// 津贴
        /// </summary>
        [NotMapped]
        public decimal BonusSalary
        {
            get { return Convert.ToDecimal(pass.get_password_ASC(BonusSalary_Pwd)); }
            set { BonusSalary_Pwd = pass.set_password_ASC(value.ToString()); }
        }
        /// <summary>
        /// 工资总额
        /// </summary>
        [Required]
        [StringLength(200)]
        public string TotalSalary_Pwd { get; set; }
        /// <summary>
        /// 工资总额
        /// </summary>
        [NotMapped]
        public decimal TotalSalary
        {
            get { return Convert.ToDecimal(pass.get_password_ASC(TotalSalary_Pwd)); }
            set { TotalSalary_Pwd = pass.set_password_ASC(value.ToString()); }
        }

        public decimal SalaryDays { get; set; }

        /// <summary>
        /// 应发基本工资
        /// </summary>
        [Required]
        [StringLength(200)]
        public string PayableSalary_Pwd { get; set; }
        /// <summary>
        /// 应发基本工资
        /// </summary>
        [NotMapped]
        public decimal PayableSalary
        {
            get { return Convert.ToDecimal(pass.get_password_ASC(PayableSalary_Pwd)); }
            set { PayableSalary_Pwd = pass.set_password_ASC(value.ToString()); }
        }
        [Required]
        [StringLength(200)]
        public string AwardMoney_Pwd { get; set; }
        /// <summary>
        /// 奖金
        /// </summary>
        [NotMapped]
        public decimal AwardMoney
        {
            get { return Convert.ToDecimal(pass.get_password_ASC(AwardMoney_Pwd)); }
            set { AwardMoney_Pwd = pass.set_password_ASC(value.ToString()); }
        }
        [Required]
        [StringLength(200)]
        public string PercentageMoney_Pwd { get; set; }
        /// <summary>
        /// 提成
        /// </summary>
        [NotMapped]
        public decimal PercentageMoney
        {
            get { return Convert.ToDecimal(pass.get_password_ASC(PercentageMoney_Pwd)); }
            set { PercentageMoney_Pwd = pass.set_password_ASC(value.ToString()); }
        }
        [Required]
        [StringLength(200)]
        public string MakeupMoney_Pwd { get; set; }
        /// <summary>
        /// 补发
        /// </summary>
        [NotMapped]
        public decimal MakeupMoney
        {
            get { return Convert.ToDecimal(pass.get_password_ASC(MakeupMoney_Pwd)); }
            set { MakeupMoney_Pwd = pass.set_password_ASC(value.ToString()); }
        }
        [Required]
        [StringLength(200)]
        public string PayableOther_Pwd { get; set; }
        /// <summary>
        /// 应发其它
        /// </summary>
        [NotMapped]
        public decimal PayableOther
        {
            get { return Convert.ToDecimal(pass.get_password_ASC(PayableOther_Pwd)); }
            set { PayableOther_Pwd = pass.set_password_ASC(value.ToString()); }
        }
        //[Required]
        //[StringLength(200)]
        [NotMapped]
        public string PayableTotal_Pwd { get { return pass.set_password_ASC(this.PayableTotal.ToString()); } }
        /// <summary>
        /// 应发合计
        /// </summary>
        [NotMapped]
        public decimal PayableTotal
        {
            get { return this.PayableSalary + this.AwardMoney + this.PayableOther + this.PercentageMoney + this.MakeupMoney; }
            //set { PayableTotal_Pwd = pass.set_password_ASC(value.ToString()); }
        }
        [Required]
        [StringLength(200)]
        public string SocialMoney_Pwd { get; set; }
        /// <summary>
        /// 社保
        /// </summary>
        [NotMapped]
        public decimal SocialMoney
        {
            get { return Convert.ToDecimal(pass.get_password_ASC(SocialMoney_Pwd)); }
            set { SocialMoney_Pwd = pass.set_password_ASC(value.ToString()); }
        }
        [Required]
        [StringLength(200)]
        public string HouseMoney_Pwd { get; set; }
        /// <summary>
        /// 公积金
        /// </summary>
        [NotMapped]
        public decimal HouseMoney
        {
            get { return Convert.ToDecimal(pass.get_password_ASC(HouseMoney_Pwd)); }
            set { HouseMoney_Pwd = pass.set_password_ASC(value.ToString()); }
        }
        //[Required]
        //[StringLength(200)]
        //public string TaxAmount_Pwd { get; set; }
        /// <summary>
        /// 纳税额
        /// </summary>
        [NotMapped]
        public decimal TaxAmount
        {
            get
            {
                var rt = this.PayableTotal - this.SocialMoney - this.HouseMoney;
                rt = rt < 0 ? 0 : rt;
                return rt;
            }
            //set { TaxAmount_Pwd = pass.set_password_ASC(value.ToString()); }
        }
        //[Required]
        //[StringLength(200)]
        [NotMapped]
        public string TaxMoney_Pwd { get { return pass.set_password_ASC(this.TaxMoney.ToString()); } }
        /// <summary>
        /// 个人所得税
        /// </summary>
        [NotMapped]
        public decimal TaxMoney
        {
            get { return Common.TaxHelper.Compute(this.TaxAmount); }
            //set { TaxMoney_Pwd = pass.set_password_ASC(value.ToString()); }
        }
        [Required]
        [StringLength(200)]
        public string PunishMoney_Pwd { get; set; }
        /// <summary>
        /// 赔偿
        /// </summary>
        [NotMapped]
        public decimal PunishMoney
        {
            get { return Convert.ToDecimal(pass.get_password_ASC(PunishMoney_Pwd)); }
            set { PunishMoney_Pwd = pass.set_password_ASC(value.ToString()); }
        }
        [Required]
        [StringLength(200)]
        public string CreditMoney_Pwd { get; set; }
        /// <summary>
        /// 挂账
        /// </summary>
        [NotMapped]
        public decimal CreditMoney
        {
            get { return Convert.ToDecimal(pass.get_password_ASC(CreditMoney_Pwd)); }
            set { CreditMoney_Pwd = pass.set_password_ASC(value.ToString()); }
        }
        [Required]
        [StringLength(200)]
        public string ReduceOther_Pwd { get; set; }
        /// <summary>
        /// 应扣其它
        /// </summary>
        [NotMapped]
        public decimal ReduceOther
        {
            get { return Convert.ToDecimal(pass.get_password_ASC(ReduceOther_Pwd)); }
            set { ReduceOther_Pwd = pass.set_password_ASC(value.ToString()); }
        }
        //[Required]
        //[StringLength(200)]
        [NotMapped]
        public string ReduceTotal_Pwd { get { return pass.set_password_ASC(this.ReduceTotal.ToString()); } }
        /// <summary>
        /// 应扣合计
        /// </summary>
        [NotMapped]
        public decimal ReduceTotal
        {
            get { return this.SocialMoney+this.HouseMoney+this.TaxMoney+this.PunishMoney+this.CreditMoney+this.ReduceOther; }
            //set { ReduceTotal_Pwd = pass.set_password_ASC(value.ToString()); }
        }
        //[Required]
        //[StringLength(200)]
        //public string RealPay_Pwd { get; set; }
        /// <summary>
        /// 实发金额
        /// </summary>
        [NotMapped]
        public decimal RealPay
        {
            get { return this.PayableTotal - this.ReduceTotal; }
            //set { RealPay_Pwd = pass.set_password_ASC(value.ToString()); }
        }
        /// <summary>
        /// 实发金额加密
        /// </summary>
        [NotMapped]
        public string RealPay_Pwd
        {
            get { return pass.set_password_ASC(this.RealPay.ToString()); }
        }
        /// <summary>
        /// 工资主表id
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// 公司id
        /// </summary>
        [Required]
        public Guid CompanyId { get; set; }
    }
}