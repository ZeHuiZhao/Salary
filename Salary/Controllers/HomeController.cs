using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Salary.Common;
using Salary.Models.Home;
using Salary.Models.User;
using Salary.Service;

namespace Salary.Controllers
{
    public class HomeController : BaseController
    {
        private readonly UserService _user = new UserService();
        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult Login(LoginModel model)
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
            var user = _user.Login(model);
            if (user != null)
            {
                var channelModel = _user.GetChannel(user.ChannelId);
                if (user.UserType != (int)UserTypeEnum.Admin)
                {
                    if (channelModel == null)
                    {
                        return Json(new OperationResult(OperationResultType.Error, "渠道不存在"));
                    }
                }
                Cookies.UserCode = user.Id.ToString();
                Cookies.UserPhone = user.PhoneNum;

                if (model.IsAutoLogin == 1)
                {
                    Cookies.AutoUserID = user.Id.ToString();
                }
                return Json(new OperationResult(OperationResultType.Success, "", new UserinfoDto() { HeadImage = !string.IsNullOrEmpty(user.HeadImage) ? user.HeadImage : string.Empty, Id = user.Id, PhoneNum = user.PhoneNum, TrueName = user.TrueName, UserType = user.UserType, ChannelId = user.UserType == (int)UserTypeEnum.Admin ? 0 : channelModel.Id, ChannelName = user.UserType == (int)UserTypeEnum.Admin ? "中力知识科技" : channelModel.Name, UserStatus = user.UserStatus, Job = user.UserType == (int)UserTypeEnum.ChannelManager ? "渠道管理员" : user.UserType == (int)UserTypeEnum.ZLChannelManager ? "中力渠道管理员" : user.UserType == (int)UserTypeEnum.Sales ? "渠道销售员" : user.UserType == (int)UserTypeEnum.Admin ? "超级管理员" : "" }));
            }
            return Json(new OperationResult(OperationResultType.Error, "用户名或密码错误"));
        }


        /// <summary>
        /// erp 自动登陆
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult ErpLogin([FromUri]string token)
        {
            LogHelper.WriteInfoLog(token);
            var user = _user.ErpLogin(token);
            if (user != null)
            {
                Cookies.UserCode = user.Id.ToString();
                Cookies.UserPhone = user.PhoneNum;
                return Json(new OperationResult(OperationResultType.Success, "", new UserinfoDto() { HeadImage = user.HeadImage, Id = user.Id, PhoneNum = user.PhoneNum, TrueName = user.TrueName, UserType = user.UserType, ChannelId = 0, ChannelName = "中力知识科技", UserStatus = user.UserStatus, Job = user.UserType == (int)UserTypeEnum.ChannelManager ? "渠道管理员" : user.UserType == (int)UserTypeEnum.ZLChannelManager ? "中力渠道管理员" : user.UserType == (int)UserTypeEnum.Sales ? "渠道销售员" : user.UserType == (int)UserTypeEnum.Admin ? "超级管理员" : "" }));
            }
            return Json(new OperationResult(OperationResultType.Error, "用户名或密码错误"));
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CustomPwd(UpdatePwd dto)
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
            int result = _user.CustomPwd(dto);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "密码修改成功"));
            }
            else if (result == -1)
            {
                return Json(new OperationResult(OperationResultType.Error, "用户不存在"));
            }
            else if (result == -2)
            {
                return Json(new OperationResult(OperationResultType.Error, "旧密码不匹配"));
            }
            return Json(new OperationResult(OperationResultType.Error, "密码修改失败"));
        }

        /// <summary>
        /// 自动登陆  返回0为不自动登陆，1自动登陆，跳转到首页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult AutoLogin()
        {
            if (!string.IsNullOrEmpty(Cookies.AutoUserID))
            {
                var user = _user.AutoLogin(Convert.ToInt32(Cookies.AutoUserID));
                if (user != null)
                {
                    Cookies.UserCode = user.Id.ToString();
                    Cookies.UserPhone = user.PhoneNum;
                    Cookies.AutoUserID = user.Id.ToString();
                }
                return Json(new OperationResult(OperationResultType.Success, "", 1));
            }
            return Json(new OperationResult(OperationResultType.Success, "", 0));
        }
        /// <summary>
        /// 获取登陆用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetLoginInfo()
        {
            if (string.IsNullOrEmpty(Cookies.UserPhone))
            {
                return Json(new OperationResult(OperationResultType.Error, "没有登陆信息"));
            }
            return Json(new OperationResult(OperationResultType.Success, "", "", Cookies.UserPhone));
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        public IHttpActionResult UpdatePassword(int id)
        {
            int result = _user.ResetPassword(id);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "密码重置成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "密码重置失败"));
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Logout()
        {
            Cookies.UserCode = "";
            Cookies.AutoUserID = "";
            return Json(new OperationResult(OperationResultType.Success, "", 1));
        }
        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ForgetPassword(string phoneNum)
        {
            var list = _user.GetUserByPhone(phoneNum);
            if (list == null || list.Count == 0)
            {
                return Json(new OperationResult(OperationResultType.Error, "请确认手机号码输入正确"));
            }
            int result = _user.ResetPassword(list.Select(o => o.Id).ToList());
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "密码重置成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "密码重置失败"));
        }


        /// <summary>
        /// 获得角色列表
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetRoleList(string phoneNum)
        {
            var obj = _user.GetRoleList(phoneNum);
            return Json(new OperationResult(OperationResultType.Success, "", obj));
        }

        /// <summary>
        /// 切换角色
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ToggleRole(RoleModel model)
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
            var user = _user.ToggleRole(model);
            if (user != null)
            {
                var channelModel = _user.GetChannel(user.ChannelId);
                if ((int)UserTypeEnum.Admin != user.UserType)
                {
                    if (channelModel == null)
                    {
                        return Json(new OperationResult(OperationResultType.Error, "渠道不存在"));
                    }
                }
                Cookies.UserCode = user.Id.ToString();
                Cookies.UserPhone = user.PhoneNum;
                return Json(new OperationResult(OperationResultType.Success, "", new UserinfoDto() { HeadImage = !string.IsNullOrEmpty(user.HeadImage) ? user.HeadImage : string.Empty, Id = user.Id, PhoneNum = user.PhoneNum, TrueName = user.TrueName, UserType = user.UserType, ChannelId = user.UserType == (int)UserTypeEnum.Admin ? 0 : channelModel.Id, ChannelName = user.UserType == (int)UserTypeEnum.Admin ? "中力知识科技" : channelModel.Name, UserStatus = user.UserStatus, Job = user.UserType == (int)UserTypeEnum.ChannelManager ? "渠道管理员" : user.UserType == (int)UserTypeEnum.ZLChannelManager ? "中力渠道管理员" : user.UserType == (int)UserTypeEnum.Sales ? "渠道销售员" : user.UserType == (int)UserTypeEnum.Admin ? "超级管理员" : "" }));

            }
            return Json(new OperationResult(OperationResultType.Error, "切换用户失败！"));
        }


        /// <summary>
        /// 获取所有角色包括超级管理员（用来角色权限下拉）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAllRole()
        {
            var list = _user.GetAllRole();
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }



        /// <summary>
        /// 保存角色权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateRolePrivilege(RolePrivilege dto)
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
            var i = _user.UpdateRolePrivilege(dto);
            if (i > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "修改成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "保存失败，请重试"));
        }
    }
}
