using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Salary_MVC.Controllers
{
    public class EmployeeSalaryController : ZlController
    {
        // GET: EmployeeSalary
        Services.EmployeeSalaryService employeeSalaryServie = new Services.EmployeeSalaryService();
        Services.FinancialUnitService financialUnitSerive = new Services.FinancialUnitService();
        Services.DepartmentService departmentService = new Services.DepartmentService();
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


        public class DateWrapper : IComparable
        {
             DateTime effected;
            DateTime created;
            public DateWrapper(DateTime effected,DateTime created)
            {
                this.effected = effected;
                this.created = created;
            }
            public int CompareTo(object obj)
            {
                var value= obj as DateWrapper;
                var rt = (this.effected.Date - value.effected.Date).Days;
                if (rt != 0)
                    return rt;
                var rt2 = (this.created - value.created).TotalMilliseconds;
                return rt2 > 0 ? 1 : -1;
            }
        }
        /// <summary>
        /// 调薪列表数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ActionResult GetEntity(Salary_MVC.Models.BonusQuery parameter)
        {
            List<DataModel.GZ_Department> lstDepartment = this.departmentService.GetEntityWithKeyValue();
            var dictDepartment = lstDepartment.ToDictionary(x => x.Id, x => x.Name);
            List<DataModel.GZ_FinancialUnit> lstFinancialUnit = this.financialUnitSerive.GetEntity();
            var dictFinancialUnit = lstFinancialUnit.ToDictionary(x => x.Id, x => x.Name);
            var lst = this.employeeSalaryServie.GetEntity(parameter);
            //处理调整前
            var dictSalary= lst.Select(x => x.Item1).GroupBy(x => x.EmployeeId)
                .ToDictionary(gp => gp.Key, gp => gp.OrderBy(x => new DateWrapper(x.EffectedDate, x.CreateDate.Value)).ToList());
            var lstRT = lst.Select(x => new {
                x.Item1.Comment,
                x.Item1.Id,
                OriginalMoney= dictSalary[x.Item1.EmployeeId].TakeWhile(a=>a.Id!=x.Item1.Id).Where(a=>a.Status== Enum.ApproveStatus.财务同意).LastOrDefault()?.Money??default(decimal),
                Money = x.Item1.Money,
                Status = x.Item1.Status.ToString(),
                StatusValue=(int)x.Item1.Status,
                EffectedDate = x.Item1.EffectedDate.ToString("yyyy-MM-dd"),
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
        /// 新增调薪
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ActionResult Add(Salary_MVC.Models.EmployeeSalaryAdd parameter)
        {
            //if (string.IsNullOrEmpty(parameter.Comment))
            //    throw new ArgumentException("必须填写调薪备注");
            //if (string.IsNullOrEmpty(parameter.FilePath))
            //    throw new ArgumentException("必须选择附件");
            if (parameter.Money <= 0m)
                throw new ArgumentException("金额数据格式不正确");
            if (parameter.EffectedDate.Date == new DateTime(1, 1, 1))
                throw new ArgumentException("必须指定生效日期");
            if (parameter.EmployeeId == Guid.Empty)
                throw new ArgumentException("必须对调薪指定所属的员工Id");
            var rt = this.employeeSalaryServie.Add(parameter) > 0 ? Common.OperationResultType.Success : Common.OperationResultType.Error;
            var msg = rt == Common.OperationResultType.Success ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(rt, msg));
        }

        public ActionResult GetEntityById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("必须指定要调薪Id");
            var shortSalaryWrapper = this.employeeSalaryServie.GetEntityById(id);
            if (shortSalaryWrapper == null)
                throw new ArgumentException("未找到指定Id的调薪记录");
            List<DataModel.GZ_Department> lstDepartment = this.departmentService.GetEntityWithKeyValue();
            var dictDepartment = lstDepartment.ToDictionary(x => x.Id, x => x.Name);
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, new {
                shortSalaryWrapper.Item1.Id,
                shortSalaryWrapper.Item2.Name,
                DepartmentName = dictDepartment.ContainsKey(shortSalaryWrapper.Item1.DepartmentId) ? dictDepartment[shortSalaryWrapper.Item1.DepartmentId] : string.Empty,
                shortSalaryWrapper.Item2.Mobile,
                Money = shortSalaryWrapper.Item1.Money,
                EffectedDate = shortSalaryWrapper.Item1.EffectedDate.ToString("yyyy-MM-dd"),
                //StartDate = shortSalaryWrapper.Item1.StartDate.ToString("yyyy-MM-dd"),
                //EndDate = shortSalaryWrapper.Item1.EndDate.ToString("yyyy-MM-dd"),
                shortSalaryWrapper.Item1.Comment,
                FilePath=shortSalaryWrapper.Item3?.FilePath??string.Empty,
                FileName = shortSalaryWrapper.Item3==null?string.Empty:System.IO.Path.GetFileName(shortSalaryWrapper.Item3.FilePath).Substring("yyyy_MM_dd_HH_mm_ss".Length),
                AttachmentCategory = (int?)shortSalaryWrapper.Item3?.Category,
            }));

        }

        public ActionResult Edit(Models.EmployeeSalaryEdit parameter)
        {
            //if (string.IsNullOrEmpty(parameter.Comment))
            //    throw new ArgumentException("必须填写调薪说明");
            //if (string.IsNullOrEmpty(parameter.FilePath))
            //    throw new ArgumentException("必须选择附件");
            if (parameter.Id == Guid.Empty)
                throw new ArgumentException("必须指定被修改调薪的Id");
            if (parameter.Money <= 0m)
                throw new ArgumentException("金额数据格式不正确");
            if (parameter.EffectedDate.Date == new DateTime(1, 1, 1))
                throw new ArgumentException("必须指定生效日期");
            var rt = this.employeeSalaryServie.Edit(parameter) > 0 ? Common.OperationResultType.Success : Common.OperationResultType.Error;
            var msg = rt == Common.OperationResultType.Success ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(rt, msg));
        }

        /// <summary>
        /// 删除制定Id的调薪记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("必须指定要删除的调薪记录的Id");
            var rt = this.employeeSalaryServie.DeleteById(id) > 0 ? Common.OperationResultType.Success : Common.OperationResultType.Error;
            var msg = rt == Common.OperationResultType.Success ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(rt, msg));
        }


        public ActionResult ApproveByHR(Models.StartApproveBatch parameter)
        {
            if (parameter.TargetIds == null || parameter.TargetIds.Length < 1)
                throw new ArgumentException("必须指定要审核的调薪记录的Id");
            var rt = this.employeeSalaryServie.ApproveByHR(parameter) > 0 ? Common.OperationResultType.Success : Common.OperationResultType.Error;
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
            var lst = this.employeeSalaryServie.GetEntityByFinance(parameter);
            //处理调整前
            var dictSalary = lst.Select(x => x.Item1).GroupBy(x => x.EmployeeId)
                .ToDictionary(gp => gp.Key, gp => gp.OrderBy(x => new DateWrapper(x.EffectedDate, x.CreateDate.Value)).ToList());
            var lstRT = lst.Select(x => new {
                x.Item1.Comment,
                x.Item1.Id,
                OriginalMoney = dictSalary[x.Item1.EmployeeId].TakeWhile(a => a.Id != x.Item1.Id).Where(a => a.Status == Enum.ApproveStatus.财务同意).LastOrDefault()?.Money ?? default(decimal),
                Money = x.Item1.Money,
                Status = x.Item1.Status.ToString(),
                StatusValue=(int)x.Item1.Status,
                EffectedDate = x.Item1.EffectedDate.ToString("yyyy-MM-dd"),
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
                throw new ArgumentException("必须指定要审核的调薪记录的Id");
            if (parameter.Handler != DataModel.GZ_ApproveLog.ApproveLogCategory.Through && parameter.Handler != DataModel.GZ_ApproveLog.ApproveLogCategory.NotThrough)
                throw new ArgumentException("审核操作只能是通过或者不通过");
            var rt = this.employeeSalaryServie.ApproveByFinance(parameter) > 0 ? Common.OperationResultType.Success : Common.OperationResultType.Error;
            var msg = rt == Common.OperationResultType.Success ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(rt, msg));
        }
    }
}