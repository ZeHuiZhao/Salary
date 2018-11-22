using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary.Common;
using Salary.Data;
using Salary.DataModel.Entity;
using Salary.Models.Function;

namespace Salary.Service
{
    public class FunctionService : EFRepository<YY_Function>
    {
        internal List<FuncModel> GetFunctionList(FuncQueryDto queryModel)
        {
            try
            {
                var a = Entities;
                if (queryModel.FuncType == 1)
                {
                    a = a.Where(o => o.ParentId == 0);
                }
                else if (queryModel.FuncType == 2)
                {
                    a = a.Where(o => o.ParentId != 0);
                }
                else
                {
                    return new List<FuncModel>();
                }

                a = a.OrderBy(o => o.DisOrder);
                var list = from f in a
                           select new FuncModel { Id = f.Id, Icon = f.Icon, Name = f.Name, Url = f.Url, DisOrder = f.DisOrder, Enable = f.Enable, ParentId = f.ParentId };

                return list.ToList();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return new List<FuncModel>();
            }

        }

        internal int AddFunction(FuncCreateDto create)
        {
            try
            {
                var model = new YY_Function { Url = create.Url, DisOrder = create.DisOrder, Enable = create.Enable, Icon = create.Icon, Name = create.Name, ParentId = create.ParentId };
                if (!string.IsNullOrEmpty(Cookies.UserCode))
                {
                    model.CreateUser = Convert.ToInt32(Cookies.UserCode);
                }
                else
                {
                    model.CreateUser = 0;
                }
                model.CreateTime = DateTime.Now;
                return Insert(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal FuncModel GetFunctionById(int id)
        {
            try
            {
                return Entities.Where(o => o.Id == id).Select(f => new FuncModel { Id = f.Id, Icon = f.Icon, Name = f.Name, Url = f.Url, DisOrder = f.DisOrder, Enable = f.Enable, ParentId = f.ParentId }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return new FuncModel();
            }

        }

        internal int UpdateFunc(FuncUpdateDto update)
        {
            try
            {
                var model = Entities.Where(o => o.Id == update.Id).FirstOrDefault();
                model.Name = update.Name;
                model.Url = update.Url;
                model.ParentId = update.ParentId;
                model.Icon = update.Icon;
                model.Enable = update.Enable;
                model.DisOrder = update.DisOrder;

                if (!string.IsNullOrEmpty(Cookies.UserCode))
                {
                    model.UpdateUser = Convert.ToInt32(Cookies.UserCode);
                }
                model.UpdateTime = DateTime.Now;
                return Update(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal object GetMenu()
        {
            try
            {
                if (Cookies.UserType>0)
                {
                    string navigateId = UnitOfWork.context.Set<YY_Role>().Find(Cookies.UserType)?.NavigateId;
                    string[] strs = navigateId.Split(',');
                    List<int> ids = Entities.Where(o => strs.Contains(o.Id.ToString())).GroupBy(o => o.ParentId).Select(o => o.Key).ToList();
                    var a = Entities.Where(o => o.Enable == 1 && o.ParentId == 0 && ids.Contains(o.Id)).ToList().Select(o => new { id = o.Id, text = o.Name, icon = o.Icon, children = Entities.Where(b => b.Enable == 1 && b.ParentId == o.Id && strs.Contains(b.Id.ToString())).Select(c => new { id = c.Id, text = c.Name, url = c.Url, targetType = "iframe-tab" }).ToList() });
                    return a.ToList();
                    //var a = Find(o => o.Enable == 1 && o.ParentId == 0).OrderBy(o => o.DisOrder).Select(o => new { id = o.Id, text = o.Name, icon = o.Icon, children = Find(b => b.Enable == 1 && b.ParentId == o.Id).OrderBy(n => n.DisOrder).Select(c => new { id = c.Id, text = c.Name, url = c.Url, targetType = "iframe-tab" }).ToList() });
                    //return a.ToList();
                }
                return null;
            }
            catch (NullReferenceException ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return 1;
            }
        }

        internal object GetPrivilegeList()
        {
            try
            {
                var a = Entities.Where(o => o.Enable == 1 && o.ParentId == 0).Select(o => new { id = o.Id, text = o.Name, children = Entities.Where(b => b.Enable == 1 && b.ParentId == o.Id).Select(c => new { id = c.Id, text = c.Name }).ToList() });
                return a.ToList();

            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }

        internal object GetParentFunc()
        {
            try
            {
                var a = Entities.Where(o => o.Enable == 1 && o.ParentId == 0).Select(o => new { Id = o.Id, Name = o.Name });
                return a.ToList();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }


    }
}