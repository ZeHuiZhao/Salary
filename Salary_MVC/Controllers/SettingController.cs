using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Salary_MVC.Common;
using Salary_MVC.Models.Role;
using Salary_MVC.Models.Function;
using Salary_MVC.Models.FunctionGroup;
using Salary_MVC.Models.User;
using Salary_MVC.Models.UserFunctionRight;
using Salary_MVC.Models.FunctionGroupRight;

namespace Salary_MVC.Controllers
{
    public partial class SettingController : ZlController
    {
        // GET: Setting
        public ActionResult UserList()
        {
            return View();
        }

        public ActionResult FunctionGroupList()
        {
            return View();
        }

        public ActionResult SetUserFunctionRight()
        {
            return View();
        }

        public ActionResult SetFunctionGroupRight()
        {
            return View();
        }


        #region  MyRegion 角色管理
        Services.RoleService SR = new Services.RoleService();

        public ActionResult RoleList()
        {
            return View();
        }

        public ActionResult AddRole(AddRoleDto dto)
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

            var a = SR.AddRole(dto);
            if (a == 1)
            {
                return Json(new OperationResult(OperationResultType.Success, "新增成功", "", 1));
            }
            return Json(new OperationResult(OperationResultType.Error, "新增失败"));
        }

        public ActionResult UpdateRole(UpdateRoleDto dto)
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

