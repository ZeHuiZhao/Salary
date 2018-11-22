using Salary_MVC.Data;
using Salary_MVC.DataModel;
using Salary_MVC.Models.FunctionGroupRight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Services
{
    public class FunctionGroupRightService : Service<GZ_FunctionGroupRight>
    {
        internal object GetGroupPrivilegeList()
        {
            var list = this.DbContext.GZ_Function.Where(o => o.ParentId == null).OrderBy(o => o.Order).Select(O => new
            {
                id = O.Id,
                text = O.Name,
                children = this.DbContext.GZ_Function.Where(x => x.ParentId == O.Id).OrderBy(x => x.Order).Select(x => new { id = x.Id, text = x.Name })
            });
            return list;
        }

        internal object GetGroupPrivilege(Guid id)
        {
            var list = this.DbContext.GZ_FunctionGroupRight.Where(o => o.FunctionGroupId == id).Select(o => o.FunctionId).ToList();
            return list;
        }

        internal object GetGroupName(Guid id)
        {
            var list = this.DbContext.GZ_FunctionGroup.Where(u => u.Id == id).Select(u => u.Name).ToList();
            return list;
        }

        internal int UpdateGroupPrivilege(UpdateFunctionGroupRightDto dto)
        {
            DbContext.GZ_FunctionGroupRight.RemoveRange(DbContext.GZ_FunctionGroupRight.Where(u => u.FunctionGroupId == dto.FunctionGroupId));

            var list = dto.FunctionId.Split(",".ToArray());

            foreach (string item_function in list)
            {
                GZ_FunctionGroupRight model_add = new GZ_FunctionGroupRight() { Id = new Guid(Guid.NewGuid().ToString("N")), CreateDate = DateTime.Now, CreateUser = Cookies.UserCode };
                model_add.FunctionGroupId = dto.FunctionGroupId;
                model_add.FunctionId = new Guid(item_function);
                this.DbContext.GZ_FunctionGroupRight.Add(model_add);
            }
            return this.DbContext.SaveChanges();

        }

    }
}