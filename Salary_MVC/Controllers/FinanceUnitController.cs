using Salary_MVC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Salary_MVC.Controllers
{
    public class FinanceUnitController : ZlController
    {
        Services.FinancialUnitService financialUnitSerive = new Services.FinancialUnitService();
        
        Services.UserService userService = new Services.UserService();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Approve()
        {
            return View();
        }

        /// <summary>
        /// 财务核算单位列表数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ActionResult GetEntity()
        {
            var lst = this.financialUnitSerive.GetEntity();
            var lstUser= this.userService.GetAllUser();
            var dictUser= lstUser.ToDictionary(x => x.Id, x => x.Name);
            var lstRT = lst.Select(x => new {
                x.Id,
                x.Name,
                CreateDate = x.CreateDate.Value.ToString("yyyy-MM-dd HH:mm"),
                CreateUser = dictUser.ContainsKey(x.CreateUser.Value) ? dictUser[x.CreateUser.Value] : "admin",
                Status=x.Status.ToString()
            }).ToList();
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, lstRT));
        }

        /// <summary>
        /// 新增财务核算单位
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ActionResult Add(Salary_MVC.Models.FinanceUnitAdd parameter)
        {
            if (string.IsNullOrEmpty(parameter.Name))
                throw new ArgumentException("必须指定财务核算单位的名称");
            var rt = this.financialUnitSerive.Add(parameter) > 0 ? Common.OperationResultType.Success : Common.OperationResultType.Error;
            var msg = rt == Common.OperationResultType.Success ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(rt, msg));
        }

        public ActionResult GetEntityById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("必须指定要财务核算单位Id");
            var unit = this.financialUnitSerive.GetEntityById(id);
            if (unit == null)
                throw new ArgumentException("未找到指定Id的财务核算单位");
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, unit));
        }

        public ActionResult Edit(Models.FinanceUnitEdit parameter)
        {
            if (string.IsNullOrEmpty(parameter.Name))
                throw new ArgumentException("必须指定财务核算单位的名称");
            if (parameter.Id == Guid.Empty)
                throw new ArgumentException("必须指定被修改财务核算单位的Id");
            var rt = this.financialUnitSerive.Edit(parameter) > 0 ? Common.OperationResultType.Success : Common.OperationResultType.Error;
            var msg = rt == Common.OperationResultType.Success ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(rt, msg));
        }

        /// <summary>
        /// 删除制定Id的财务核算单位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("必须指定要删除的财务核算单位的Id");
            var rt = this.financialUnitSerive.DeleteById(id) > 0 ? Common.OperationResultType.Success : Common.OperationResultType.Error;
            var msg = rt == Common.OperationResultType.Success ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(rt, msg));
        }

        /// <summary>
        /// 作废财务核算单位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ForbidById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("必须指定要删除的财务核算单位的Id");
            var rt = this.financialUnitSerive.ForbidById(id) > 0 ? Common.OperationResultType.Success : Common.OperationResultType.Error;

            var msg = rt == Common.OperationResultType.Success ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(rt, msg));
        }

        [HttpGet]
        public ActionResult GetFinanceUnitList()
        {
            var obj = financialUnitSerive.GetFinanceUnitList();
            return Json(new OperationResult(OperationResultType.Success, "", "", obj), JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetEntityWithKeyValue()
        {
            var lst= financialUnitSerive.GetEntityWithKeyValue();
            var lstTmp= lst.Select(x => new { x.Id, x.Name }).ToList();
            return this.Json(new Common.OperationResult(OperationResultType.Success,string.Empty,lstTmp));
        }
    }
}