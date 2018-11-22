using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.User
{
    public class UserinfoDto
    {
        /// <summary>
        /// 显示的序号
        /// </summary>
        public int RowNum { get; set; }

        /// <summary>
        /// 显示的提示文字
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 标识列
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNum { get; set; }
        /// <summary>
        /// 微信头像
        /// </summary>
        public string WechatHeadImage { get; set; }
        /// <summary>
        /// 职业
        /// </summary>
        public string Job { get; set; }
        /// <summary>
        /// 用户状态
        /// </summary>
        public int UserStatus { get; set; }
        /// <summary>
        /// 用户角色  1中力渠道管理员  2渠道管理员  3 渠道销售员
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// 用户状态显示的值
        /// </summary>
        public string UserStatusName { get; set; }

        /// <summary>
        /// 渠道id
        /// </summary>
        public int ChannelId { get; set; }

        /// <summary>
        /// 渠道名称
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImage { get; set; }
        /// <summary>
        /// 是否可以删除 0不能 1能
        /// </summary>
        public int IsDelete { get; set; }
    }
}