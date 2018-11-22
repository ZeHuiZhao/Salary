using Salary_MVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models.Employee
{
    public class UpdateIdCardDto:UpdateDto
    {
        /// <summary>
        /// 身份证号码
        /// </summary>
        [StringLength(50)]
        public string IDCard { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        [StringLength(50)]
        public string BankCard { get; set; }

        /// <summary>
        /// 银行卡省份
        /// </summary>
        [StringLength(50)]
        public string BankArea { get; set; }

        /// <summary>
        /// 意见
        /// </summary>
        public string Opinion { get; set; } = string.Empty;

        /// <summary>
        /// 1保存并且加锁 0只保存
        /// </summary>
        public int IsLock { get; set; } = 0;
    }
}