using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

using System.Threading.Tasks;
using System.Web;

using Salary.Common;
using System.Web.Mvc;
using Salary_MVC.Common;

namespace Salary_MVC.Controllers
{
    public class FileController : ZlController
    {

        Services.AttachmentService attachmentService = new Services.AttachmentService();
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult  UploadFile(int Group)
        {
            var lst= System.Enum.GetValues(typeof(Salary_MVC.Enum.FileGroupEnum)).Cast<Salary_MVC.Enum.FileGroupEnum>().ToList();
            var groupEnum = (Salary_MVC.Enum.FileGroupEnum)Group;
            if (!lst.Contains(groupEnum))
                return this.Json(new Common.OperationResult(OperationResultType.Error,"未识别的Group"));
            var filelist = this.HttpContext.Request.Files;
            if (filelist == null || filelist.Count <= 0||filelist[0]==null||filelist[0].ContentLength<=0)
            {
                return Json(new OperationResult(OperationResultType.Error, "请选择文件"));
            }
            HttpPostedFileBase file = filelist[0];
            var extensions = new string[] { "png", "jpg", "doc", "docx", "xls", "xlsx", "ppt", "pptx", "gif", "pdf", "rar", "txt", "zip" };

            var fileName = this.UploadFile(file, groupEnum);
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
        private string UploadFile(HttpPostedFileBase file, Salary_MVC.Enum.FileGroupEnum group)
        {
            try
            {
                //string[] extendNames = new string[] { "png", "jpg", "doc", "docx", "xls", "xlsx", "ppt", "pptx", "gif", "pdf", "rar", "txt", "zip" };
               // string extendName = System.IO.Path.GetExtension(file.FileName);
                //file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);
                //string str_path = "~/uploadfile/user_" + Cookies_C.UserCode + "/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("yyyyMMddHHmmss") + "/";
                string str_path = string.Format("/file/{0}/{1}/", group.ToString(),DateTime.Now.ToString("yyyy-MM"));
                string fileName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + file.FileName;
                string localDirectory = this.Server.MapPath(str_path);
                    //"/uploadfile/user_" + 247 + "/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("yyyyMMddHHmmss") + "/";
                if (!Directory.Exists(localDirectory))
                {
                    Directory.CreateDirectory(localDirectory);
                }
                
                string localPath = System.IO.Path.Combine(localDirectory, fileName);
                file.SaveAs(localPath);
                return str_path+fileName;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return string.Empty;
            }

        }

        /// <summary>
        /// 下载附件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetAttachment(Guid sourceId)
        {
            if (sourceId == Guid.Empty)
                throw new ArgumentException("必须指定津贴记录的Id");
            DataModel.GZ_Attachment attachment = this.attachmentService.GetEnityBySourceId(sourceId);
            if (attachment == null)
                throw new ArgumentException("未找到该津贴记录的附近信息");
            var filePath = this.Server.MapPath(attachment.FilePath);
            if (!System.IO.File.Exists(filePath))
                throw new ArgumentException("附件文件不存在，可能被删除，请联系管理员");
            var FileName = System.IO.Path.GetFileName(filePath).Substring("yyyy_MM_dd_HH_mm_ss".Length);
            return this.File(filePath, "application/octet-stream", FileName);
        }
    }
}
