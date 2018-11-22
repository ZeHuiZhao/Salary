using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.Enroll
{
    public class CreateEnrollDto
    {
        /// <summary>
        /// 客户（公司）名称
        /// </summary>
        [Required]
        public string CompanyName { get; set; }
        /// <summary>
        /// 联系人名字
        /// </summary>
        [Required]
        public string ContactName { get; set; }

        /// <summary>
        /// 联系手机
        /// </summary>
        [Required]
        public string ContactPhone { get; set; }
        /// <summary>
        /// 联系人职位
        /// </summary>
        [Required]
        public string ContactJob { get; set; }
        /// <summary>
        /// 课程时间
        /// </summary>
        [Required]
        public string ClassTime { get; set; }
        /// <summary>
        /// 销售员openId
        /// </summary>
        [Required]
        public string OpenId { get; set; }
        /// <summary>
        /// 渠道id
        /// </summary>
        [Required]
        public int ChannelId { get; set; }
        /// <summary>
        /// 素材id
        /// </summary>
        [Required]
        public int MId { get; set; }

        /// <summary>
        /// 0,不是推广活动 大于0为活动id
        /// </summary>
        [Required]
        public int IsActivity { get; set; }
    }
}