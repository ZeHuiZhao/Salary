using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.User
{
    public class ChannelStatue
    {
        /// <summary>
        /// 渠道id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 渠道开通状态  0未开通  1已开通
        /// </summary>
        public int Status { get; set; }
    }
}