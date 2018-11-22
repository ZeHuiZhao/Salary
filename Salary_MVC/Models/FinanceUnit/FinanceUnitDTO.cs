using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models
{
    public class FinanceUnitAdd
    {
        public string Name { get; set; }
    }

    public class FinanceUnitEdit:UpdateDto
    {
        public string Name { get; set; }
    }
}