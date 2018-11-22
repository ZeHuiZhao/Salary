using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary.DataModel.Entity;

namespace Salary.Models.CompanyContact
{
    public class QueryCompanyContactPaging:BasePaging
    {
        public List<CompanyContactDto> CompanyContactList { get; set; }
    }
}