using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Salary_MVC.Common
{
    public class ModelBindingException: System.Exception
    {
        public ICollection<ModelState> ValidateResult { get; set; }
        public ModelBindingException(ICollection<ModelState> validateResult)
        {
            this.ValidateResult = validateResult;
        }
    }
}