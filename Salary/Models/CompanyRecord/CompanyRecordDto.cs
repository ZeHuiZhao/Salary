using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.ComapnyRecord
{
    public class CompanyRecordDto
    {
        public int Id { get; set; }
        public int RowNum { get; set; }
        public string DisplayName { get; set; }
        /// <summary>
        /// 是否可以操作（编辑和删除）（0不可以  1可以）
        /// </summary>
        public int IsAction { get; set; }
        /// <summary>
        /// 联系时间
        /// </summary>
        public string ContactTime { get; set; }

        /// <summary>
        /// 联系摘要
        /// </summary>
        public string ContactSummary { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 销售员
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 客户归属销售员
        /// </summary>
        public string CurrentTrueName { get; set; }
    }
}