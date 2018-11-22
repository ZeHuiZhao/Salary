using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Salary.Common;
using Salary.Models.Function;
using Salary.Service;

namespace Salary.Controllers
{
    public class FunctionController : BaseController
    {
        private readonly FunctionService _function = new FunctionService();

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetFunctionList([FromUri]FuncQueryDto dto)
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

            var list = _function.GetFunctionList(dto);
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="create"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddFunction(FuncCreateDto create)
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

            var a = _function.AddFunction(create);
            if (a == 1)
            {
                return Json(new OperationResult(OperationResultType.Success, "", "", 1));
            }
            return Json(new OperationResult(OperationResultType.Error, "添加失败"));
        }

        /// <summary>
        /// 查询单个菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetFunctionById(int id)
        {
            var model = _function.GetFunctionById(id);
            return Json(new OperationResult(OperationResultType.Success, "", "", model));
        }

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateFunc(FuncUpdateDto update)
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

            var a = _function.UpdateFunc(update);
            if (a == 1)
            {
                return Json(new OperationResult(OperationResultType.Success, "", "", 1));
            }
            return Json(new OperationResult(OperationResultType.Error, "修改失败"));
        }

        /// <summary>
        /// 获取导航数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetMenu()
        {
            var list = _function.GetMenu();
            return Json(new OperationResult(OperationResultType.Success, "", "", list));
        }
        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetPrivilegeList()
        {
            var list = _function.GetPrivilegeList();
            return Json(new OperationResult(OperationResultType.Success, "", "", list));
        }

        /// <summary>
        /// 获取所有可用的父级菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetParentFunc()
        {
            var i = _function.GetParentFunc();
            return Json(new OperationResult(OperationResultType.Success, "", "", i));
        }

    }
}
