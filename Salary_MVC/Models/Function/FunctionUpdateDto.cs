using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models.Function
{
    public class FunctionUpdateDto : UpdateDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// URI
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 图标(一个或者多个css的class的名称)
        /// </summary>
        public string Ico { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Order { get; set; }
    }
}