using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary_MVC.DataModel;

namespace Salary_MVC.Services
{
    public class ApproveLogService : Data.Service<DataModel.GZ_ApproveLog>
    {
        public List<GZ_ApproveLog> GetEntityByTargetId(Guid id)
        {
            var lst= this.DbContext.GZ_ApproveLog.Where(x => x.TargetId == id).OrderBy(x => x.OperatorTime).ToList();
            return lst;
        }
    }
}