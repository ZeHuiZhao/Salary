using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Salary.Common;
using Salary.Models.Activity;
using Salary.Service;

namespace Salary.Controllers
{
    public class ActivityController : BaseController
    {
        private readonly ActivityService _activity = new ActivityService();
        /// <summary>
        /// 获取获得列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetActivityList([FromUri]ActivityQueryDto dto)
        {
            var list = _activity.GetActivityList(dto);
            return Json(new OperationResult(OperationResultType.Success, "","",list));
        }

        /// <summary>
        /// 获取活动详情
        /// </summary>
        /// <param name="id">活动id</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetActivityById(int id)
        {
            var obj = _activity.GetActivityById(id);
            return Json(new OperationResult(OperationResultType.Success, "","", obj));
        }

        /// <summary>
        /// 添加活动
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddActivity(ActivityCreateDto create)
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
            int result = _activity.AddActivity(create);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "新增成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "新增失败", ""));
        }

        /// <summary>
        /// 更新活动
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateActivity(ActivityUpdateDto update)
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
            int result = _activity.UpdateActivity(update);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "修改成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "修改失败", ""));
        }

        /// <summary>
        /// 复制活动
        /// </summary>
        /// <param name="id">活动id</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CopyActivity(int id)
        {
            int result = _activity.CopyActivity(id);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "复制成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "复制失败", ""));
        }

        /// <summary>
        /// 删除一条活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteActivity(int id)
        {
            int result = _activity.DeleteActivity(id);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "删除成功"));
            }else if(result==-1)
            {
                return Json(new OperationResult(OperationResultType.Error, "活动24小时内才能删除", ""));
            }
            return Json(new OperationResult(OperationResultType.Error, "删除失败", ""));
        }

        /// <summary>
        /// 获取活动状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetActivityStatus()
        {
            List<object> list = new List<object>();
            list.Add(new { Id = 1, Name = "未开始" });
            list.Add(new { Id = 2, Name = "进行中" });
            list.Add(new { Id = 3, Name = "已结束" });
            return Json(new OperationResult(OperationResultType.Success, "","",list));
        }

        /// <summary>
        /// 获取活动素材使用情况
        /// </summary>
        /// <param name="id">活动id</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetActivityUseMaterialDetail(int id)
        {
            var obj = _activity.GetActivityUseMaterialDetail(id);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 获取活动素材列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult GetActivityMaterialList(int id)
        {
            return Ok();
        }

        /// <summary>
        /// 预览活动报名
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ShowActivityEnroll(int id)
        {
            return Ok();
        }

        /// <summary>
        /// 获取报名二维码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetEnrollQR(int id)
        {
            return Ok();
        }

        /// <summary>
        /// 获取签到二维码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetSignInQR(int id)
        {
            return Ok();
        }

        /// <summary>
        /// 通过报名（审核不通过里面操作通过报名）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult PassEnroll(int id)
        {
            return Ok();
        }

        /// <summary>
        /// 销售员审核报名（活动）
        /// </summary>
        /// <param name="id">报名id</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ReviewEnroll(int id)
        {
            return Ok();
        }

        /// <summary>
        /// 切换活动素材
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ToggleActivityMaterial()
        {
            return Ok();
        }

        /// <summary>
        /// 删除活动素材
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteActivityMaterial()
        {
            return Ok();
        }

        /// <summary>
        /// 预览活动素材
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ShowActivityMaterial()
        {
            return Ok();
        }

        /// <summary>
        /// 添加活动素材
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddActivityMaterial()
        {
            return Ok();
        }
        
        /// <summary>
        /// 获取活动素材根据素材id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetActivityMaterialById()
        {
            return Ok();
        }

        /// <summary>
        /// 更新活动素材
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateActivityMaterial()
        {
            return Ok();
        }

    }
}
