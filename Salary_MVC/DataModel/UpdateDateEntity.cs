using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary_MVC.DataModel
{
    public class UpdateDateEntity:CreateDateEntity
    {
        /// <summary>
        /// 更新用户
        /// </summary>
       // [Required]
        public Guid? LastUpdateUser { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
      //  [Required]
        public DateTime? LastUpdateDate { get; set; }
    }
}