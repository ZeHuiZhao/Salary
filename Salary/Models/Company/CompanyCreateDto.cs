using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.Company
{
    public class CompanyCreateDto
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        [Required]
        public string CompanyName { get; set; }
        /// <summary>
        /// 所属行业
        /// </summary>
        public string Industry { get; set; }
        /// <summary>
        /// 销售员id  销售员id对用户的id，如果为公海客户为0，如果为垃圾客户为-1
        /// </summary>
        [Required]
        public int SalesId { get; set; }
        /// <summary>
        /// 客户来源（1 .中力友友录入2.中力友友手工录入 3活动推广）
        /// </summary>
        [Required]
        public int SourceType { get; set; }
        /// <summary>
        /// 公司规模
        /// </summary>
        public string CompanySize { get; set; }
        /// <summary>
        /// 成立时间
        /// </summary>
        public DateTime? EstablishedTime { get; set; }

        /// <summary>
        /// 指派时间（用来显示new）
        /// </summary>
        public DateTime? AssignTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
    }
}