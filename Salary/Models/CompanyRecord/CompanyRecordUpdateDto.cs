using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Salary.Models.Base;

namespace Salary.Models.CompanyRecord
{
    public class CompanyRecordUpdateDto:UpdateDto
    {
        /// <summary>
        /// 联系摘要
        /// </summary>
        [StringLength(1000)]
        public string ContactSummary { get; set; }
    }
}