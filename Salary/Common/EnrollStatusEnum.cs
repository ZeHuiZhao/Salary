using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Salary.Common
{
    public enum EnrollStatusEnum
    {
        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部")]
        All=0,
        /// <summary>
        /// 渠道未审核
        /// </summary>
        [Description("渠道未审核")]
        ChannelUnApproved = 1,
        /// <summary>
        /// 渠道审核不通过
        /// </summary>
        [Description("渠道审核不通过")]
        ChannelNoPass =2,
        /// <summary>
        /// 中力未审核
        /// </summary>
        [Description("中力未审核")]
        ZLUnApproved =3,
        /// <summary>
        /// 中力审核不通过
        /// </summary>
        [Description("中力审核不通过")]
        ZLNoPass =4,
        /// <summary>
        /// 未参课
        /// </summary>
        [Description("未参课")]
        NotParticipate =5,
        /// <summary>
        /// 已参课
        /// </summary>
        [Description("已参课")]
        AlreadyParticipate =6,

        /// <summary>
        /// 活动未审核
        /// </summary>
        [Description("未审核")]
        ActivityUnApproved=7,

        /// <summary>
        /// 活动审核不通过
        /// </summary>
        [Description("审核不通过")]
        ActivityNoPass=8,

        /// <summary>
        /// 活动未签到
        /// </summary>
        [Description("未签到")]
        NoSignIn=9,

        /// <summary>
        /// 活动已签到
        /// </summary>
        [Description("已签到")]
        SignIn = 10


    }
}