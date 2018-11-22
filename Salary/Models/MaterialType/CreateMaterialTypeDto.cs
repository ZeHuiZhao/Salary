﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.MaterialType
{
    public class CreateMaterialTypeDto
    {
        /// <summary>
        /// 类别名称
        /// </summary>
        [StringLength(50)]
        [Required]
        public string TypeName { get; set; }
        /// <summary>
        /// 封面图片
        /// </summary>
        [StringLength(500)]
        [Required]
        public string CoverImg { get; set; }
    }
}