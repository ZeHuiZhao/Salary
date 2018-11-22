using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Salary_MVC.Controllers
{
    public class BaseController: Controller
    {
        //hui备注，需要做登录校验 TODO
        
    }



    public class ZlController: BaseController
    {
        protected const string UploadFileDirectoryRoot="/file/";
        protected const string TemplateFileDirectory = "/Template/";

        protected override void OnException(ExceptionContext filterContext)
        {
            if (!this.Request.IsAjaxRequest())//ajax的请求
                return;//暂时只处理ajax的异常，视图的异常直接报黄页，
            filterContext.ExceptionHandled = true;
            Common.OperationResult rt = new Common.OperationResult( Common.OperationResultType.Error);
            rt.Message = filterContext.Exception.Message;
            var modelException = filterContext.Exception as Salary_MVC.Common.ModelBindingException;
            if (modelException != null)
            {
                var lstError= modelException.ValidateResult.Where(x=>x.Errors.Count>0).Select(x => string.Join(",", x.Errors.Select(a => a.ErrorMessage)));
                rt.Message = string.Join("；", lstError);
            }
            filterContext.Result = this.Json(rt,JsonRequestBehavior.AllowGet);
        }

    }
}