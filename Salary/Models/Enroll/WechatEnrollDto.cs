using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.Enroll
{
    public class WechatEnrollDto:BasePaging
    {
        public List<WechatEnrollModel> EnrollList { get; set; }
    }
}