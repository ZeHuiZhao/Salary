using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models
{
    public class BaseQueryDto
    {
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
    }
}