using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Common
{
    public class ServiceHelper
    {
        public static List<string> GetParams(params string[] str)
        {
            return str.ToList();
        }
    }
}