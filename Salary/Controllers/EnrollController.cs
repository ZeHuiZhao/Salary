using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Salary.Common;
using Salary.Models.Enroll;
using Salary.Service;

namespace Salary.Controllers
{
    public class EnrollController : BaseController
    {
        private readonly EnrollService _enroll = new EnrollService();
        /// <summary>
        /// 获取报名列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IHttpActionResult GetEnrollList([FromUri]QueryEnrollDto query)
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
            var list = _enroll.GetEnrollList(query);
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        /// <summary>
        /// 添加报名
        /// </summary>
        /// <param name="create"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddEnroll(CreateEnrollDto create)
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
            int result = _enroll.AddEnroll(create);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "报名成功,等待邀约"));
            }
            return Json(new OperationResult(OperationResultType.Error, "报名失败", ""));
        }

        /// <summary>
        /// 获取课程时间和课程类别封面
        /// </summary>
        /// <param name="MaterialType"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetClassTime([FromUri]int MaterialType)
        {
            var obj = _enroll.GetClassTime(MaterialType);
            if (obj != null)
            {
                return Json(new OperationResult(OperationResultType.Success, "", "", obj));
            }
            return Json(new OperationResult(OperationResultType.Error, ""));
        }


        /// <summary>
        /// 退回重审
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Reapproval(AuditModel obj)
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
            int result = _enroll.Reapproval(obj);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "退回成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "退回失败"));
        }

        /// <summary>
        /// 销售员审核
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ApprovalEnroll(AuditEnrollDto obj)
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
            int result = _enroll.ApprovalEnroll(obj);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "操作成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "操作失败"));
        }

        /// <summary>
        /// 更新报名信息
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public IHttpActionResult UpdateEnroll(UpdateEnrollDto update)
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
            int result = _enroll.UpdateEnroll(update);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "修改成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "修改失败"));
        }

        /// <summary>
        /// 获得单条报名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetEnrollDetail(int id)
        {
            var obj = _enroll.GetEnrollById(id);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }
        /// <summary>
        /// 获取报名结果
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetEnrollResultList([FromUri]QueryWechatEnrollResult query)
        {
            if (CheckWechatUser(query.OpenId)) return Json(new OperationResult(OperationResultType.PurviewLack, "无权限"));
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
            var obj = _enroll.GetEnrollResultList(query);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 微信端获取参课列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetEnrollParticipateList([FromUri]QueryWechatEnrollResult query)
        {
            if (CheckWechatUser(query.OpenId)) return Json(new OperationResult(OperationResultType.PurviewLack, "无权限"));
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
            var obj = _enroll.GetEnrollParticipateList(query);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 微信获取审核结果
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetEnrollAuditResult([FromUri]QueryWechatEnrollResult query)
        {
            if (CheckWechatUser(query.OpenId)) return Json(new OperationResult(OperationResultType.PurviewLack, "无权限"));
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
            var obj = _enroll.GetEnrollAuditResult(query);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }
    }
}
