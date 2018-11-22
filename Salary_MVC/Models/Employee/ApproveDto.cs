using Salary_MVC.Enum;
using Salary_MVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Salary_MVC.DataModel.GZ_ApproveLog;

namespace Salary_MVC.Models
{
    public class ApproveDto:UpdateDto
    {
        /// <summary>
        /// 用户操作
        /// </summary>
        [Required]
        public ApproveLogCategory UserOperation { get; set; }
        /// <summary>
        /// 用户意见
        /// </summary>
        public string Opinion { get; set; }

        /// <summary>
        /// 审核用户的手机号码
        /// </summary>
        public string Phone { get; set; }
    }

    public class ApproveMultipleDto
    {
        /// <summary>
        /// 用户操作
        /// </summary>
        [Required]
        public ApproveLogCategory UserOperation { get; set; }
        /// <summary>
        /// 用户意见
        /// </summary>
        public string Opinion { get; set; }
        /// <summary>
        /// 审核的id列表
        /// </summary>
        public List<Guid> Ids { get; set; }
    }

    public class ApproveAllDto
    {
        /// <summary>
        /// 用户操作
        /// </summary>
        [Required]
        public ApproveLogCategory UserOperation { get; set; }
        /// <summary>
        /// 用户意见
        /// </summary>
        public string Opinion { get; set; }
    }
}