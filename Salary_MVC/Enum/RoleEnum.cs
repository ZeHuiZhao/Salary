using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Enum
{
    /// <summary>
    /// 工资系统业务涉及到的所有角色
    /// </summary>
    public enum RoleEnum
    {
        /// <summary>
        /// HR
        /// </summary>
        HR=0,
        /// <summary>
        /// 人力资本总监
        /// </summary>
        HRManager=1,
        /// <summary>
        /// 财务
        /// </summary>
        Finance=2,
        /// <summary>
        /// 财务经理
        /// </summary>
        FinanceManager=3,
        /// <summary>
        /// CFO
        /// </summary>
        CFO=4,
        /// <summary>
        /// 董办
        /// </summary>
        CEO=5,
        /// <summary>
        /// 员工
        /// </summary>
        Employee=6
    }
}