using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary_MVC.DataModel
{
    public class CreateDateEntity:BaseEntity
    {
        /// <summary>
        /// 创建人Id
        /// </summary>
       // [Required]
        public Guid? CreateUser { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
       // [Required]
        public DateTime? CreateDate { get; set; }
    }
}