            var a = SR.UpdateRole(dto);
            if (a == 1)
            {
                return Json(new OperationResult(OperationResultType.Success, "修改成功", "", 1));
            }
            return Json(new OperationResult(OperationResultType.Error, "修改失败"));
        }

        public ActionResult GetOneRole(Guid id)
        {
            var one_list = SR.GetOneRole(id);
            return Json(new OperationResult(OperationResultType.Success, "", one_list));
        }

        public ActionResult DelRole(Guid id)
        {
            int result = SR.DelRole(id);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "删除成功"));
            }
            else
            {
                return Json(new OperationResult(OperationResultType.Error, "删除失败"));
            }

        }

        public ActionResult GetRoleList()
        {
            var list = SR.QueryRole();
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        #endregion


        #region MyRegion 功能管理
        Services.FunctionService FS = new Services.FunctionService();

        public ActionResult FunctionList()
        {
            return View();
        }

        public ActionResult AddFunction(Models.FunctionCreateDto dto)
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

            var a = FS.AddFunction(dto);
            if (a == 1)
            {
                return Json(new OperationResult(OperationResultType.Success, "新增成功", "", 1));
            }
            return Json(new OperationResult(OperationResultType.Error, "新增失败"));
        }

        public ActionResult GetFunctionName()
        {
            var list = FS.GetFunctionName();
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        public ActionResult GetFunctionList()
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
            var list = FS.GetFunctionList();
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        public ActionResult GetOneFunction(Guid id)
        {
            var one_list = FS.GetOneFunction(id);
            return Json(new OperationResult(OperationResultType.Success, "", one_list));
        }

        public ActionResult UpdateFunction(FunctionUpdateDto dto)
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

            var a = FS.UpdateFunction(dto);
            if (a == 1)
            {
                return Json(new OperationResult(OperationResultType.Success, "修改成功", "", 1));
            }
            return Json(new OperationResult(OperationResultType.Error, "修改失败"));
        }

        public ActionResult DelFunction(Guid id)
        {
            int result = FS.DelFunction(id);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "删除成功"));
            }
            else
            {
                return Json(new OperationResult(OperationResultType.Error, "删除失败"));
            }

        }

        #endregion


        #region MyRegion 功能组管理

        Services.FunctionGroupService FG = new Services.FunctionGroupService();

        public ActionResult FunctionGroup()
        {
            return View();
        }

        public ActionResult AddFunctionGroup(AddFunctionGroupDto dto)
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

            var a = FG.AddFunctionGroup(dto);
            if (a == 1)
            {
                return Json(new OperationResult(OperationResultType.Success, "新增成功", "", 1));
            }
            return Json(new OperationResult(OperationResultType.Error, "新增失败"));
        }

        public ActionResult UpdateFunctionGroup(UpdateFunctionGroupDto dto)
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

            var a = FG.UpdateFunctionGroup(dto);
            if (a == 1)
            {
                return Json(new OperationResult(OperationResultType.Success, "修改成功", "", 1));
            }
            return Json(new OperationResult(OperationResultType.Error, "修改失败"));
        }

        public ActionResult GetFunctionGroupList(QueryFunctionGroupDto dto)
        {
            var list = FG.GetFunctionGroupList(dto);
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        public ActionResult GetOneFunctionGroup(Guid id)
        {
            var one_list = FG.GetOneFunctionGroup(id);
            return Json(new OperationResult(OperationResultType.Success, "", one_list));
        }

        public ActionResult DelFunctionGroup(Guid id)
        {
            int result = FG.DelFunctionGroup(id);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "删除成功"));
            }
            else
            {
                return Json(new OperationResult(OperationResultType.Error, "删除失败"));
            }

        }

        #endregion


        #region MyRegion 用户管理
        Services.UserService US = new Services.UserService();

        public ActionResult GetRoleName()
        {
            var list = US.GetRoleName();
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        public ActionResult GetFunctionGroup()
        {
            var list = US.GetFunctionGroup();
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        public ActionResult AddUser(AddUserDto dto)
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

            var a = US.AddUser(dto);
            if (a > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "新增成功", "", 1));
            }
            return Json(new OperationResult(OperationResultType.Error, "新增失败"));

        }


        public ActionResult GetUserList()
        {
            var list = US.GetUserList();
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        public ActionResult GetOneUser(Guid id)
        {
            var one_list = US.GetOneUser(id);
            return Json(new OperationResult(OperationResultType.Success, "", one_list));
        }

        public ActionResult DelOneUser(Guid id)
        {
            int result = US.DelOneUser(id);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "删除成功"));
            }
            else
            {
                return Json(new OperationResult(OperationResultType.Error, "删除失败"));
            }
        }

        public ActionResult UpdateUser(UpdateUserDto dto)
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

            var a = US.UpdateUser(dto);
            if (a > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "修改成功", "", 1));
            }
            return Json(new OperationResult(OperationResultType.Error, "修改失败"));
        }

        #endregion


        #region MyRegion 用户权限管理

        Services.UserFunctionRightService URS = new Services.UserFunctionRightService();

        public ActionResult GetPrivilegeList()
        {
            var list = URS.GetPrivilegeList();
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        public ActionResult GetUserPrivilege(Guid id)
        {
            var one_list = URS.GetUserPrivilege(id);
            return Json(new OperationResult(OperationResultType.Success, "", one_list));
        }


        public ActionResult UpdateUserPrivilege(UpdateUserFunctionRightDto dto)
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

            var a = URS.UpdateUserPrivilege(dto);
            if (a > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "保存成功", "", 1));
            }
            return Json(new OperationResult(OperationResultType.Error, "保存失败"));

        }


        public ActionResult GetUserName(Guid id)
        {
            var list = URS.GetUserName(id);
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        #endregion


        #region MyRegion 用户组权限管理

        Services.FunctionGroupRightService FRS = new Services.FunctionGroupRightService();

        public ActionResult GetGroupPrivilegeList()
        {
            var list = FRS.GetGroupPrivilegeList();
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        public ActionResult GetGroupName(Guid id)
        {
            var list = FRS.GetGroupName(id);
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        public ActionResult UpdateGroupPrivilege(UpdateFunctionGroupRightDto dto)
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

            var a = FRS.UpdateGroupPrivilege(dto);
            if (a > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "保存成功", "", 1));
            }
            return Json(new OperationResult(OperationResultType.Error, "保存失败"));

        }

        public ActionResult GetGroupPrivilege(Guid id)
        {
            var one_list = FRS.GetGroupPrivilege(id);
            return Json(new OperationResult(OperationResultType.Success, "", one_list));
        }

        #endregion






    }
}


