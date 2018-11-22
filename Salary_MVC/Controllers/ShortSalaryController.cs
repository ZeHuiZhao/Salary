using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Salary_MVC.Controllers
{
    public class ShortSalaryController : ZlController
    {
        Services.ShortSalaryServie shortSalaryServie = new Services.ShortSalaryServie();
        Services.FinancialUnitService financialUnitSerive = new Services.FinancialUnitService();
        Services.DepartmentService departmentService = new Services.DepartmentService();
        //Services.AttachmentService attachmentService = new Services.AttachmentService();
        //Services.ApproveLogService approveService = new Services.ApproveLogService();
        //Services.UserService userService = new Services.UserService();
        // GET: Bonus
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Approve()
        {
            return View();
        }

        /// <summary>
        /// 奖惩列表数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ActionResult GetEntity(Salary_MVC.Models.BonusQuery parameter)
        {
            List<DataModel.GZ_Department> lstDepartment = this.departmentService.GetEntityWithKeyValue();
            var dictDepartment = lstDepartment.ToDictionary(x => x.Id, x => x.Name);
            List<DataModel.GZ_FinancialUnit> lstFinancialUnit = this.financialUnitSerive.GetEntity();
            var dictFinancialUnit = lstFinancialUnit.ToDictionary(x => x.Id, x => x.Name);
            var lst = this.shortSalaryServie.GetEntity(parameter);
            var lstRT = lst.Select(x => new {
                x.Item1.Comment,
                x.Item1.Id,
                Money = x.Item1.Money,
                Category=x.Item1.Category.ToString(),
                Status = x.Item1.Status.ToString(),
                StatusValue=(int)x.Item1.Status,
                EffectedDate = x.Item1.EffectedDate.ToString("yyyy-MM"),
                //EndDate = x.Item1.EndDate.ToString("yyyy-MM-dd"),
                x.Item2.Name,
                x.Item2.Mobile,
                x.Item3?.FilePath,
                AttachmentCategory = (int?)x.Item3?.Category,
                DepartmentName = dictDepartment.ContainsKey(x.Item1.DepartmentId) ? dictDepartment[x.Item1.DepartmentId] : string.Empty,
                FinancialUnitName = dictFinancialUnit.ContainsKey(x.Item1.FinancailUnitId) ? dictFinancialUnit[x.Item1.FinancailUnitId] : string.Empty
            }).ToList();
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, lstRT));
        }

        /// <summary>
        /// 新增奖惩
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ActionResult Add(Salary_MVC.Models.ShortSalaryAdd parameter)
        {
            //if (string.IsNullOrEmpty(parameter.Comment))
            //    throw new ArgumentException("必须填写奖惩备注");
            //if (string.IsNullOrEmpty(parameter.FilePath))
            //    throw new ArgumentException("必须选择附件");
            if (parameter.Money <= 0m)
                throw new ArgumentException("金额数据格式不正确");
            if (parameter.EffectedDate.Date== new DateTime(1,1,1))
                throw new ArgumentException("必须指定月份");
            if (parameter.Category != DataModel.GZ_ShortSalary.ShortSalaryCategoryEnum.奖励 && parameter.Category != DataModel.GZ_ShortSalary.ShortSalaryCategoryEnum.赔偿)
                throw new ArgumentException("只能是奖励或者赔偿");
            if (parameter.EmployeeId == Guid.Empty)
                throw new ArgumentException("必须对奖惩指定所属的员工Id");
            var rt = this.shortSalaryServie.Add(parameter) > 0 ? Common.OperationResultType.Success : Common.OperationResultType.Error;
            var msg = rt == Common.OperationResultType.Success ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(rt, msg));
        }

        public ActionResult GetEntityById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("必须指定要奖惩Id");
            var shortSalaryWrapper = this.shortSalaryServie.GetEntityById(id);
            if (shortSalaryWrapper == null)
                throw new ArgumentException("未找到指定Id的奖惩记录");
            List<DataModel.GZ_Department> lstDepartment = this.departmentService.GetEntityWithKeyValue();
            var dictDepartment = lstDepartment.ToDictionary(x => x.Id, x => x.Name);
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, new {
                shortSalaryWrapper.Item1.Id,
                shortSalaryWrapper.Item2.Name,
                DepartmentName = dictDepartment.ContainsKey(shortSalaryWrapper.Item1.DepartmentId) ? dictDepartment[shortSalaryWrapper.Item1.DepartmentId] : string.Empty,
                shortSalaryWrapper.Item2.Mobile,
                Money = shortSalaryWrapper.Item1.Money,
                shortSalaryWrapper.Item1.Category,
                EffectedDate=shortSalaryWrapper.Item1.EffectedDate.ToString("yyyy-MM"),
                //StartDate = shortSalaryWrapper.Item1.StartDate.ToString("yyyy-MM-dd"),
                //EndDate = shortSalaryWrapper.Item1.EndDate.ToString("yyyy-MM-dd"),
                shortSalaryWrapper.Item1.Comment,
                FilePath=shortSalaryWrapper.Item3?.FilePath??string.Empty,
                FileName =shortSalaryWrapper.Item3==null?string.Empty:System.IO.Path.GetFileName(shortSalaryWrapper.Item3.FilePath).Substring("yyyy_MM_dd_HH_mm_ss".Length),
                AttachmentCategory = (int?)shortSalaryWrapper.Item3?.Category,

            }));

        }

        public ActionResult Edit(Models.ShortSalaryEdit parameter)
        {
            //if (string.IsNullOrEmpty(parameter.Comment))
            //    throw new ArgumentException("必须填写奖惩说明");
            //if (string.IsNullOrEmpty(parameter.FilePath))
            //    throw new ArgumentException("必须选择附件");
            if (parameter.Id == Guid.Empty)
                throw new ArgumentException("必须指定被修改奖惩的Id");
            if (parameter.Money <= 0m)
                throw new ArgumentException("金额数据格式不正确");
            if (parameter.EffectedDate.Date == new DateTime(1, 1, 1))
                throw new ArgumentException("必须指定月份");
            var rt = this.shortSalaryServie.Edit(parameter) > 0 ? Common.OperationResultType.Success : Common.OperationResultType.Error;
            var msg = rt == Common.OperationResultType.Success ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(rt, msg));
        }

        /// <summary>
        /// 删除制定Id的奖惩记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("必须指定要删除的奖惩记录的Id");
            var rt = this.shortSalaryServie.DeleteById(id) > 0 ? Common.OperationResultType.Success : Common.OperationResultType.Error;
            var msg = rt == Common.OperationResultType.Success ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(rt, msg));
        }


        public ActionResult ApproveByHR(Models.StartApproveBatch parameter)
        {
            if (parameter.TargetIds == null || parameter.TargetIds.Length < 1)
                throw new ArgumentException("必须指定要审核的奖惩记录的Id");
            var rt = this.shortSalaryServie.ApproveByHR(parameter) > 0 ? Common.OperationResultType.Success : Common.OperationResultType.Error;
            var msg = rt == Common.OperationResultType.Success ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(rt, msg));
        }


        public ActionResult GetEntityByFinance(Models.BonusQueryByApprove parameter)
        {
            if (parameter.TabIndex != Models.TabEnum.待审核 && parameter.TabIndex != Models.TabEnum.已审核)
                throw new ArgumentException("只能查询待审核 或者 已审核的数据");
            List<DataModel.GZ_Department> lstDepartment = this.departmentService.GetEntityWithKeyValue();
            var dictDepartment = lstDepartment.ToDictionary(x => x.Id, x => x.Name);
            List<DataModel.GZ_FinancialUnit> lstFinancialUnit = this.financialUnitSerive.GetEntity();
            var dictFinancialUnit = lstFinancialUnit.ToDictionary(x => x.Id, x => x.Name);
            parameter.Name = parameter.TabIndex == Models.TabEnum.待审核 ? string.Empty : parameter.Name;
            var lst = this.shortSalaryServie.GetEntityByFinance(parameter);
            var lstRT = lst.Select(x => new {
                x.Item1.Comment,
                x.Item1.Id,
                Money = x.Item1.Money,
                Category = x.Item1.Category.ToString(),
                Status = x.Item1.Status.ToString(),
                StatusValue=(int)x.Item1.Status,
                EffectedDate = x.Item1.EffectedDate.ToString("yyyy-MM"),
                //EndDate = x.Item1.EndDate.ToString("yyyy-MM-dd"),
                x.Item2.Name,
                x.Item2.Mobile,
                x.Item3?.FilePath,
                AttachmentCategory = (int?)x.Item3?.Category,
                DepartmentName = dictDepartment.ContainsKey(x.Item1.DepartmentId) ? dictDepartment[x.Item1.DepartmentId] : string.Empty,
                FinancialUnitName = dictFinancialUnit.ContainsKey(x.Item1.FinancailUnitId) ? dictFinancialUnit[x.Item1.FinancailUnitId] : string.Empty
            }).ToList();
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, lstRT));
        }

        public ActionResult ApproveByFinance(Models.ApproveBatchInput parameter)
        {
            if (parameter.TargetIds == null || parameter.TargetIds.Length < 1)
                throw new ArgumentException("必须指定要审核的奖惩记录的Id");
            if (parameter.Handler != DataModel.GZ_ApproveLog.ApproveLogCategory.Through && parameter.Handler != DataModel.GZ_ApproveLog.ApproveLogCategory.NotThrough)
                throw new ArgumentException("审核操作只能是通过或者不通过");
            var rt = this.shortSalaryServie.ApproveByFinance(parameter) > 0 ? Common.OperationResultType.Success : Common.OperationResultType.Error;
            var msg = rt == Common.OperationResultType.Success ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(rt, msg));
        }
    }
}