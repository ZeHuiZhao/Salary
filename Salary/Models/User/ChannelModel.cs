using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.User
{
    public class ChannelModel
    {
        /// <summary>
        /// 渠道id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 渠道名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 渠道状态  1可用 0停用
        /// </summary>
        public int IsActive { get; set; }
    }
}