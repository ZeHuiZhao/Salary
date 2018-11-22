using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models
{
    public class GenerateMonthSalaryValidateInfo
    {
        public GenerateMonthSalaryValidateInfo()
        {
            this.Attendence = new GenerateMonthSalaryValidateInfoWrapper("../Attendance/Index");
            this.Bonus = new GenerateMonthSalaryValidateInfoWrapper("../Bonus/Approve");
            this.Employee = new GenerateMonthSalaryValidateInfoWrapper("../Employee/Index");
            this.EmployeeSalary = new GenerateMonthSalaryValidateInfoWrapper("../EmployeeSalary/Approve");

            this.HouseMoney = new GenerateMonthSalaryValidateInfoWrapper("../HouseMoneyGZ/Approve");
            this.ShortSalary = new GenerateMonthSalaryValidateInfoWrapper("../ShortSalary/Approve");

            this.SocialMoney = new GenerateMonthSalaryValidateInfoWrapper("../SocialMoneySZ/Approve");
        }
        /// <summary>
        /// 考勤
        /// </summary>
        public GenerateMonthSalaryValidateInfoWrapper Attendence { get; set; }
        /// <summary>
        /// 员工信息
        /// </summary>
        public GenerateMonthSalaryValidateInfoWrapper Employee { get; set; }

        /// <summary>
        /// 津贴
        /// </summary>
        public GenerateMonthSalaryValidateInfoWrapper Bonus { get; set; }
        /// <summary>
        /// 奖惩
        /// </summary>
        public GenerateMonthSalaryValidateInfoWrapper ShortSalary { get; set; }
        /// <summary>
        /// 调薪
        /// </summary>
        public GenerateMonthSalaryValidateInfoWrapper EmployeeSalary { get; set; }

        /// <summary>
        /// 深圳
        /// </summary>
        public GenerateMonthSalaryValidateInfoWrapper HouseMoney { get; set; }

        /// <summary>
        /// 深圳
        /// </summary>
        public GenerateMonthSalaryValidateInfoWrapper SocialMoney { get; set; }


    }

    public class GenerateMonthSalaryValidateInfoWrapper
    {
        public GenerateMonthSalaryValidateInfoWrapper(string url)
        {
            this.IsOk = true;
            this.Msg = new List<string>();
            this.Url = url;
        }
        /// <summary>
        /// 是否就绪
        /// </summary>
        public bool IsOk { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public List<string> Msg { get; set; }
        /// <summary>
        /// 跳转的地址
        /// </summary>
        public string Url { get; set; }

        public GenerateMonthSalaryValidateInfoWrapper AddError(string msg)
        {
            this.IsOk = false;
            this.Msg.Add(msg);
            return this;
        }

        public GenerateMonthSalaryValidateInfoWrapper AddError(string msg, string url)
        {
            this.IsOk = false;
            this.Url = url;
            this.Msg.Add(msg);
            return this;
        }

    }

    public class ApproveByCFOInput : Models.ApproveInput
    {
        public Guid[] Operators { get; set; }
    }

    public class CeoLoginInput
    {
        public string Moblie { get; set; }

        public Guid TargetId { get; set; }

        public string Pwd { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }

        public string Msg { get; set; }
    }

    public class MonthlySalarySum
    {
        public int Index { get; set; }

        public decimal MonthMoney { get; set; }

        public decimal LastMonthMoney { get; set; }

        public string Name { get; set; }
    }

    public class MonthlySalarySumWrapper
    {
        public List<MonthlySalarySum> DepartmentMonty { get; set; }
        public List<MonthlySalarySum> FinanceMoney { get; set; }

        public DataModel.GZ_MonthlySalaryMaster Master { set; get; }

        public int TotalEmployee { get; set; }

        public decimal TotalMoney { get; set; }

        public decimal LastTotalMoney { get; set; }

        public string Status { get; set; }
    }

    public class MonthSalaryEdit:UpdateDto
    {
       
        /// <summary>
        /// 提成
        /// </summary>
        public decimal PercentageMoney
        {
            get;
            set;
        }
      
        /// <summary>
        /// 补发
        /// </summary>
        public decimal MakeupMoney
        {
            get;
            set;
        }
     
        /// <summary>
        /// 应发其它
        /// </summary>
        public decimal PayableOther
        {
            get;
            set;
        }
        
        /// <summary>
        /// 挂账
        /// </summary>
        public decimal CreditMoney
        {
            get;
            set;
        }
      
        /// <summary>
        /// 应扣其它
        /// </summary>
        public decimal ReduceOther
        {
            get;
            set;
        }
    }

    public class MonthSalaryQuery
    {
        /// <summary>
        /// 月份
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required]
        public string Month { get; set; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 公司id
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        public Guid? DepartmentId { get; set; }

        public Guid? FinancailUnitId { get; set; }
    }
}