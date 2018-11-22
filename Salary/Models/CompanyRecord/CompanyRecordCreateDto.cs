using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.ComapnyRecord
{
    public class CompanyRecordCreateDto
    {
        /// <summary>
        /// 联系时间
        /// </summary>
        [Required]
        public DateTime ContactTime { get; set; }
        /// <summary>
        /// 联系摘要
        /// </summary>
        [StringLength(1000)]
        public string ContactSummary { get; set; }
        /// <summary>
        /// 联系人id
        /// </summary>
        [Required]
        public int ContactId { get; set; }
        /// <summary>
        /// 销售员id
        /// </summary>
        [Required]
        public int SalesId { get; set; }
    }
}