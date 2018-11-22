using Salary_MVC.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace Salary_MVC.DataModel
{
    [Table("GZ_HouseMoneyDetail")]
    public partial class GZ_HouseMoneyDetail:BaseEntity
    {
        /// <summary>
        /// 个人公积金账号
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
        /// 身份证
        /// </summary>
        [StringLength(50)]
        [Required]
        public string IDCard { get; set; }
     
        /// <summary>
        /// 缴存基数
        /// </summary>
        [DecimalPrecision(18,4)]
        public decimal PaymentStandard { get; set; }
       
        /// <summary>
        /// 公司缴存比例
        /// </summary>
        [DecimalPrecision(7, 4)]
        public decimal CorpRate { get; set; }

        /// <summary>
        /// 个人缴存比例
        /// </summary>
        [DecimalPrecision(7, 4)]
        public decimal PersonalRate { get; set; }

        /// <summary>
        /// 缴存额，加密
        /// </summary>
        [StringLength(200)]
        [Required]
        public string Total_Pwd { get; set; }


        /// <summary>
        /// 基本工资
        /// </summary>
        [NotMapped]
        public decimal Total
        {
            get { return Convert.ToDecimal(pass.get_password_ASC(Total_Pwd)); }
            set { Total_Pwd = pass.set_password_ASC(value.ToString()); }
        }
        /// <summary>
        /// 公积金主表Id
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// 公司缴存额，加密
        /// </summary>
        [StringLength(200)]
        [Required]
        public string CorpMoney_Pwd { get; set; }

        [NotMapped]
        public decimal CorpMoney
        {
            get { return Convert.ToDecimal(pass.get_password_ASC(CorpMoney_Pwd)); }
            set { CorpMoney_Pwd = pass.set_password_ASC(value.ToString()); }
        }
        //个人缴存额
        [StringLength(200)]
        [Required]
        public string PersonalMoney_Pwd { get; set; }

        [NotMapped]
        public decimal PersonalMoney
        {
            get { return Convert.ToDecimal(pass.get_password_ASC(PersonalMoney_Pwd)); }
            set { PersonalMoney_Pwd = pass.set_password_ASC(value.ToString()); }
        }
    }
}