using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.Function
{
    public class FuncModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 导航名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 导航图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 导航链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 导航父亲id
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 导航排序
        /// </summary>
        public int DisOrder { get; set; }
        /// <summary>
        /// 导航是否可用
        /// </summary>
        public int Enable { get; set; }
    }
}