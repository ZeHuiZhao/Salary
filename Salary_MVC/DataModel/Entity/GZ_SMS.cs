using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary_MVC.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salary_MVC.DataModel
{
    /// <summary>
    /// 短信数据表
    /// </summary>
    [Table("GZ_SMS")]
    public class GZ_SMS: CreateDateEntity
    {

        /// <summary>
        /// 手机
        /// </summary>
        [StringLength(50)]
        [Required]
        public string Mobile { get; set; }

        /// <summary>
        /// 数据(填坑的数据)
        /// </summary>
        [StringLength(500)]
        [Required]
        public string RequestBody { get; set; }

        /// <summary>
        /// 短信模板id
        /// </summary>
        [Required]
        public TemplateIdEnum TemplateId { get; set; }

        /// <summary>
        /// 状态（0待发送，1api发送成功，2api发送失败）
        /// </summary>
        [Required]
        public SMSStatusEnum Status { get; set; }

        /// <summary>
        /// 已发送次数
        /// </summary>
        [Required]
        public int SendTimes { get; set; }

        /// <summary>
        /// api返回结果
        /// </summary>
        [StringLength(2048)]
        public string ApiResult { get; set; }

        /// <summary>
        /// 短信完整内容
        /// </summary>
        [StringLength(500)]
        [Required]
        public string FullContent { get; set; }

        public enum SMSStatusEnum
        {
            /// <summary>
            /// 待发送
            /// </summary>
            Ready=0,
            /// <summary>
            /// 发送成功
            /// </summary>
            Success=1,
            /// <summary>
            /// 发送失败
            /// </summary>
            Fault=2
        }

        public enum TemplateIdEnum
        {
            员工信息发起审核= 227210
            , 员工信息审核结果= 212623
            , 社保发起审核= 212649
            , 社保审核结果= 212650
            , 基本工资发起审核= 212651
            , 基本工资审核结果= 212652
            , 考勤确认= 227764
            , 验证码= 213238
                ,综合工资待审核= 225899

                , 综合工资审核结果= 225863
                , 董办待审核= 227214//中力薪酬管家，您好，公司{0}月份工资已生成，请用手机直接登录{1}进行审批
        }
    }
}