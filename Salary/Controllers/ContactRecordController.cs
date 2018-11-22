using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Salary.Common;
using Salary.Models.ComapnyRecord;
using Salary.Models.CompanyRecord;
using Salary.Service;

namespace Salary.Controllers
{
    public class ContactRecordController : BaseController
    {
        private readonly ContactRecordService _contactRecord = new ContactRecordService();

        /// <summary>
        /// 添加联系记录
        /// </summary>
        /// <param name="create"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddComapnyRecord(CompanyRecordCreateDto create)
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

            int result = _contactRecord.AddComapnyRecord(create);
            return Json(new OperationResult(OperationResultType.Success, "", result));
        }

        /// <summary>
        /// 更新联系记录
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateComapnyRecord(CompanyRecordUpdateDto update)
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

            int result = _contactRecord.UpdateComapnyRecord(update);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "修改成功"));
            }
            if (result == -1)
            {
                return Json(new OperationResult(OperationResultType.Error, "24小时内才能删除"));
            }
            return Json(new OperationResult(OperationResultType.Error, "修改失败"));
        }

        /// <summary>
        /// 获取联系记录列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCompanyRecordList([FromUri]CompanyRecordQueryDto query)
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

            var list = _contactRecord.GetCompanyRecordList(query);
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        /// <summary>
        /// 删除一条联系记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteCompanyRecord(int id)
        {
            int result = _contactRecord.DeleteCompanyRecord(id);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "删除成功"));
            }
            if (result == -1)
            {
                return Json(new OperationResult(OperationResultType.Error, "24小时内才能删除"));
            }
            return Json(new OperationResult(OperationResultType.Error, "删除失败"));
        }

        /// <summary>
        /// 根据id获取联系记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCompanyRecordById(int id)
        {
            var list = _contactRecord.GetCompanyRecordById(id);
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }
    }
}
