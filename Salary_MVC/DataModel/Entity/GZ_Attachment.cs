using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Salary_MVC.DataModel
{
    /// <summary>
    /// 附件信息
    /// </summary>
    [Table("GZ_Attachment")]
    public class GZ_Attachment:BaseEntity
    {
        /// <summary>
        /// 所属对象的Id
        /// </summary>
        [Required]
        public Guid SourceId { get; set; }

        /// <summary>
        /// 类别（1PDF,2图片）
        /// </summary>
        [Required]
        public CategoryEnum Category { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        [Required]
        [StringLength(512)]
        public string FilePath { get; set; }

        public enum CategoryEnum
        {
            PDF=1,
            图片=2,
            其他=3
        }
    }
}