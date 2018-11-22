using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.Report
{
    public class MaterialQueryDto
    {
        /// <summary>
        /// 素材类别
        /// </summary>
        public int? MTId { get; set; }

        /// <summary>
        /// 素材名称
        /// </summary>
        public string MaterialTitle { get; set; }
    }
}