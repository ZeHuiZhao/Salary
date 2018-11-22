using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.Enroll
{
    public class EnrollModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int RowNum { get; set; }
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 客户（公司）名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 联系人名字
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 联系手机
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// 联系人职位
        /// </summary>
        public string ContactJob { get; set; }
        /// <summary>
        /// 课程时间
        /// </summary>
        public string ClassTime { get; set; }
        /// <summary>
        /// 销售员名称
        /// </summary>
        public string SalesName { get; set; }
        /// <summary>
        /// 报名状态
        /// </summary>
        public int EnrollStatus { get; set; }
        /// <summary>
        /// 报名状态详情
        /// </summary>
        public string EnrollStatusName { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public string AuditTime { get; set; } = string.Empty;

        /// <summary>
        /// 备注
        /// </summary>
        public string AuditOpinion { get; set; } = string.Empty;
    }
}