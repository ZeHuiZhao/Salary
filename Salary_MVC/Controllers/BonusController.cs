using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Salary_MVC.Controllers
{
    public class BonusController : ZlController
    {
        Services.BonusServie bonusServie = new Services.BonusServie();
        Services.FinancialUnitService financialUnitSerive = new Services.FinancialUnitService();
        Services.DepartmentService departmentService = new Services.DepartmentService();
        //Services.AttachmentService attachmentService = new Services.AttachmentService();
        //Services.ApproveLogService approveService = new Services.ApproveLogService();
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
        /// 津贴列表数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ActionResult GetEntity(Salary_MVC.Models.BonusQuery parameter)
        {
            if (!this.ModelState.IsValid)
                throw new Common.ModelBindingException(this.ModelState.Values);
            List<DataModel.GZ_Department> lstDepartment = this.departmentService.GetEntityWithKeyValue();
            var dictDepartment = lstDepartment.ToDictionary(x => x.Id, x => x.Name);
            List<DataModel.GZ_FinancialUnit> lstFinancialUnit = this.financialUnitSerive.GetEntity();
            var dictFinancialUnit = lstFinancialUnit.ToDictionary(x => x.Id, x => x.Name);
            var lst = this.bonusServie.GetEntity(parameter);
            var lstRT= lst.Select(x => new {
                x.Item1.Comment,
                x.Item1.Id,
                Money=x.Item1.Money_pwd,
                Status=x.Item1.Status.ToString(),
                StatusValue=(int)x.Item1.Status,
                StartDate =x.Item1.StartDate.ToString("yyyy-MM-dd"),
                EndDate = x.Item1.EndDate.ToString("yyyy-MM-dd"),
                x.Item2.Name,
                x.Item2.Mobile,
                x.Item3?.FilePath,
                AttachmentCategory=(Nullable<int>)(x.Item3?.Category),
                DepartmentName = dictDepartment.ContainsKey(x.Item1.DepartmentId) ? dictDepartment[x.Item1.DepartmentId] : string.Empty,
                FinancialUnitName = dictFinancialUnit.ContainsKey(x.Item1.FinancailUnitId) ? dictFinancialUnit[x.Item1.FinancailUnitId] : string.Empty
            }).ToList();
            return this.Json(new Common.OperationResult( Common.OperationResultType.Success,string.Empty,lstRT));
        }

        /// <summary>
        /// 新增津贴
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ActionResult Add(Salary_MVC.Models.BonusAdd parameter)
        {
            //if (string.IsNullOrEmpty(parameter.Comment))
            //    throw new ArgumentException("必须填写津贴备注");
            //if (string.IsNullOrEmpty(parameter.Attachment))
            //    throw new ArgumentException("必须选择附件");
            if (parameter.Money <= 0m)
                throw new ArgumentException("金额数据格式不正确");
            if(parameter.StartDate.Date==new DateTime(1,1,1))
                throw new ArgumentException("必须指定开始日期");
            if (parameter.EndDate.Date == new DateTime(1, 1, 1))
                throw new ArgumentException("必须指定结束日期");
            if (parameter.StartDate >= parameter.EndDate)
                throw new ArgumentException("津贴开始日期必须小于津贴结束日期");
            if (parameter.EmployeeId==Guid.Empty)
                throw new ArgumentException("必须对津贴指定所属的员工Id");
            var rt= this.bonusServie.Add(parameter)>0?Common.OperationResultType.Success: Common.OperationResultType.Error;
            var msg = rt == Common.OperationResultType.Success ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(rt,msg));
        }

        public ActionResult GetEntityById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("必须指定要津贴Id");
            var bonusWrapper=  this.bonusServie.GetEntityById(id);
            if (bonusWrapper == null)
                throw new ArgumentException("未找到指定Id的津贴记录");
            List<DataModel.GZ_Department> lstDepartment = this.departmentService.GetEntityWithKeyValue();
            var dictDepartment = lstDepartment.ToDictionary(x => x.Id, x => x.Name);
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, new {
                bonusWrapper.Item1.Id,
                bonusWrapper.Item2.Name,
                DepartmentName = dictDepartment.ContainsKey(bonusWrapper.Item1.DepartmentId) ? dictDepartment[bonusWrapper.Item1.DepartmentId] : string.Empty,
                bonusWrapper.Item2.Mobile,
                Money = bonusWrapper.Item1.Money_pwd,
                StartDate = bonusWrapper.Item1.StartDate.ToString("yyyy-MM-dd"),
                EndDate = bonusWrapper.Item1.EndDate.ToString("yyyy-MM-dd"),
                bonusWrapper.Item1.Comment,
                FilePath=bonusWrapper.Item3?.FilePath??string.Empty,
                FileName= bonusWrapper.Item3==null?string.Empty:System.IO.Path.GetFileName(bonusWrapper.Item3.FilePath).Substring("yyyy_MM_dd_HH_mm_ss".Length),
                AttachmentCategory = (Nullable<int>)bonusWrapper.Item3?.Category,
            }));

        }

        public ActionResult Edit(Models.BonusEdit parameter)
        {
            //if (string.IsNullOrEmpty(parameter.Comment))
            //    throw new ArgumentException("必须填写津贴备注");
            //if (string.IsNullOrEmpty(parameter.FilePath))
            //    throw new ArgumentException("必须选择附件");
            if (parameter.Id==Guid.Empty)
                throw new ArgumentException("必须指定被修改津贴的Id");
            if (parameter.Money<=0m)
                throw new ArgumentException("金额数据格式不正确");
            if (parameter.StartDate.Date == new DateTime(1, 1, 1))
                throw new ArgumentException("必须指定开始日期");
            if (parameter.EndDate.Date == new DateTime(1, 1, 1))
                throw new ArgumentException("必须指定结束日期");
            if (parameter.StartDate>=parameter.EndDate)
                throw new ArgumentException("津贴开始日期必须小于津贴结束日期");
            var rt= this.bonusServie.Edit(parameter)>0?Common.OperationResultType.Success: Common.OperationResultType.Error;
            var msg = rt == Common.OperationResultType.Success ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(rt,msg));
        }

        /// <summary>
        /// 删除制定Id的津贴记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("必须指定要删除的津贴记录的Id");
            var rt= this.bonusServie.DeleteById(id) >0?Common.OperationResultType.Success: Common.OperationResultType.Error;
            var msg = rt == Common.OperationResultType.Success ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(rt, msg));
        }



        public ActionResult ApproveByHR(Models.StartApproveBatch parameter)
        {
            if (parameter.TargetIds == null || parameter.TargetIds.Length < 1)
                throw new ArgumentException("必须指定要审核的津贴记录的Id");
            var rt= this.bonusServie.ApproveByHR(parameter)>0?Common.OperationResultType.Success: Common.OperationResultType.Error;
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
            var lst = this.bonusServie.GetEntityByFinance(parameter);
            var lstRT = lst.Select(x => new {
                x.Item1.Comment,
                x.Item1.Id,
                Money = x.Item1.Money_pwd,
                Status = x.Item1.Status.ToString(),
                StatusValue=(int)x.Item1.Status,
                StartDate = x.Item1.StartDate.ToString("yyyy-MM-dd"),
                EndDate = x.Item1.EndDate.ToString("yyyy-MM-dd"),
                x.Item2.Name,
                x.Item2.Mobile,
                x.Item3?.FilePath,
                AttachmentCategory = (int?)x.Item3?.Category,
                DepartmentName = dictDepartment.ContainsKey(x.Item1.DepartmentId) ? dictDepartment[x.Item1.DepartmentId] : string.Empty,
                FinancialUnitName = dictFinancialUnit.ContainsKey(x.Item1.FinancailUnitId ) ? dictFinancialUnit[x.Item1.FinancailUnitId] : string.Empty
            }).ToList();
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, lstRT));
        }

        public ActionResult ApproveByFinance(Models.ApproveBatchInput parameter)
        {
            if (parameter.TargetIds == null || parameter.TargetIds.Length < 1)
                throw new ArgumentException("必须指定要审核的津贴记录的Id");
            if(parameter.Handler!= DataModel.GZ_ApproveLog.ApproveLogCategory.Through&&    parameter.Handler!= DataModel.GZ_ApproveLog.ApproveLogCategory.NotThrough)
                throw new ArgumentException("审核操作只能是通过或者不通过");
            var rt = this.bonusServie.ApproveByFinance(parameter) > 0 ? Common.OperationResultType.Success : Common.OperationResultType.Error;
            var msg = rt == Common.OperationResultType.Success ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(rt, msg));
        }
    }
}