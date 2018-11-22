using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Code
{
    [AttributeUsage(AttributeTargets.All)]
    public class ViewDescriptionAttribute : Attribute
    {
        public string Description { get; set; }
        public ViewDescriptionAttribute(string DescString)
        {
            this.Description = DescString;
        }

    }
}