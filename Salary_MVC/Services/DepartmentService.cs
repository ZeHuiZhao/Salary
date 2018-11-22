using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Services
{
    public class DepartmentService : Data.Service<DataModel.GZ_Department>
    {
        public List<DataModel.GZ_Department> GetEntityWithKeyValue()
        {
            var lst= this.DbContext.GZ_Department.Where(x => x.Status == 1).OrderBy(x=>x.Order).ToList();
            return lst;
        }

        public List<DataModel.GZ_Department> GetEntity()
        {
            var lst = this.DbContext.GZ_Department.OrderBy(x => x.Order).ToList();
            return lst;
        }
    }
}