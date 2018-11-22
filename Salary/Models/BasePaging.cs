using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models
{
    public class BasePaging
    {
        /// <summary>
        /// 每页的记录数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage { get; set; }
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPage { get; set; }
    }
}