using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.Company
{
    public class QueryCompanyPaging:BasePaging
    {
        public List<ComapnyDto> CompanyList { get; set; }
    }
}