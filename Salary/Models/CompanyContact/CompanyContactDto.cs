using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.CompanyContact
{
    public class CompanyContactDto
    {
        public int Id { get; set; }
        public int RowNum { get; set; }

        public int CId { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// 联系人职位
        /// </summary>
        public string ContactJob { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        public string WechatNum { get; set; }
        /// <summary>
        /// 销售员
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 客户表id（公司Id）
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 是否第一联系人
        /// </summary>
        public int? IsFirst { get; set; }

        /// <summary>
        /// 最后一次联系时间
        /// </summary>
        public string LastTime { get; set; }

        /// <summary>
        /// qq号
        /// </summary>
        public string QQNum { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        public int SalesId { get; set; }
    }
}