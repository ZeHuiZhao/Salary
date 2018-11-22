using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Salary_MVC.Controllers
{
    public class MonthlySalaryController : ZlController
    {
        Services.MonthlySalaryService monthlySalaryService = new Services.MonthlySalaryService();
        Services.FinancialUnitService financialUnitSerive = new Services.FinancialUnitService();
        Services.DepartmentService departmentService = new Services.DepartmentService();
        Services.ApproveLogService approveLogService = new Services.ApproveLogService();
        Services.UserService userService = new Services.UserService();
        public ActionResult Index(string month)
        {
            DateTime tmp;
            if (!DateTime.TryParse(month, out tmp))
                tmp = DateTime.Now.AddMonths(-1);
            //var  this.monthlySalaryService.GetMasterByMonth(tmp);
            this.ViewData["masterMonth"] = tmp;
            return View();
        }

        public ActionResult Board(string month)
        {
            DateTime tmp;
            if (!DateTime.TryParse(month,out tmp))
                tmp= DateTime.Now.AddMonths(-1);
            this.ViewData["masterMonth"] = tmp;
            return View();
        }


        public ActionResult GetMasterByMonth(string month)
        {
            DateTime tmp;
            if (string.IsNullOrEmpty(month))
                throw new ArgumentException("必须指定综合工资的月份");
            if (!DateTime.TryParse(month, out tmp))
                throw new ArgumentException("综合工资的月份的格式错误，示例2018-01");
            var master= this.monthlySalaryService.GetMasterByMonth(tmp);
            //master = master ?? new DataModel.GZ_MonthlySalaryMaster();
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, master));
        }

        /// <summary>
        /// 检测是否就绪
        /// 考勤、员工基本信息、津贴、奖惩、调薪
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public ActionResult Validate(string month)
        {
            DateTime tmp;
            if (string.IsNullOrEmpty(month))
                throw new ArgumentException("必须指定综合工资的月份");
            if (!DateTime.TryParse(month, out tmp))
                throw new ArgumentException("综合工资的月份的格式错误，示例2018-01");
            var rt= this.monthlySalaryService.Validate(tmp);
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, rt));
        }


        public ActionResult Generate(string month)
        {
            DateTime tmp;
            if (string.IsNullOrEmpty(month))
                throw new ArgumentException("必须指定综合工资的月份");
            if (!DateTime.TryParse(month, out tmp))
                throw new ArgumentException("综合工资的月份的格式错误，示例2018-01");
            var rt = this.monthlySalaryService.GenerateSalary(tmp)>0?Common.OperationResultType.Success: Common.OperationResultType.Error;
            var msg = rt == Common.OperationResultType.Success ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(rt, msg));
        }


        /// <summary>
        /// 财务经理审核
        /// </summary>
        /// <returns></returns>
        public ActionResult ManagerApprove()
        {
            return View();
        }

        /// <summary>
        /// CFO审核
        /// </summary>
        /// <returns></returns>
        public ActionResult CFOApprove()
        {
            return View();
        }

        

        public ActionResult GetEntity(Models.MonthSalaryQuery parameter)
        {
            DateTime datetime;
            if (!DateTime.TryParse(parameter.Month, out datetime))
                throw new ArgumentException("指定的月份格式错误");
            parameter.Month = datetime.ToString("yyyy-MM");
            Salary_MVC.DataModel.GZ_MonthlySalaryMaster master = monthlySalaryService.GetMasterByMonth(datetime);
            List<Tuple<DataModel.GZ_MonthlySalaryDetail, DataModel.GZ_Employee>> lst = new List<Tuple<DataModel.GZ_MonthlySalaryDetail, DataModel.GZ_Employee>>();
            if(master!=null)
                lst= monthlySalaryService.GetEntityByMaster(master, parameter).ToList();
            master =master?? new DataModel.GZ_MonthlySalaryMaster();
            List<DataModel.GZ_Department> lstDepartment = this.departmentService.GetEntity();
            var dictDepartment = lstDepartment.ToDictionary(x => x.Id, x => x.Name);
            List<DataModel.GZ_FinancialUnit> lstFinancialUnit = this.financialUnitSerive.GetEntity();
            var dictFinancialUnit = lstFinancialUnit.ToDictionary(x => x.Id, x => x.Name);
            var lsttmp = lst.OrderBy(x=>x.Item1.DepartmentId).Select(x => new {
                x.Item1.Id ,
                x.Item1.EmployeeId,
                DepartmentName = dictDepartment.ContainsKey(x.Item1.DepartmentId) ? dictDepartment[x.Item1.DepartmentId] : string.Empty,
                FinancailUnitName = dictFinancialUnit.ContainsKey(x.Item1.FinancailUnitId) ? dictFinancialUnit[x.Item1.FinancailUnitId] : string.Empty,
                x.Item1.BaseSalary,
                x.Item1.BonusSalary,
                x.Item1.TotalSalary ,
                x.Item1.SalaryDays,
                x.Item1.PayableSalary ,
                x.Item1.AwardMoney,
                x.Item1.PercentageMoney ,
                x.Item1.MakeupMoney,
                x.Item1.PayableOther,
                x.Item1.PayableTotal,
                x.Item1.SocialMoney,
                x.Item1.HouseMoney,
                x.Item1.TaxAmount,
                x.Item1.TaxMoney,
                x.Item1.PunishMoney,
                x.Item1.CreditMoney,
                x.Item1.ReduceOther,
                x.Item1.ReduceTotal,
                x.Item1.RealPay ,
                x.Item2.Name
            }).ToList();
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, new {
                master = new { master.Id, master.Month, Status = master.Status.ToString(), StatusValue=(int)master.Status },
                data_box = lsttmp,
            }));
        }

        /// <summary>
        /// HR发起审核
        /// </summary>
        /// <returns></returns>
        public ActionResult ApproveByFinance(string month)
        {
            DateTime tmp;
            if (string.IsNullOrEmpty(month))
                throw new ArgumentException("必须指定综合工资的月份");
            if (!DateTime.TryParse(month, out tmp))
                throw new ArgumentException("综合工资的月份的格式错误，示例2018-01");
            bool rt = monthlySalaryService.ApproveByFinance(tmp);
            var msg = rt ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, msg));
        }

        public ActionResult Approve(string targetid)
        {
            Guid id;
            Guid.TryParse(targetid, out id);
            this.ViewData["targetid"] = id;
            return this.View();
        }
        public ActionResult ApproveDetail(string targetid)
        {

            Guid id;
            Guid.TryParse(targetid, out id);
            this.ViewData["targetid"] = id;
            return this.View();
        }
        

        /// <summary>
        /// 获取最新月份的待财务审核的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEntityByFinanceManager(Models.GetEntityByApprove parameter)
        {
            DateTime datetime;
            if (!DateTime.TryParse(parameter.Month, out datetime))
                throw new ArgumentException("指定的月份格式错误");
            parameter.Month = datetime.ToString("yyyy-MM");
            var master = this.monthlySalaryService.GetEntityByFinanceManager(parameter);
            var lst = new List<Tuple<DataModel.GZ_MonthlySalaryDetail,DataModel.GZ_Employee>>();
            var parameterWrapper = new Models.MonthSalaryQuery();
            parameterWrapper.Name =parameter.TabIndex == Models.TabEnum.待审核 ? string.Empty : parameter.Name;
            if (master != null)
                lst = monthlySalaryService.GetEntityByMaster(master, parameterWrapper).ToList();
            master = master ?? new DataModel.GZ_MonthlySalaryMaster();
            List<DataModel.GZ_Department> lstDepartment = this.departmentService.GetEntity();
            var dictDepartment = lstDepartment.ToDictionary(x => x.Id, x => x.Name);
            List<DataModel.GZ_FinancialUnit> lstFinancialUnit = this.financialUnitSerive.GetEntity();
            var dictFinancialUnit = lstFinancialUnit.ToDictionary(x => x.Id, x => x.Name);
            var lsttmp = lst.OrderBy(x => x.Item1.DepartmentId).Select(x => new {
                x.Item1.Id,
                x.Item1.EmployeeId,
                DepartmentName = dictDepartment.ContainsKey(x.Item1.DepartmentId) ? dictDepartment[x.Item1.DepartmentId] : string.Empty,
                FinancailUnitName = dictFinancialUnit.ContainsKey(x.Item1.FinancailUnitId) ? dictFinancialUnit[x.Item1.FinancailUnitId] : string.Empty,
                x.Item1.BaseSalary,
                x.Item1.BonusSalary,
                x.Item1.TotalSalary,
                x.Item1.SalaryDays,
                x.Item1.PayableSalary,
                x.Item1.AwardMoney,
                x.Item1.PercentageMoney,
                x.Item1.MakeupMoney,
                x.Item1.PayableOther,
                x.Item1.PayableTotal,
                x.Item1.SocialMoney,
                x.Item1.HouseMoney,
                x.Item1.TaxAmount,
                x.Item1.TaxMoney,
                x.Item1.PunishMoney,
                x.Item1.CreditMoney,
                x.Item1.ReduceOther,
                x.Item1.ReduceTotal,
                x.Item1.RealPay,
                x.Item2.Name
            }).ToList();
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, new {
                master = new { master.Id, master.Month, Status = master.Status.ToString(), StatusValue=(int)master.Status },
                data_box = lsttmp,
            }));
        }

        public ActionResult GetEntityByCEO(string targetid)
        {
            Guid id;
            if (!Guid.TryParse(targetid, out id))
                throw new ArgumentException("必须指定要审核的工资主记录id");
            var rt= this.monthlySalaryService.GetEntityByCEO(id);

            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, rt));
            
        }

        public ActionResult GetEntityDetailByCEO(string targetid)
        {
            Guid id;
            if (!Guid.TryParse(targetid, out id))
                throw new ArgumentException("必须指定要审核的工资主记录id");
            var lstWrapper = this.monthlySalaryService.GetEntityDetailByCEO(id);
            var lstmaster = lstWrapper.Where(x => x.Item1.SubjectId == id).OrderBy(x => x.Item1.DepartmentId).ToList();
            var dictLastMonth = lstWrapper.Where(x => x.Item1.SubjectId != id).ToDictionary(x => x.Item1.EmployeeId, x => x);
            var dictDepartment= this.departmentService.GetEntity().ToDictionary(x=>x.Id,x=>x);
            var lstRT= lstmaster.Select((x, index) => new {
                Index = index + 1,
                x.Item2.Name,
                DepartmentName = dictDepartment.ContainsKey(x.Item1.DepartmentId) ? dictDepartment[x.Item1.DepartmentId].Name : string.Empty,
                LastMonthMoney = dictLastMonth.ContainsKey(x.Item1.EmployeeId) ? dictLastMonth[x.Item1.EmployeeId].Item1.RealPay : default(decimal),
                MonthMoney = x.Item1.RealPay
            }).ToList();
            decimal unit = 10000m;
            int youxiaoweishu = 2;//四舍五入小数位数
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, new {
                data_box = lstRT,
                TotalEmployee = lstRT.Count,
                TotalMoney = System.Math.Round(lstRT.Sum(x => x.MonthMoney) / unit,youxiaoweishu)
            }));
        }
        public ActionResult GetEntityByCFO(Models.GetEntityByApprove parameter)
        {
            DateTime datetime;
            if (!DateTime.TryParse(parameter.Month, out datetime))
                throw new ArgumentException("指定的月份格式错误");
            parameter.Month = datetime.ToString("yyyy-MM");
            var master = this.monthlySalaryService.GetEntityByCFO(parameter);
            var lst = new List<Tuple<DataModel.GZ_MonthlySalaryDetail, DataModel.GZ_Employee>>();
            var parameterWrapper = new Models.MonthSalaryQuery();
            parameterWrapper.Name = parameter.TabIndex == Models.TabEnum.待审核 ? string.Empty : parameter.Name;
            if (master != null)
                lst = monthlySalaryService.GetEntityByMaster(master, parameterWrapper).ToList();
            master = master ?? new DataModel.GZ_MonthlySalaryMaster();
            List<DataModel.GZ_Department> lstDepartment = this.departmentService.GetEntity();
            var dictDepartment = lstDepartment.ToDictionary(x => x.Id, x => x.Name);
            List<DataModel.GZ_FinancialUnit> lstFinancialUnit = this.financialUnitSerive.GetEntity();
            var dictFinancialUnit = lstFinancialUnit.ToDictionary(x => x.Id, x => x.Name);
            var lsttmp = lst.OrderBy(x => x.Item1.DepartmentId).Select(x => new {
                x.Item1.Id,
                x.Item1.EmployeeId,
                DepartmentName = dictDepartment.ContainsKey(x.Item1.DepartmentId) ? dictDepartment[x.Item1.DepartmentId] : string.Empty,
                FinancailUnitName = dictFinancialUnit.ContainsKey(x.Item1.FinancailUnitId) ? dictFinancialUnit[x.Item1.FinancailUnitId] : string.Empty,
                x.Item1.BaseSalary,
                x.Item1.BonusSalary,
                x.Item1.TotalSalary,
                x.Item1.SalaryDays,
                x.Item1.PayableSalary,
                x.Item1.AwardMoney,
                x.Item1.PercentageMoney,
                x.Item1.MakeupMoney,
                x.Item1.PayableOther,
                x.Item1.PayableTotal,
                x.Item1.SocialMoney,
                x.Item1.HouseMoney,
                x.Item1.TaxAmount,
                x.Item1.TaxMoney,
                x.Item1.PunishMoney,
                x.Item1.CreditMoney,
                x.Item1.ReduceOther,
                x.Item1.ReduceTotal,
                x.Item1.RealPay,
                x.Item2.Name
            }).ToList();
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, new {
                master = new { master.Id, master.Month, Status = master.Status.ToString(), StatusValue=(int)master.Status },
                data_box = lsttmp,
            }));
        }

        public ActionResult GetEntityById(string id)
        {
            Guid detailId;
            if (!Guid.TryParse(id, out detailId))
                throw new ArgumentException("必须指定要审核的工资主记录id");
            var wrapper= this.monthlySalaryService.GetEntityById(detailId);
            var rt = new {wrapper.Item1.Id,
                wrapper.Item2.Name,
                DepartmentName = wrapper.Item3.Name,
                FinanceUnitName=wrapper.Item4.Name,
                wrapper.Item2.Mobile,
                wrapper.Item1.PayableTotal,
                wrapper.Item1.AwardMoney,
                wrapper.Item1.PunishMoney,
                wrapper.Item1.PercentageMoney,
                wrapper.Item1.MakeupMoney,
                wrapper.Item1.PayableOther,
                wrapper.Item1.CreditMoney,
                wrapper.Item1.ReduceOther
            };
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, rt));

        }

        public ActionResult ApproveByFinanceManager(Models.ApproveInput parameter)
        {
            if (parameter.Handler != DataModel.GZ_ApproveLog.ApproveLogCategory.Through && parameter.Handler != DataModel.GZ_ApproveLog.ApproveLogCategory.NotThrough)
                throw new ArgumentException("审核操作只能是通过或者不通过");
            bool rt = monthlySalaryService.ApproveByFinanceManager(parameter);
            var msg = rt ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, msg));
        }

        public ActionResult ApproveByCEO(Models.ApproveInput parameter)
        {
            if (parameter.Handler != DataModel.GZ_ApproveLog.ApproveLogCategory.Through && parameter.Handler != DataModel.GZ_ApproveLog.ApproveLogCategory.NotThrough)
                throw new ArgumentException("审核操作只能是通过或者不通过");
            bool rt = monthlySalaryService.ApproveByCEO(parameter);
            var msg = rt ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, msg));
        }


        public ActionResult ApproveByCFO(Models.ApproveByCFOInput parameter)
        {
            if (parameter.Handler != DataModel.GZ_ApproveLog.ApproveLogCategory.Through && parameter.Handler != DataModel.GZ_ApproveLog.ApproveLogCategory.NotThrough)
                throw new ArgumentException("审核操作只能是通过或者不通过");
            if (parameter.Id == Guid.Empty)
                throw new ArgumentException("必须指定要审核的工资主记录");
            if (parameter.Operators == null || parameter.Operators.Length < 1)
                throw new ArgumentException("必须指定下一步的审核人");
            bool rt = monthlySalaryService.ApproveByCFO(parameter);
            var msg = rt ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, msg));
        }

        /// <summary>
        /// 获取指定月份工资的审核记录
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public ActionResult GetApproveLog(string month)
        {
            if (month == null)
                throw new ArgumentException("必须指定工资月份");
            DateTime datetime;
            if (!DateTime.TryParse(month, out datetime))
                throw new ArgumentException("指定的月份格式错误");
            var master= this.monthlySalaryService.GetMasterByMonth(datetime);
            if (master == null)
                throw new ArgumentException(string.Format("未找到{0}月的工资主记录",month));
            var lst= this.approveLogService.GetEntityByTargetId(master.Id);
            List<DataModel.GZ_User> lstUser = this.userService.GetAllUser();
            var dictUser = lstUser.ToDictionary(x => x.Id, x => x.Name);
            var tmp = lst.Select(x => new {
                Name = dictUser.ContainsKey(x.OperatorId) ? dictUser[x.OperatorId] : "admin",
                OperatorTime = x.OperatorTime.ToString("yyyy-MM-dd HH:mm"),
                TargetStatus = ((Salary_MVC.DataModel.GZ_MonthlySalaryMaster.MonthlySalaryStatus)x.TargetStatus).ToString(),
                Handler = x.Category == DataModel.GZ_ApproveLog.ApproveLogCategory.Through ? "通过" : "不通过",
                x.Comment
            }).ToList();
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, tmp));
        }

        /// <summary>
        /// 导出列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportList(string month)
        {
            DateTime tmp;
            if (string.IsNullOrEmpty(month))
                throw new ArgumentException("必须指定综合工资的月份");
            if (!DateTime.TryParse(month, out tmp))
                throw new ArgumentException("综合工资的月份的格式错误，示例2018-01");
            var filepath = System.IO.Path.Combine(TemplateFileDirectory, "工资列表导出模板.xlsx");
            var fullpath = this.Server.MapPath(filepath);
            if (!System.IO.File.Exists(fullpath))
                throw new ArgumentException("工资列表导出模板.xlsx 不存在");
            if (this.Request.IsAjaxRequest())
            {
                var buffer = this.monthlySalaryService.ExportList(tmp, fullpath);
                this.Session[tmp.ToString("yyyy-MM")] = buffer;
                return this.Json(new Common.OperationResult(Common.OperationResultType.Success));
            }
            else
            {
                var buffer = this.Session[tmp.ToString("yyyy-MM")] as byte[];
                this.Session[tmp.ToString("yyyy-MM")] = null;
                return this.File(buffer, "application/vnd.ms-excel", string.Format("{0}工资明细表.xlsx", tmp.ToString("yyyy年MM月")));
            }
        }

        public ActionResult GetCeoApproveOperator()
        {
            List<DataModel.GZ_User> lst= this.monthlySalaryService.GetCeoApproveOperator();
            var lstTmp= lst.Select(x => new { x.Id, x.Name }).ToList();
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success,string.Empty, lstTmp));
        }

        public ActionResult Edit(Models.MonthSalaryEdit parameter)
        {
            if (!this.ModelState.IsValid)
                throw new Common.ModelBindingException(ModelState.Values);
            if (parameter.CreditMoney < 0 || parameter.MakeupMoney < 0 || parameter.PayableOther < 0 || parameter.PercentageMoney < 0 || parameter.ReduceOther < 0)
                throw new ArgumentException("金额不能小于0");
            if (parameter.Id == Guid.Empty)
                throw new ArgumentException("必须指定要修改的记录的Id");
            var rt = this.monthlySalaryService.Edit(parameter) > 0 ? Common.OperationResultType.Success : Common.OperationResultType.Error;
            var msg = rt == Common.OperationResultType.Success ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(rt, msg));

        }

        public ActionResult ExportBank(string month)
        {
            DateTime tmp;
            if (string.IsNullOrEmpty(month))
                throw new ArgumentException("必须指定综合工资的月份");
            if (!DateTime.TryParse(month, out tmp))
                throw new ArgumentException("综合工资的月份的格式错误，示例2018-01");
            if(this.Request.IsAjaxRequest())
            {//先导出，看能否成功，不行的话会弹框
                var buffer = this.monthlySalaryService.ExportBank(tmp);
                 this.Session[tmp.ToString("yyyy-MM")] = buffer;
                return this.Json(new Common.OperationResult(Common.OperationResultType.Success));
            }
            else
            {
                var buffer = this.Session[tmp.ToString("yyyy-MM")] as byte[];
                this.Session[tmp.ToString("yyyy-MM")] = null;
                return this.File(buffer, "application/vnd.ms-excel", string.Format("{0}工资表.xlsx", tmp.ToString("yyyy年MM月")));
            }
        }

        [HttpPost]
        public ActionResult SyncToZlApp(string month)
        {
            var obj = monthlySalaryService.SyncToZlApp(month);
            return Json(new Common.OperationResult(Common.OperationResultType.Success, "", "", obj));
        }
    }
}