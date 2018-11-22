using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Salary.Common;

namespace Salary.Controllers
{
    public class FileController : BaseController
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UploadFile()
        {
            HttpFileCollection filelist = HttpContext.Current.Request.Files;
            HttpPostedFile file = null;
            if (filelist != null && filelist.Count > 0)
            {
                file = filelist[0];
            }
            if (file == null || file.ContentLength <= 0)
            {
                return Json(new OperationResult(OperationResultType.Error, "请选择文件"));
            }
            var fileName = this.UploadFile(file,new string[] { "png", "jpg" });
            if (string.IsNullOrEmpty(fileName))
            {
                return Json(new OperationResult(OperationResultType.Error, "上传失败"));
            }
            return Json(new OperationResult(OperationResultType.Success, fileName));
        }
        
        

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="extendNames">文件扩展名</param>
        /// <returns></returns>
        private string UploadFile(HttpPostedFile file,string [] extendNames)
        {
            try
            {
                //string[] extendNames = new string[] { "png", "jpg", "doc", "docx", "xls", "xlsx", "ppt", "pptx", "gif", "pdf", "rar", "txt", "zip" };
                string extendName = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);
                if (!extendNames.Contains(extendName.ToLower()) || true)
                {
                    //string str_path = "~/uploadfile/user_" + Cookies_C.UserCode + "/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("yyyyMMddHHmmss") + "/";
                    string str_path = "/uploadfile/user_" + 247 + "/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("yyyyMMddHHmmss") + "/";
                    if (!Directory.Exists(HttpContext.Current.Server.MapPath(str_path)))
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(str_path));
                    }
                    file.SaveAs(HttpContext.Current.Server.MapPath(str_path) + file.FileName.Substring(file.FileName.LastIndexOf("/") + 1));
                    str_path += file.FileName.Substring(file.FileName.LastIndexOf("/") + 1);
                    return str_path;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return string.Empty;
            }
            
        }
    }
}
