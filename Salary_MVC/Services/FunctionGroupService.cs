using Salary_MVC.Data;
using Salary_MVC.DataModel;
using Salary_MVC.Models.FunctionGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Services
{
    public class FunctionGroupService : Service<DataModel.GZ_FunctionGroup>
    {
        internal int AddFunctionGroup(AddFunctionGroupDto add)
        {
            GZ_FunctionGroup model = new GZ_FunctionGroup { Id = new Guid(Guid.NewGuid().ToString("N")), CreateDate = DateTime.Now, CreateUser = Cookies.UserCode, LastUpdateDate = DateTime.Now, LastUpdateUser = Cookies.UserCode };
            model.Name = add.Name;

            DbContext.GZ_FunctionGroup.Add(model);
            return DbContext.SaveChanges();
        }

        internal int UpdateFunctionGroup(UpdateFunctionGroupDto update)
        {
            GZ_FunctionGroup model = Entities.Where(o => o.Id == update.Id).FirstOrDefault();

            if (model != null)
            {
                model.Name = update.Name;
                model.LastUpdateDate = DateTime.Now;
                model.LastUpdateUser = Cookies.UserCode;

                return Update(model);
            }
            else
            {
                return 0;
            }
        }

        internal object GetFunctionGroupList(QueryFunctionGroupDto query)
        {
            var b = Entities;
            if (!string.IsNullOrEmpty(query.Name))
            {
                b = b.Where(o => o.Name.Contains(query.Name));
            }

            var list = b.Select(o => new
            {
                Id = o.Id,
                Name = o.Name,
                UserName = (from u in this.DbContext.GZ_User join O in this.DbContext.GZ_FunctionGroup on u.FunctionGroupId equals O.Id where u.FunctionGroupId == o.Id select u.Name)
            }).ToList();

            return list;
        }

        internal object GetOneFunctionGroup(Guid id)
        {
            var one_list = Entities.Where(o => o.Id == id).FirstOrDefault();
            return one_list;
        }

        internal int DelFunctionGroup(Guid id)
        {
            var model = Entities.Where(o => o.Id == id).FirstOrDefault();
            if (model != null)
            {
                return Delete(model);
            }
            else
            {
                return 0;
            }
        }


    }
}