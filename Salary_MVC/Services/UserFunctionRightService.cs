using Salary_MVC.Data;
using Salary_MVC.DataModel;
using Salary_MVC.Models.UserFunctionRight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Services
{
    public class UserFunctionRightService : Service<GZ_UserFunctionRight>
    {
        internal object GetPrivilegeList()
        {
            var list = this.DbContext.GZ_Function.Where(o => o.ParentId == null).OrderBy(o => o.Order).Select(O => new
            {
                id = O.Id,
                text = O.Name,
                children = this.DbContext.GZ_Function.Where(x => x.ParentId == O.Id).OrderBy(x => x.Order).Select(x => new { id = x.Id, text = x.Name })
            });
            return list;
        }

        internal object GetUserPrivilege(Guid id)
        {
            var list = this.DbContext.GZ_UserFunctionRight.Where(o => o.UserId == id).Select(o => o.FunctionId).ToList();
            return list;
        }

        internal int UpdateUserPrivilege(UpdateUserFunctionRightDto dto)
        {
            DbContext.GZ_UserFunctionRight.RemoveRange(DbContext.GZ_UserFunctionRight.Where(u => u.UserId == dto.UserId));

            var list = dto.FunctionId.Split(",".ToArray());

            foreach (string item_function in list)
            {
                GZ_UserFunctionRight model_add = new GZ_UserFunctionRight() { Id = new Guid(Guid.NewGuid().ToString("N")), CreateDate = DateTime.Now, CreateUser = Cookies.UserCode };
                model_add.UserId = dto.UserId;
                model_add.FunctionId = new Guid(item_function);
                this.DbContext.GZ_UserFunctionRight.Add(model_add);
            }
            return this.DbContext.SaveChanges();
        }

        internal object GetUserName(Guid id)
        {
            var list = this.DbContext.GZ_User.Where(u => u.Id == id).Select(u => u.Name).ToList();
            return list;
        }
    }


}