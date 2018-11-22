using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.Company
{
    public class ComapnyDto
    {
        public int Id { get; set; }
        public int RowNum { get; set; }
        public string CompanyName { get; set; }
        public string CreateTime { get; set; }
        public string SourceType { get; set; }
        public string TrueName { get; set; }
        public string Industry { get; set; }
        public string ContactJob { get; set; }
        public string ContactPhone { get; set; }
        public string ContactName { get; set; }
        /// <summary>
        /// 是否最新 0不是 1是
        /// </summary>
        public int IsNew { get; set; }
    }
}