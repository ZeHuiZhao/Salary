using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Salary.Common;
using Salary.Models.Material;
using Salary.Service;

namespace Salary.Controllers
{
    public class MaterialController : BaseController
    {
        private readonly MaterialService _material = new MaterialService();
        /// <summary>
        /// 获取中力渠道列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetMaterialListByZL([FromUri]QueryMaterialDto query)
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
            var list = _material.GetMaterialList(query);
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        /// <summary>
        /// 获取素材列表(微信端)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetMaterialList([FromUri]QueryWechatMaterialDto dto)
        {
            var list = _material.GetMaterialListByWechat(dto);
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        /// <summary>
        /// 中力添加素材
        /// </summary>
        /// <param name="create"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddMaterialByZL(CreateMaterialDto create)
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
            int result = _material.AddMaterial(create);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "新增成功"));
            }
            return Json(new OperationResult(OperationResultType.Success, "新增失败"));
        }


        /// <summary>
        /// 更新素材
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateMaterialByZL(UpdateMaterialDto update)
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
            int result = _material.UpdateMaterial(update);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "修改成功"));
            }
            return Json(new OperationResult(OperationResultType.Success, "修改失败"));
        }

        /// <summary>
        /// 获取单条素材
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetMaterialById(int id)
        {
            var obj = _material.GetMaterialById(id);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 微信查询素材详情根据id和openid
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetWechatMaterialById([FromUri]QueryMaterialDetail dto)
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
            var obj = _material.GetWechatMaterialById(dto);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 切换素材状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ToggleStatusByZL(int id)
        {
            int result = _material.ToggleStatusByZL(id);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "修改成功"));
            }
            return Json(new OperationResult(OperationResultType.Success, "修改失败"));
        }


        /// <summary>
        /// 生成预览二维码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetQRcodeUrlByZL(int id)
        {
            var result = _material.GetQRcodeUrlByZL(id);
            if (!string.IsNullOrEmpty(result))
            {
                return Json(new OperationResult(OperationResultType.Success, "", "", result));
            }
            return Json(new OperationResult(OperationResultType.Success, "生成二维码失败"));
        }


        /// <summary>
        /// 删除渠道素材
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteChannelMaterial(int id)
        {
            int result = _material.DeleteChannelMaterial(id);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "删除成功"));
            }else if (result == -1)
            {
                return Json(new OperationResult(OperationResultType.Error, "素材24小时内才能删除"));
            }
            return Json(new OperationResult(OperationResultType.Error, "删除失败"));
        }

        /// <summary>
        /// 素材分享
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddMaterialRelay(MaterialShareModel dto)
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
            int result = _material.AddMaterialRelay(dto);
            if (result == 1)
            {
                return Json(new OperationResult(OperationResultType.Success, "", "", 1));
            }
            return Json(new OperationResult(OperationResultType.Error, "分享失败"));
        }


        /// <summary>
        /// 添加素材浏览
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddMaterialBrowse(MaterialBrowswModel dto)
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
            int result = _material.AddMaterialBrowse(dto);
            if (result == 1)
            {
                return Json(new OperationResult(OperationResultType.Success, "", "", 1));
            }
            return Json(new OperationResult(OperationResultType.Error, "", "", ""));
        }

        /// <summary>
        /// 获得浏览数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetBrowseData([FromUri]BrowseDataDto query)
        {
            if (CheckWechatUser(query.OpenId)) return Json(new OperationResult(OperationResultType.PurviewLack, "无权限"));
            var obj = _material.GetBrowseData(query);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }
    }
}
