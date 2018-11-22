using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.Enroll
{
    public class ClassTime
    {
        /// <summary>
        /// 课程时间列表
        /// </summary>
        public List<string> Time { get; set; }
        /// <summary>
        /// 封面图片
        /// </summary>
        public string CoverImg { get; set; }
    }
}