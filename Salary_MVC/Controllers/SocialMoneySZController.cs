using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Salary_MVC.Controllers
{
    public class SocialMoneySZController : ZlController
    {

        private readonly Salary_MVC.Services.SocialMoneySZService _socialMoney = new Services.SocialMoneySZService();
        Salary_MVC.Services.UserService userService = new Services.UserService();
        /// <summary>
        /// 社保页面（hr）
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 社保页面（财务）
        /// </summary>
        /// <returns></returns>
        public ActionResult Approve()
        {
            return View();
        }

        public ActionResult GetEntity(Models.GetEntityInput parameter)
        {
            DateTime datetime;
            if (!DateTime.TryParse(parameter.Month, out datetime))
                throw new ArgumentException("指定的月份格式错误");
            parameter.Month = datetime.ToString("yyyy-MM");
            var master = _socialMoney.GetMasterByMonth(parameter);
            var lst = new List<DataModel.GZ_SocialMoneyDetailSZ>();
            if (master != null)
                lst = _socialMoney.GetDetailEntity(master, parameter).OrderByDescending(x => x.TotalMoney).ToList();
            master = master ?? new DataModel.GZ_SocialMoneyMasterSZ();
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, new {
                master = new { master.Id, master.Month, Status = master.Status.ToString(), StatusValue=(int)master.Status },
                data_box = lst,
                totalEmployee = lst.Count,
                TotalMoney = lst.Sum(x => x.TotalMoney),
                TotalPesonal = lst.Sum(x => x.TotalPesonal),
                TotalCorp = lst.Sum(x => x.TotalCorp)
            }));
        }

        /// <summary>
        /// HR发起审核
        /// </summary>
        /// <returns></returns>
        public ActionResult ApproveByHR(Models.UpdateDto parameter)
        {
            bool rt = _socialMoney.ApproveByHR(parameter);
            var msg = rt ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, msg));
        }

        /// <summary>
        /// 社保导入
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ActionResult Import(Salary_MVC.Models.ImportInput parameter)
        {
            if (parameter == null)
                throw new ArgumentException("请求参数格式错误");
            if (string.IsNullOrEmpty(parameter.FilePath))
                throw new ArgumentException("必须指定导入的excel文件所在的路径");
            string fullFilePath = this.Server.MapPath(parameter.FilePath);
            if (!System.IO.File.Exists(fullFilePath))
                throw new ArgumentException("指定的excel文件不存在");
            DateTime datetime;
            if (!DateTime.TryParse(parameter.Month, out datetime))
                throw new ArgumentException("指定的月份格式错误");
            parameter.Month = datetime.ToString("yyyy-MM");
            var rt = _socialMoney.Import(parameter, fullFilePath);
            var rst = new Common.OperationResult(rt > 0 ? Common.OperationResultType.Success : Common.OperationResultType.Error);
            rst.Message = rt > 0 ? string.Format("操作成功，总共导入{0}条记录", rt-1) : "操作失败";
            rst.AppendData = new { SyntaxError = rt == -1 };
            return this.Json(rst);
        }

        /// <summary>
        /// 删除指定月份的社保数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ActionResult DeleteByMonth(string Month)
        {
            DateTime datetime;
            if (!DateTime.TryParse(Month, out datetime))
                throw new ArgumentException("指定的月份格式错误");
            var rt = _socialMoney.DeleteByMonth(datetime.ToString("yyyy-MM")) >0? Common.OperationResultType.Success : Common.OperationResultType.Error;
            var msg = rt== Common.OperationResultType.Success ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(rt, msg));
        }

        /// <summary>
        /// 获取财务审核列表数据
        /// 待审核，已审核
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEntityByFinance(Models.GetEntityByApprove parameter)
        {
            var master = _socialMoney.GetMasterByFinance(parameter);
            var lst = new List<DataModel.GZ_SocialMoneyDetailSZ>();
            if (master != null)
                lst = _socialMoney.GetDetailEntity(master, new Models.GetEntityInput() { Name = parameter.TabIndex == Models.TabEnum.待审核 ? string.Empty : parameter.Name }).OrderByDescending(x => x.TotalMoney).ToList();
            master = master ?? new DataModel.GZ_SocialMoneyMasterSZ();
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, new {
                master = new { master.Id, master.Month, Status = master.Status.ToString(),StatusValue=(int)master.Status },
                data_box = lst,
                totalEmployee = lst.Count,
                TotalMoney = lst.Sum(x => x.TotalMoney),
                TotalPesonal = lst.Sum(x => x.TotalPesonal),
                TotalCorp = lst.Sum(x => x.TotalCorp)
            }));
        }

        /// <summary>
        /// 财务审核
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ActionResult ApproveByFinance(Models.ApproveInput parameter)
        {
            if (parameter.Handler != DataModel.GZ_ApproveLog.ApproveLogCategory.Through && parameter.Handler != DataModel.GZ_ApproveLog.ApproveLogCategory.NotThrough)
                throw new ArgumentException("审核操作只能是通过或者不通过");
            var rt = _socialMoney.ApproveByFinance(parameter) ? Common.OperationResultType.Success : Common.OperationResultType.Error;
            var msg = rt== Common.OperationResultType.Success ? "操作成功" : "操作失败";
            return this.Json(new Common.OperationResult(rt, msg));
        }

        /// <summary>
        /// 获取指定月份社保的审核记录
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ActionResult GetApproveLog(string Month)
        {
            DateTime datetime;
            if (!DateTime.TryParse(Month, out datetime))
                throw new ArgumentException("指定的月份格式错误");
            List<DataModel.GZ_ApproveLog> lst = _socialMoney.GetApproveLogByMonth(datetime.ToString("yyyy-MM"));
            var dictUser= this.userService.GetAllUser().ToDictionary(x=>x.Id,x=>x);
            var tmp = lst.Select(x => new {
                Name = dictUser.ContainsKey(x.OperatorId)?dictUser[x.OperatorId].Name: "admin",
                OperatorTime = x.OperatorTime.ToString("yyyy-MM-dd HH:mm"),
                TargetStatus = ((Salary_MVC.Enum.ApproveStatus)x.TargetStatus).ToString(),
                Handler = x.Category == DataModel.GZ_ApproveLog.ApproveLogCategory.Through ? "通过" : "不通过",
                x.Comment
            }).ToList();
            return this.Json(new Common.OperationResult(Common.OperationResultType.Success, string.Empty, tmp));
        }

        /// <summary>
        /// 下载模板
        /// </summary>
        /// <returns></returns>
        public ActionResult Template()
        {
            var filepath = System.IO.Path.Combine(TemplateFileDirectory, "深圳社保模板.xlsx");
            var fullpath = this.Server.MapPath(filepath);
            return this.File(fullpath, "application/vnd.ms-excel", System.IO.Path.GetFileName(fullpath));
        }

        public ActionResult ErrorInfo(string filename)
        {
            var fullpathTmp = this.Server.MapPath(filename);
            //var fullpath = this.Server.MapPath(filepath);
            var fullpath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(fullpathTmp), System.IO.Path.GetFileNameWithoutExtension(fullpathTmp) + ".txt");
            return this.File(fullpath, "text/plain", "错误消息.txt");
        }
    }
}