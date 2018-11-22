using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Salary_MVC.Code
{
    public class FieldItem
    {
        /// <summary>
        /// 类名字
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 类描述
        /// </summary>
        public string ClassDescription { get; set; }

        /// <summary>
        /// 属性名字(字段名字)
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 属性描述(字段描述)
        /// </summary>
        public string FieldDescription { get; set; }

        /// <summary>
        /// 需要字符长度
        /// </summary>
        public string StringLength { get; set; }

        /// <summary>
        /// 控件关键字
        /// </summary>
        public string ViewKey { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string FieldType { get; set; }

        /// <summary>
        /// 是否必须
        /// </summary>
        public string IsRequied { get; set; }

    }
}