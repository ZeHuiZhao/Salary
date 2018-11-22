using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.Material
{
    public class BrowseDataDto
    {
        [Required]
        public string OpenId { get; set; }

        /// <summary>
        /// 每页显示的条数
        /// </summary>
        [Required]
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页数
        /// </summary>
        [Required]
        public int PageIndex { get; set; }
    }
}