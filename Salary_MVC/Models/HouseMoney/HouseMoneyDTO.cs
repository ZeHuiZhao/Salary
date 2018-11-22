using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models
{
    public class ImportInput
    {
        /// <summary>
        /// excel路径
        /// </summary>
        [Required]
        public string FilePath { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        [Required]
        public string Month { get; set; }
    }


    public class GetEntityInput
    {
        /// <summary>
        /// 月份
        /// </summary>
        public string Month { get; set; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string Name { get; set; }
    }

    public class ApproveInput
    {
        /// <summary>
        /// 被审核的记录Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 操作
        /// </summary>
        public DataModel.GZ_ApproveLog.ApproveLogCategory Handler { get; set; }

        /// <summary>
        /// 审核意见
        /// </summary>
        public string Comment { get; set; }
    }

    public class GetEntityByApprove: GetEntityInput
    {
        public TabEnum TabIndex { get; set; }
    }

    public enum TabEnum
    {
        待审核=1,
        已审核 = 2
    }
}