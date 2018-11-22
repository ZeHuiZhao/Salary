using Salary_MVC.Data;
using Salary_MVC.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Services
{
    public class CommonService : Service<GZ_Company>
    {
        internal object GetCompanyList()
        {
            return Entities.Select(o => new { o.Id, o.Name }).ToList();
        }

        internal object GetDepartmentList(Guid? companyId)
        {
            var list = DbContext.GZ_Department.Where(o=>true);
            if (companyId.HasValue)
            {
                list = list.Where(o => o.CpId == companyId.Value);
            }
            return list.Select(o=>new { o.Id,o.Name}).ToList();
        }
    }
}