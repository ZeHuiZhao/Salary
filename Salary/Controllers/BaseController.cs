using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using WxLib;
using Salary.Common;
using Salary.Filter;
using Salary.Service;

namespace Salary.Controllers
{
    // [Login]
    public class BaseController : ApiController
    {
        /// <summary>
        /// 检测微信用户
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        protected bool CheckWechatUser(string openId)
        {
            var user = new UserService().GetSalesUser(openId);

            return user == null || user.UserStatus != (int)UserStatusEnum.Normal;
        }
    }
}
