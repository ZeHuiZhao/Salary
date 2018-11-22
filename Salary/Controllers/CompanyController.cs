using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Salary.Common;
using Salary.Models.Company;
using Salary.Service;

namespace Salary.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly CompanyService _company = new CompanyService();

        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCompanyList([FromUri]CompanyQueryDto queryDto)
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

            var list = _company.GetCompanyList(queryDto);
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        /// <summary>
        /// 根据id获取客户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCompanyById(int id)
        {
            var list = _company.GetCompanyById(id);
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="create"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddCompany(CompanyCreateDto create)
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
           int result = _company.AddComapny(create);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "新增成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "新增失败"));
        }

        /// <summary>
        /// 更新公司
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateCompany(CompanyUpdateDto update)
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
            int result = _company.UpdateComapny(update);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "修改成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "修改失败"));
        }

        /// <summary>
        /// 移动到公海
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ToggelInPublic(List<int> ids)
        {
            int result = _company.ToggleCompany(ids,1);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "移动到公海成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "移动到公海失败"));
        }

        /// <summary>
        /// 移出公海，指派销售员
        /// </summary>
        /// <param name="toggle"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ToggleOutPublic(ToggleCompany toggle)
        {
            int result = _company.ToggleCompany(toggle);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "指派成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "指派失败"));
        }

        /// <summary>
        /// 移动到回收站
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ToggleInRecycle(List<int> ids)
        {
            int result = _company.ToggleCompany(ids,0);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "移动到回收站成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "移动到回收站失败"));
        }

        /// <summary>
        /// 客户移出回收站
        /// </summary>
        /// <param name="toggle"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ToggleOutRecycle(ToggleCompany toggle)
        {
            int result = _company.ToggleCompany(toggle);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "指派成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "指派失败"));
        }



    }
}
