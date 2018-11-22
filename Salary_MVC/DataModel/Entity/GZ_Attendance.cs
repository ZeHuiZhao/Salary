using Salary_MVC.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salary_MVC.DataModel
{
    [Table("GZ_Attendance")]
    public class GZ_Attendance:UpdateDateEntity
    {
        /// <summary>
        /// 公司id
        /// </summary>
        [Required]
        public Guid CompanyId { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        [Required]
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// 员工id
        /// </summary>
        [Required]
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// 考勤表用户id
        /// </summary>
        [Required]
        public int KProjectId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [StringLength(20)]
        [Required]
        public string Mobile { get; set; }

        /// <summary>
        /// 考勤月份
        /// </summary>
        [StringLength(7)]
        [Required]
        public string Month { get; set; }

        /// <summary>
        /// 应出勤天数
        /// </summary>
        [DecimalPrecision(5, 2)]
        [Required]
        public decimal TotalDays { get; set; }

        /// <summary>
        /// 实际出勤天数
        /// </summary>
        [DecimalPrecision(5, 3)]
        [Required]
        public decimal RealDays { get; set; }

        /// <summary>
        /// 缺勤天数
        /// </summary>
        [DecimalPrecision(5, 2)]
        [Required]
        public decimal AbsenteeismDays { get; set; }

        /// <summary>
        /// 病假/时
        /// </summary>
        [DecimalPrecision(15, 6)]
        [Required]
        public decimal SickLeave { get; set; }

        /// <summary>
        /// 事假/时
        /// </summary>
        [DecimalPrecision(15, 6)]
        [Required]
        public decimal CompassionateLeave { get; set; }
        

        /// <summary>
        /// 调休/时
        /// </summary>
        [DecimalPrecision(15, 6)]
        [Required]
        public decimal BreakDown { get; set; }

        
        /// <summary>
        /// 年假/时
        /// </summary>
        [DecimalPrecision(15, 6)]
        [Required]
        public decimal AnnualLeave { get; set; }
        
        /// <summary>
        /// 其它带薪假时/时
        /// </summary>
        [DecimalPrecision(15, 6)]
        [Required]
        public decimal OtherLeave { get; set; }
        
        

        /// <summary>
        /// 请假/分钟
        /// </summary>
        [DecimalPrecision(15, 6)]
        [Required]
        public decimal LeaveMinute { get; set; }

        /// <summary>
        /// 迟到/小时（需要扣工资）
        /// </summary>
        [DecimalPrecision(15, 6)]
        [Required]
        public decimal BelateByHour { get; set; }

        /// <summary>
        /// 迟到/分钟(总)
        /// </summary>
        [DecimalPrecision(15, 6)]
        [Required]
        public decimal BelateMinute { get; set; }

        /// <summary>
        /// 外勤/分钟
        /// </summary>
        [DecimalPrecision(15, 6)]
        [Required]
        public decimal OutsideMinute { get; set; }

        /// <summary>
        /// 早退/分钟
        /// </summary>
        [DecimalPrecision(15, 6)]
        [Required]
        public decimal LeaveEarlyMinute { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(1024)]
        public string Remark { get; set; }

        /// <summary>
        /// 审核状态（30系统生成，31待用户确认，12用户同意，23用户否决，10人力资本总监同意，25人力资本总监否决，11系统强制同意）
        /// </summary>
        [Required]
        public int Status { get; set; }

        /// <summary>
        /// 确认出勤天数
        /// </summary>
        [Required]
        [DecimalPrecision(5, 3)]
        public decimal FinalDays { get; set; }

        /// <summary>
        /// 数据来源（0考勤系统，1用户上传）
        /// </summary>
        [Required]
        public int DataSourceType { get; set; }
    }
}