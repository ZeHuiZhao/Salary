using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Common
{
    /// <summary>
    /// 工资系统自定义异常
    /// </summary>
    public class InputException:System.Exception
    {
        public InputException(string msg):base(msg)
        {
            
        }
    }
}