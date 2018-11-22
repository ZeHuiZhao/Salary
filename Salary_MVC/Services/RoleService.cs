using Salary.Common;
using Salary_MVC.Data;
using Salary_MVC.DataModel;
using Salary_MVC.Models.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Services
{
    public class RoleService : Service<DataModel.GZ_Role>
    {
        internal int AddRole(AddRoleDto Add)
        {
            try
            {
                GZ_Role model = new GZ_Role() { Id = new Guid(Guid.NewGuid().ToString("N")), CreateDate = DateTime.Now, CreateUser = Cookies.UserCode, LastUpdateUser = Cookies.UserCode, LastUpdateDate = DateTime.Now };

                model.Name = Add.Name;
                model.Code = Add.Code;
                this.DbContext.GZ_Role.Add(model);
                return this.DbContext.SaveChanges();
            }
            catch
            {
                //UnitOfWork.Rollback();
                return 0;
            }
        }

        internal int UpdateRole(UpdateRoleDto Edit)
        {
            GZ_Role model = Entities.Where(o => o.Id == Edit.Id).FirstOrDefault();

            if (model != null)
            {
                model.Name = Edit.Name;
                model.Code = Edit.Code;
                model.LastUpdateDate = DateTime.Now;
                model.LastUpdateUser = Cookies.UserCode;
                //UnitOfWork.RegisterModified(model);
                //return UnitOfWork.Commit();
                this.DbContext.Entry(model).State = System.Data.Entity.EntityState.Modified;
                return this.DbContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        internal object GetOneRole(Guid id)
        {
            try
            {
                var one_list = Entities.Where(o => o.Id == id).FirstOrDefault();
                return one_list;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }

        internal int DelRole(Guid id)
        {
            try
            {
                var model = Entities.Where(o => o.Id == id).FirstOrDefault();
                if (model != null)
                {
                    this.DbContext.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                    return this.DbContext.SaveChanges();
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal object QueryRole()
        {
            var list = this.DbContext.GZ_Role.OrderBy(o => o.Code).Select(o => new
            {
                Id = o.Id,
                Code = o.Code,
                Name = o.Name,
                User = (from u in this.DbContext.GZ_User join r in this.DbContext.GZ_UserRole on u.Id equals r.UserId where r.RoleId == o.Id select u.Name)
            }).ToList();
            return list;
        }



    }
}