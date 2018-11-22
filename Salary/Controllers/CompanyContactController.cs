using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Salary.Common;
using Salary.Models.CompanyContact;
using Salary.Service;

namespace Salary.Controllers
{
    public class CompanyContactController : BaseController
    {
        private readonly CompanyContactService _companyContact = new CompanyContactService();

        /// <summary>
        /// 获取联系人列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCompanyContactList([FromUri]CompanyContactQueryDto queryDto)
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

            var list = _companyContact.GetCompanyContactList(queryDto);
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        /// <summary>
        /// 获取联系人列表（不分页）
        /// </summary>
        /// <param name="id">客户表id</param>
        /// <returns></returns>
        public IHttpActionResult GetCompanyContactListNotPager(int id)
        {
            var list = _companyContact.GetCompanyContactListNotPager(id);
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        /// <summary>
        /// 根据id查询联系人
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCompanyContactById(int id)
        {
            var model = _companyContact.GetCompanyContactById(id);
            return Json(new OperationResult(OperationResultType.Success, "", model));
        }

        /// <summary>
        /// 添加客户联系人
        /// </summary>
        /// <param name="create"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddCompanyContact(CompanyContactCreateDto create)
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
            int result = _companyContact.AddCompanyContact(create);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "新增成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "新增失败"));

        }

        /// <summary>
        /// 更新联系人
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateCompanyContact(CompanyContactUpdateDto update)
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
            int result = _companyContact.UpdateCompanyContact(update);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "修改成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "修改失败"));
        }

        /// <summary>
        /// 设置联系人为第一联系人
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SetFirstContact(int id)
        {
            int result = _companyContact.SetFirstContact(id);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "设置成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "设置失败"));
        }

        /// <summary>
        /// 根据id删除联系人
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteCompanyContact(int id)
        {
            int result = _companyContact.DeleteCompanyContact(id);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "删除成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "删除失败"));
        }

    }
}
