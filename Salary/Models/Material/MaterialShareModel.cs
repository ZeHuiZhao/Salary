using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.Material
{
    public class MaterialShareModel
    {
        /// <summary>
        /// 微信的openid
        /// </summary>
        [Required]
        public string OpenId { get; set; }
        /// <summary>
        /// 素材id
        /// </summary>
        [Required]
        public int MId { get; set; }
    }
}