using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.User
{
    public class QueryChannelDto
    {
        /// <summary>
        /// 渠道类别
        /// </summary>
        public int? Type { get; set; }
        /// <summary>
        /// 渠道名称
        /// </summary>
        public string ChannelName { get; set; }
    }
}