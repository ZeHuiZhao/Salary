using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salary_MVC.DataModel
{
    /// <summary>
    /// 津贴记录表
    /// </summary>
    [Table("GZ_Bonus")]
    public class GZ_Bonus : UpdateDateEntity
    {
        /// <summary>
        /// 员工id
        /// </summary>
        [Required]
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// 基本工资
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Money { get; set; }

        /// <summary>
        /// 基本工资
        /// </summary>
        [NotMapped]
        public decimal Money_pwd
        {
            get { return Convert.ToDecimal(pass.get_password_ASC(Money)); }
            set { Money = pass.set_password_ASC(value.ToString()); }
        }


        /// <summary>
        /// 开始日期
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [Required(AllowEmptyStrings =true)]
        [StringLength(1024)]
        public string Comment { get; set; }

        /// <summary>
        /// 审核状态（30待发起审核，31待财务审核，12财务同意，23财务否决）
        /// </summary>
        [Required]
        public Salary_MVC.Enum.ApproveStatus Status { get; set; }

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
        /// 公司id
        /// </summary>
        [Required]
        public Guid CompanyId { get; set; }

    }
}