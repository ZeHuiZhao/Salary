using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Salary.Common;
using Salary.Models.MaterialType;
using Salary.Service;

namespace Salary.Controllers
{
    public class MaterialTypeController : BaseController
    {
        private readonly MaterialTypeService _materialType = new MaterialTypeService();

        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetZLMaterialTypeList([FromUri]QueryMaterialType query)
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
            var list = _materialType.GetZLMaterialTypeList(query);
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        /// <summary>
        /// 微信公众号获取素材类别
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetMaterialType([FromUri]QueryWechatMaterialTypeDto dto)
        {
            if(CheckWechatUser(dto.OpenId))return Json(new OperationResult(OperationResultType.PurviewLack, "无权限"));
            var list = _materialType.GetMaterialListByWechat(dto);
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }
        /// <summary>
        /// 获取类别列表用于添加素材
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        public IHttpActionResult GetMaterialTypeList()
        {
            var list = _materialType.GetMaterialTypeList();
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        /// <summary>
        /// 获取单条素材类别
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetOneMaterialTypeById(int id)
        {
            var list = _materialType.GetOneMaterialTypeById(id);
            return Json(new OperationResult(OperationResultType.Success, "", list));
        }

        /// <summary>
        /// 添加素材类别
        /// </summary>
        /// <param name="create"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddMaterialType(CreateMaterialTypeDto create)
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

            var a = _materialType.AddMaterialType(create);
            if (a == 1)
            {
                return Json(new OperationResult(OperationResultType.Success, "", "", 1));
            }
            return Json(new OperationResult(OperationResultType.Error, "添加失败"));
        }

        /// <summary>
        /// 更新素材类别
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateMaterialType(UpdateMaterialTypeDto obj)
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
            var a = _materialType.UpdateMaterialType(obj);
            if (a == 1)
            {
                return Json(new OperationResult(OperationResultType.Success, "", "", 1));
            }
            return Json(new OperationResult(OperationResultType.Error, "修改失败"));
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ToggleStatus(int id)
        {
            int result = _materialType.ToggleStauts(id);
            if (result > 0)
            {
                return Json(new OperationResult(OperationResultType.Success, "操作成功"));
            }
            return Json(new OperationResult(OperationResultType.Error, "操作失败"));
        }

        /// <summary>
        /// 预览报名页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetQRcodeUrl(int id)
        {
            string result = _materialType.GetQRcodeUrl(id);
            if (!string.IsNullOrEmpty(result))
            {
                return Json(new OperationResult(OperationResultType.Success, result));
            }
            return Json(new OperationResult(OperationResultType.Error, "操作失败"));
        }

        

    }
}
