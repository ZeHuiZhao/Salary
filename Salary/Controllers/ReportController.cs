using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Salary.Common;
using Salary.Models.Report;
using Salary.Service;

namespace Salary.Controllers
{
    public class ReportController : BaseController
    {
        private readonly MaterialService _material = new MaterialService();
        private readonly ReportService _report = new ReportService();
        /// <summary>
        /// 导出素材分析报表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ExportMaterialList(MaterialQueryDto query)
        {
            try
            {
                LogHelper.WriteInfoLog("api开始时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ff"));
                var dt = _material.ExportMaterialList(query);
                if(dt==null)return Json(new OperationResult(OperationResultType.Error, "请登陆或者没有权限"));

                LogHelper.WriteInfoLog("api结束时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ff"));
                LogHelper.WriteInfoLog("excel导出中间处理开始时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ff"));
                ExportExcel exp = new ExportExcel();
                ExportExcel.ExcelSheet sheet = new ExportExcel.ExcelSheet("素材分析报表", dt.DefaultView, ExportTypes.Custom);
                sheet.Add("Id", "标识列");
                sheet.Add("MaterialTitle", "素材标题");
                sheet.Add("MaterialTypeName", "所属类别");
                sheet.Add("ShareCount", "素材转发量");
                sheet.Add("BrowserCount", "素材浏览量");
                sheet.Add("EnrollCount", "报名人数");
                sheet.Add("ParticipateCount", "参课量");
                sheet.Add("LastTimeString", "最后分享时间");
                sheet.DisableAutoSetWidth();
                exp.AddSheet(sheet);
                string directory = "~/uploadfile/export/" + Cookies.UserId + "/";
                string url = "~/uploadfile/export/" + Cookies.UserId + "/" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".xls";
                string result = Config.websiteUrl + url.TrimStart('~');
                directory = System.Web.Hosting.HostingEnvironment.MapPath(directory);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                LogHelper.WriteInfoLog("excel导出中间处理结束时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ff"));
                LogHelper.WriteInfoLog("excel导出开始时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ff"));
                exp.Export(System.Web.Hosting.HostingEnvironment.MapPath(url));
                LogHelper.WriteInfoLog("excel导出结束时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ff"));
                return Json(new OperationResult(OperationResultType.Success, "", "", result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                throw ex;
            }

        }

        /// <summary>
        /// 导出素材被销售员使用报表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ExportMateriaSaleslList(int id)
        {
            try
            {
                var dt = _material.ExportMateriaSaleslList(id);
                if (dt == null) return Json(new OperationResult(OperationResultType.Error, "请登陆或者没有权限"));
                ExportExcel exp = new ExportExcel();
                ExportExcel.ExcelSheet sheet = new ExportExcel.ExcelSheet("销售员使用素材报表", dt.DefaultView, ExportTypes.Custom);
                sheet.Add("TrueName", "销售员");
                sheet.Add("ShareCount", "素材转发量");
                sheet.Add("BrowserCount", "素材浏览量");
                sheet.Add("EnrollCount", "报名人数");
                sheet.Add("ParticipateCount", "参课量");
                sheet.Add("LastTime", "最后分享时间");
                sheet.DisableAutoSetWidth();
                exp.AddSheet(sheet);
                string directory = "~/uploadfile/export/" + Cookies.UserId + "/";
                string url = "~/uploadfile/export/" + Cookies.UserId + "/" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".xls";
                string result = Config.websiteUrl + url.TrimStart('~');
                directory = System.Web.Hosting.HostingEnvironment.MapPath(directory);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                exp.Export(System.Web.Hosting.HostingEnvironment.MapPath(url));
                return Json(new OperationResult(OperationResultType.Success, "", "", result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                throw ex;
            }

        }

        /// <summary>
        /// 获取素材分析详情(树状图数据)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetMaterialDataById([FromUri]MaterialDataQueryDto dto)
        {
            var obj = _report.GetMaterialDataById(dto);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 获取素材分析销售员列表（素材分析-销售员数据）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetMaterialSalesDataById([FromUri]MaterialSalesDataQueryDto dto)
        {
            var obj = _report.GetMaterialSalesDataById(dto);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 获取渠道分析报表（漏斗图数据--第一级）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetChannelExportList([FromUri]ChannelExportQueryDto query)
        {
            var obj = _report.GetChannelExportList(query);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 获取渠道分析报表（销售员--第一级）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetChannelSalesExportList([FromUri]ChannelSalesQueryDto query)
        {
            var obj = _report.GetChannelSalesExportList(query);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 导出渠道分析报表
        /// </summary>
        /// <param name="query">素材id</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ExportSalesList(ChannelExportQueryDto query)
        {
            try
            {
                var dt = _report.ExportSalesList(query);
                if (dt == null) return Json(new OperationResult(OperationResultType.Error, "请登陆或者没有权限"));
                ExportExcel exp = new ExportExcel();
                ExportExcel.ExcelSheet sheet = new ExportExcel.ExcelSheet("渠道分析报表", dt.DefaultView, ExportTypes.Custom);
                sheet.Add("TrueName", "销售员");
                sheet.Add("ShareCount", "素材转发量");
                sheet.Add("BrowseCount", "素材浏览量");
                sheet.Add("EnrollCount", "报名人数");
                sheet.Add("ParticipateCount", "参课量");
                sheet.DisableAutoSetWidth();
                exp.AddSheet(sheet);
                string directory = "~/uploadfile/export/" + Cookies.UserId + "/";
                string url = "~/uploadfile/export/" + Cookies.UserId + "/" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".xls";
                string result = Config.websiteUrl + url.TrimStart('~');
                directory = System.Web.Hosting.HostingEnvironment.MapPath(directory);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                exp.Export(System.Web.Hosting.HostingEnvironment.MapPath(url));
                return Json(new OperationResult(OperationResultType.Success, "", "", result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                throw ex;
            }
        }

        /// <summary>
        /// 获取渠道分销报表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetChannelYYList([FromUri]ChannelYYQueryDto query)
        {
            var obj = _report.GetChannelYYList(query);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 导出渠道分销报表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ExportChannelYYList(ChannelYYQueryNoPagingDto dto)
        {
            try
            {
                var dt = _report.ExportChannelYYList(dto);
                if (dt == null) return Json(new OperationResult(OperationResultType.Error, "请登陆或者没有权限"));
                ExportExcel exp = new ExportExcel();
                ExportExcel.ExcelSheet sheet = new ExportExcel.ExcelSheet("渠道分销报表", dt.DefaultView, ExportTypes.Custom);
                sheet.Add("MaterialTitle", "素材名称");
                sheet.Add("MaterialTypeName", "素材类型");
                sheet.Add("TrueName", "TrueName");
                sheet.Add("ShareCount", "素材转发量");
                sheet.Add("BrowseCount", "素材浏览量");
                sheet.Add("EnrollCount", "报名人数");
                sheet.Add("ParticipateCount", "参课量");
                sheet.Add("LastTime", "分享时间");
                sheet.DisableAutoSetWidth();
                exp.AddSheet(sheet);
                string directory = "~/uploadfile/export/" + Cookies.UserId + "/";
                string url = "~/uploadfile/export/" + Cookies.UserId + "/" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".xls";
                string result = Config.websiteUrl + url.TrimStart('~');
                directory = System.Web.Hosting.HostingEnvironment.MapPath(directory);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                exp.Export(System.Web.Hosting.HostingEnvironment.MapPath(url));
                return Json(new OperationResult(OperationResultType.Success, "", "", result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                throw ex;
            }
        }

        /// <summary>
        /// 渠道分析报表（第二级）根据销售员id查询（漏斗图）数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetSalesDataById(int id)
        {
            var obj = _report.GetSalesDataById(id);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 渠道分析报表（第二级）根据销售员id查询素材列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetSalesMaterialList([FromUri]MaterialSalesDataQueryDto dto)
        {
            var obj = _report.GetSalesMaterialList(dto);
            return Json(new OperationResult(OperationResultType.Success, "", "", obj));
        }

        /// <summary>
        /// 根据销售员id导出数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ExportSalesDataById(int id)
        {
            try
            {
                var dt = _report.ExportSalesDataById(id);
                if (dt == null) return Json(new OperationResult(OperationResultType.Error, "请登陆或者没有权限"));
                ExportExcel exp = new ExportExcel();
                ExportExcel.ExcelSheet sheet = new ExportExcel.ExcelSheet("销售员素材分析报表", dt.DefaultView, ExportTypes.Custom);
                sheet.Add("MaterialTitle", "素材名称");
                sheet.Add("ShareCount", "素材转发量");
                sheet.Add("BrowseCount", "素材浏览量");
                sheet.Add("EnrollCount", "报名人数");
                sheet.Add("ParticipateCount", "参课量");
                sheet.DisableAutoSetWidth();
                exp.AddSheet(sheet);
                string directory = "~/uploadfile/export/" + Cookies.UserId + "/";
                string url = "~/uploadfile/export/" + Cookies.UserId + "/" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".xls";
                string result = Config.websiteUrl + url.TrimStart('~');
                directory = System.Web.Hosting.HostingEnvironment.MapPath(directory);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                exp.Export(System.Web.Hosting.HostingEnvironment.MapPath(url));
                return Json(new OperationResult(OperationResultType.Success, "", "", result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                throw ex;
            }
        }
    }
}
