using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Salary.Common;
using Salary.Models.User;
using Salary.Service;

namespace Salary.Controllers
{
    public class ChannelController : BaseController
    {
        private readonly UserService _user = new UserService();
        /// <summary>
        /// 切换用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ToggleUser(UserStatus status)
        {
            int result = _user.ToggleUser(status);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "操作成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "操作失败"));
        }


        /// <summary>
        /// 获取渠道列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetChannelList([FromUri]QueryChannelDto dto)
        {
            var obj = _user.GetChannelList(dto);
            return Json(new OperationResult(OperationResultType.Success, "","", obj));
        }

        /// <summary>
        /// 获取渠道类型列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetChannelTypeList()
        {
           var channelTypeList =  _user.GetChannelTypeList();
            return Json(new OperationResult(OperationResultType.Success, "", "", channelTypeList));
        }
    }
}
