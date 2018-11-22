using Salary.Common;
using Salary_MVC.Data;
using Salary_MVC.DataModel;
using Salary_MVC.Models;
using Salary_MVC.Models.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Services
{
    public class FunctionService : Service<DataModel.GZ_Function>
    {

        internal int AddFunction(FunctionCreateDto create)
        {
            GZ_Function model = new GZ_Function() { Id = new Guid(Guid.NewGuid().ToString("N")), CreateDate = DateTime.Now, CreateUser = Cookies.UserCode, LastUpdateDate = DateTime.Now, LastUpdateUser = Cookies.UserCode };
            model.Name = create.Name;
            model.URL = create.URL;
            model.Order = create.Order;
            model.ParentId = create.ParentId;
            model.Ico = create.Ico;
            this.DbContext.GZ_Function.Add(model);
            return this.DbContext.SaveChanges();
        }

        internal object GetFunctionName()
        {
            var list = this.DbContext.GZ_Function.Where(o => o.ParentId == null).ToList();
            return list;
        }

        internal object GetFunctionList()
        {

            var list = Entities.OrderBy(o => o.Order).Select(o => new
            {
                Id = o.Id,
                Name = o.Name,
                URL = o.URL??string.Empty,
                ParentId = Entities.Where(i => i.Id == o.ParentId).FirstOrDefault().Name ?? string.Empty,
                Order = o.Order,
                Ico = o.Ico ?? string.Empty
            }).ToList();

            return list;
        }

        internal object GetOneFunction(Guid id)
        {
            var one_list = Entities.Where(o => o.Id == id).FirstOrDefault();
            return one_list;
        }

        internal int UpdateFunction(FunctionUpdateDto Update)
        {
            GZ_Function model = Entities.Where(o => o.Id == Update.Id).FirstOrDefault();

            if (model != null)
            {
                model.ParentId = Update.ParentId;
                model.Name = Update.Name;
                model.URL = Update.URL;
                model.Ico = Update.Ico;
                model.Order = Update.Order;
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

        internal int DelFunction(Guid id)
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

    }
}