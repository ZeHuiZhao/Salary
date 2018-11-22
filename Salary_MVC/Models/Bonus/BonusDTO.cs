using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models
{
    public class BonusAdd
    {

        /// <summary>
        /// 员工id
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// 基本工资
        /// </summary>
        public decimal Money { set; get; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public string Attachment { get; set; }
    }

    public class BonusQuery
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public Salary_MVC.Enum.ApproveStatus Status { get; set; }

        public DateTime Month { get; set; }

        public Guid? CompanyId { get; set; }

        public Guid? DepartmentId { get; set; }

        //'Status': I_search_status,
        //           'Name': I_search_name,
        //           'CompanyId': $('#I_search_companyId').val(),
        //           'DepartmentId': $('#I_search_departmentId').val(),
        //           'Month': $('#I_search_month').val(),
    }

    public class BonusEdit:Models.UpdateDto
    {
      public decimal Money { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public string Comment { set; get; }
        public string FilePath { set; get; }
    }

    public class StartApproveBatch
    {
        public Guid[] TargetIds { get; set; }
    }

    public class BonusQueryByApprove
    {
        public TabEnum TabIndex { get; set; }
        public string Name { get; set; }
    }

    public class ApproveBatchInput
    {
        public Guid[] TargetIds { get; set; }

        public DataModel.GZ_ApproveLog.ApproveLogCategory Handler { get; set; }

        /// <summary>
        /// 审核意见
        /// </summary>
        public string Comment { get; set; }
    }
}