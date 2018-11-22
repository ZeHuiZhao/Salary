using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.Enroll
{
    public class AuditEnrollDto
    {
        /// <summary>
        /// 报名id
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// 报名意见
        /// </summary>
        public string Opinion { get; set; }

        /// <summary>
        /// 审核是否通过 0不通过 1通过
        /// </summary>
        [Required]
        public int IsPass { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 联系人名称
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// 联系人职位
        /// </summary>
        public string ContactJob { get; set; }
    }
}