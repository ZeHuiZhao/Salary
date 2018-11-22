using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.Company
{
    public class ToggleCompany
    {
        /// <summary>
        /// 客户id
        /// </summary>
        [Required]
        public List<int> Ids { get; set; }
        /// <summary>
        /// 销售员Id
        /// </summary>
        [Required]
        public int SalesId { get; set; }
    }
}