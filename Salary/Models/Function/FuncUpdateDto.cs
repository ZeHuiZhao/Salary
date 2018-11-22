using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.Function
{
    public class FuncUpdateDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// 导航名称
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// 导航父亲id  如果添加为父id，为0
        /// </summary>
        [Required]
        public int ParentId { get; set; }
        /// <summary>
        /// 导航排序
        /// </summary>
        [Required]
        public int DisOrder { get; set; }
        /// <summary>
        /// 导航是否可用 0不可用，1.可用
        /// </summary>
        [Required]
        public int Enable { get; set; }
        /// <summary>
        /// 导航图标
        /// </summary>
        [StringLength(100)]
        public string Icon { get; set; }
        /// <summary>
        /// 导航链接
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Url { get; set; }
    }
}