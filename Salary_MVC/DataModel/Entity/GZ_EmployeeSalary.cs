using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Salary_MVC.DataModel
{
    /// <summary>
    /// 调薪记录
    /// </summary>
    [Table("GZ_EmployeeSalary")]
    public class GZ_EmployeeSalary:UpdateDateEntity
    {
        /// <summary>
        /// 员工id
        /// </summary>
        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        [StringLength(200)]
        public string Money_Pwd { get; set; }

        /// <summary>
        /// 员工工资
        /// </summary>
        [NotMapped]
        public decimal Money
        {
            get { return Convert.ToDecimal(pass.get_password_ASC(Money_Pwd)); }
            set { Money_Pwd = pass.set_password_ASC(value.ToString()); }
        }
        /// <summary>
        /// 生效日期
        /// </summary>
        [Required]
        [Column(TypeName ="date")]
        public DateTime EffectedDate { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [StringLength(1024)]
        [Required(AllowEmptyStrings =true)]
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