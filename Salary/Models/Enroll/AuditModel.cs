using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.Enroll
{
    public class AuditModel
    {
        /// <summary>
        /// 报名id
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// 审核是否通过 0不通过 1通过
        /// </summary>
        [Required]
        public int IsPass { get; set; }
    }
}