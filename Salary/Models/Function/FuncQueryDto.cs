using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.Function
{
    public class FuncQueryDto
    {
        /// <summary>
        /// 菜单级别  1.父级菜单  2.子菜单
        /// </summary>
        [Required]
        public int FuncType { get; set; }
    }
}