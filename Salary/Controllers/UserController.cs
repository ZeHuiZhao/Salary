using System.Collections.Generic;
using System.Web.Http;
using Salary.Common;
using Salary.Models.User;
using Salary.Service;

namespace Salary.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserController : BaseController
    {
        private readonly UserService _user = new UserService();
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUserList(int id)
        {
            if (!ModelState.IsValid)
            {
                var msg = string.Empty;
                foreach (var val in ModelState.Values)
                {
                    if (val.Errors.Count > 0)
                    {
                        foreach (var error in val.Errors)
                        {
                            msg = msg + error.ErrorMessage;
                        }
                    }
                }
                return Json(new OperationResult(OperationResultType.Error, msg));
            }
            var list = _user.GetUserList(id);
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddUser(Userinfo model)
        {
            if (!ModelState.IsValid)
            {
                var msg = string.Empty;
                foreach (var val in ModelState.Values)
                {
                    if (val.Errors.Count > 0)
                    {
                        foreach (var error in val.Errors)
                        {
                            msg = msg + error.ErrorMessage;
                        }
                    }
                }
                return Json(new OperationResult(OperationResultType.Error, msg));
            }
            var b = _user.AddUser(model);
            if (b == 1)
            {
                if (model.UserType == (int)UserTypeEnum.Sales)
                {
                    return Json(new OperationResult(OperationResultType.Success, "提交成功"));
                }
                if (model.IsContacts > 0)
                {
                    return Json(new OperationResult(OperationResultType.Success, "开通成功"));
                }
                return Json(new OperationResult(OperationResultType.Success, "新增成功"));
            }else if(b==2)
            {
                return Json(new OperationResult(OperationResultType.Error, "该手机已注册"));
            }
            return Json(new OperationResult(OperationResultType.Error, "新增失败"));
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[HttpPost]
        //public IHttpActionResult AddWechatUser(CreateWechatUser model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var msg = string.Empty;
        //        foreach (var val in ModelState.Values)
        //        {
        //            if (val.Errors.Count > 0)
        //            {
        //                foreach (var error in val.Errors)
        //                {
        //                    msg = msg + error.ErrorMessage;
        //                }
        //            }
        //        }
        //        return Json(new OperationResult(OperationResultType.Error, msg));
        //    }
        //    var b = _user.AddUser(model);
        //    if (b == 1)
        //    {
        //        if (model.UserType == (int)UserTypeEnum.Sales)
        //        {
        //            return Json(new OperationResult(OperationResultType.Success, "提交成功"));
        //        }
        //        return Json(new OperationResult(OperationResultType.Success, "新增成功"));
        //    }
        //    else if (b == 2)
        //    {
        //        return Json(new OperationResult(OperationResultType.Error, "该手机已注册"));
        //    }else if (b==3)
        //    {
        //        return Json(new OperationResult(OperationResultType.Error, "该微信已绑定"));
        //    }
        //    return Json(new OperationResult(OperationResultType.Error, "认证失败"));
        //}

        /// <summary>
        /// 添加用户（多个）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddUserList(List<Userinfo> model)
        {
            var b = _user.AddUser(model);
            if (b == 1)
            {
                return Json(new OperationResult(OperationResultType.Success, "开通成功"));
            }
            else if (b == 2)
            {
                return Json(new OperationResult(OperationResultType.Error, "该手机已注册"));
            }
            return Json(new OperationResult(OperationResultType.Error, "账号已经开通"));
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ResetPassword(int id)
        {
            int result = _user.ResetPassword(id);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "密码重置成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "密码重置失败"));
        }

        /// <summary>
        /// 切换开通
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ToggleUser(int id)
        {
            int result = _user.ToggleUser(id);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "操作成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "操作失败"));
        }

        /// <summary>
        /// 渠道切换状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ToggleChannelUser(ToggleChannelStatus status)
        {
            int result = _user.ToggleChannelUser(status);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "操作成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "操作失败"));
        }

        /// <summary>
        /// 获得单个数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetOneUser(int id)
        {
            return Json(new OperationResult(OperationResultType.Success, "", _user.GetUserById(id)));
        }


        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateUser(UpdateUserDto user)
        {
            if (!ModelState.IsValid)
            {
                var msg = string.Empty;
                foreach (var val in ModelState.Values)
                {
                    if (val.Errors.Count > 0)
                    {
                        foreach (var error in val.Errors)
                        {
                            msg = msg + error.ErrorMessage;
                        }
                    }
                }
                return Json(new OperationResult(OperationResultType.Error, msg));
            }
            int result = _user.UpdateUser(user);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "修改成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "修改失败"));
        }

        /// <summary>
        /// 更新渠道用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateChannelUser(UpdateChannelUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                var msg = string.Empty;
                foreach (var val in ModelState.Values)
                {
                    if (val.Errors.Count > 0)
                    {
                        foreach (var error in val.Errors)
                        {
                            msg = msg + error.ErrorMessage;
                        }
                    }
                }
                return Json(new OperationResult(OperationResultType.Error, msg));
            }
            int result = _user.UpdateChannelUser(dto);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "修改成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "修改失败"));
        }

        /// <summary>
        /// 获取渠道开通状态0未开通  1已开通
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetChannelStatus(int id)
        {
            int result = _user.GetChannelStatus(id);
            return Json(new OperationResult(OperationResultType.Success, "", result));
        }

        /// <summary>
        /// 更新渠道状态
        /// </summary>
        /// <param name="channel">渠道id</param>
        /// <returns></returns>
        public IHttpActionResult UpdateChannelStatus(ChannelStatue channel)
        {
            int result = _user.UpdateChannelStatus(channel);
            if (result == 1)
            {
                return Json(new OperationResult(OperationResultType.Success, "修改成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "修改失败"));
        }

        /// <summary>
        /// 分页查询用户
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUserListByPaging([FromUri]QueryUserDto query)
        {
            if (!ModelState.IsValid)
            {
                var msg = string.Empty;
                foreach (var val in ModelState.Values)
                {
                    if (val.Errors.Count > 0)
                    {
                        foreach (var error in val.Errors)
                        {
                            msg = msg + error.ErrorMessage;
                        }
                    }
                }
                return Json(new OperationResult(OperationResultType.Error, msg));
            }
            var model = _user.GetUserListByPaging(query);
            return Json(new OperationResult(OperationResultType.Success, "", "", model));
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="deleteList"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteUser(List<int> deleteList)
        {
            if (deleteList == null || deleteList.Count == 0)
            {
                return Json(new OperationResult(OperationResultType.Error, "删除失败，请重试"));
            }
            int result = _user.DeleteUser(deleteList);
            if (result >= 1)
            {
                return Json(new OperationResult(OperationResultType.Success, "删除成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "删除失败，请重试"));
        }

        /// <summary>
        /// 获取渠道的用户状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetChannelUserStatus()
        {
            return Json(new OperationResult(OperationResultType.Success, "", 1));
        }


        /// <summary>
        /// 审核提交
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ApprovalUser(UserStatus status)
        {
            int result = _user.ToggleAuditStatus(status);
            return Json(new OperationResult(OperationResultType.Success, "审核成功", result));
        }

        /// <summary>
        /// 获取渠道销售员列表
        /// </summary>
        /// <param name="id">渠道id</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetSaleList(int id)
        {
            var obj = _user.GetSaleList(id);
            if (obj != null)
            {
                return Json(new OperationResult(OperationResultType.Success, "","", obj));
            }
            return Json(new OperationResult(OperationResultType.Error, "没有销售员"));
        }

        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUserinfo([FromUri]string openId)
        {
            if (CheckWechatUser(openId)) return Json(new OperationResult(OperationResultType.PurviewLack, "无权限"));
            var obj = _user.GetSalesUser(openId);
            if (obj != null)
            {
                var channel = _user.GetChannel(obj.ChannelId);
                var o = new { obj.WechatHeadImage, obj.TrueName, obj.PhoneNum, ChannelName = channel.Name };
                return Json(new OperationResult(OperationResultType.Success, "", "", o));
            }
            return Json(new OperationResult(OperationResultType.Error, "没有销售员"));
        }
    }
}
