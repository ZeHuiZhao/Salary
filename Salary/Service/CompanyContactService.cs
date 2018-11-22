using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary.Common;
using Salary.Data;
using Salary.DataModel.Entity;
using Salary.Models.CompanyContact;

namespace Salary.Service
{
    public class CompanyContactService : EFRepository<YY_CompanyContact>
    {
        internal object GetCompanyContactList(CompanyContactQueryDto query)
        {
            var model = new QueryCompanyContactPaging();
            try
            {
                int skip = query.PageSize * (query.PageIndex - 1);
                var a = Entities;

                if (query.SalesId.HasValue)
                {
                    a = a.Where(o => o.Company.SalesId == query.SalesId);
                }
                if (!string.IsNullOrEmpty(query.CompanyName))
                {
                    a = a.Where(o => o.Company.CompanyName.Contains(query.CompanyName));
                }
                if (!string.IsNullOrEmpty(query.ContactPhone))
                {
                    a = a.Where(o => o.ContactPhone.Contains(query.ContactPhone));
                }
                if (!string.IsNullOrEmpty(query.ContactName))
                {
                    a = a.Where(o => o.ContactName.Contains(query.ContactName));
                }

                var list = a.OrderByDescending(o => o.CreateTime);
                model.CurrentPage = query.PageIndex;
                model.PageSize = query.PageSize;
                model.TotalCount = list.Count();
                model.TotalPage = (int)Math.Ceiling(model.TotalCount * 1.0 / model.PageSize);
                var b = list.Skip(skip).Take(query.PageSize).ToList().Select((o, i) => new CompanyContactDto { SalesId=o.Company.SalesId, CId=o.CId, RowNum = ++i + query.PageSize * (query.PageIndex - 1), Id = o.Id, QQNum = o.QQNum, Email = o.Email, CompanyName = UnitOfWork.context.Set<YY_Company>().Where(c => c.Id == o.CId).FirstOrDefault()?.CompanyName, ContactJob = o.ContactJob, ContactName = o.ContactName, ContactPhone = o.ContactPhone, IsFirst = o.IsFirst, TrueName = UnitOfWork.context.Set<YY_Userinfo>().Where(u => u.Id == o.Company.SalesId).FirstOrDefault().TrueName, WechatNum = o.WechatNum, LastTime = UnitOfWork.context.Set<YY_ContactRecord>().Where(cr => cr.ContactId == o.Id).OrderByDescending(cr => cr.CreateTime).FirstOrDefault()?.ContactTime.ToString("yyyy-MM-dd") }).ToList();
                for (int i = 0; i < b.Count; i++)
                {
                    if (string.IsNullOrEmpty(b[i].LastTime))
                    {
                        b[i].LastTime = string.Empty;
                    }
                    if (string.IsNullOrEmpty(b[i].QQNum))
                    {
                        b[i].QQNum = string.Empty;
                    }
                    if (string.IsNullOrEmpty(b[i].WechatNum))
                    {
                        b[i].WechatNum = string.Empty;
                    }
                    if (string.IsNullOrEmpty(b[i].Email))
                    {
                        b[i].Email = string.Empty;
                    }
                }
                model.CompanyContactList = b;
                return model;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return model;
            }
        }

        internal object GetCompanyContactListNotPager(int id)
        {
            try
            {
                var a = Entities.Where(o => o.CId == id);
                var list = a.ToList().Select((o, i) => new CompanyContactDto { CId=o.CId, RowNum = ++i, Email = o.Email, QQNum = o.QQNum, Id = o.Id, CompanyName = UnitOfWork.context.Set<YY_Company>().Where(c => c.Id == o.CId).FirstOrDefault()?.CompanyName, ContactJob = o.ContactJob, ContactName = o.ContactName, ContactPhone = o.ContactPhone, IsFirst = o.IsFirst, TrueName = UnitOfWork.context.Set<YY_Userinfo>().Where(u => u.Id == o.Company.SalesId).FirstOrDefault().TrueName, WechatNum = o.WechatNum, LastTime = UnitOfWork.context.Set<YY_ContactRecord>().Where(cr => cr.ContactId == o.Id).OrderByDescending(cr => cr.CreateTime).FirstOrDefault()?.ContactTime.ToString("yyyy-MM-dd") }).ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    if (string.IsNullOrEmpty(list[i].LastTime))
                    {
                        list[i].LastTime = string.Empty;
                    }
                    if (string.IsNullOrEmpty(list[i].QQNum))
                    {
                        list[i].QQNum = string.Empty;
                    }
                    if (string.IsNullOrEmpty(list[i].WechatNum))
                    {
                        list[i].WechatNum = string.Empty;
                    }
                    if (string.IsNullOrEmpty(list[i].Email))
                    {
                        list[i].Email = string.Empty;
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }

        internal object GetCompanyContactById(int id)
        {
            try
            {
                return Entities.Where(o => o.Id == id).Select(o => new { o.Id, o.QQNum, WechatNum = !string.IsNullOrEmpty(o.WechatNum) ? o.WechatNum : string.Empty, o.IsFirst, IdCard = !string.IsNullOrEmpty(o.IdCard) ? o.IdCard : string.Empty, o.ContactPhone, o.ContactName, o.ContactJob, Email = !string.IsNullOrEmpty(o.Email)? o.Email:string.Empty }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }

        internal int DeleteCompanyContact(int id)
        {
            try
            {
                return Delete(id);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal int AddCompanyContact(CompanyContactCreateDto create)
        {
            try
            {
                if (create.IsFirst == 1)
                {
                    var list = Entities.Where(o => o.IsFirst == 1).ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].IsFirst = 0;
                    }
                    Update(list);
                }
                var model = new YY_CompanyContact();
                model.IdCard = create.IdCard;
                model.CId = create.CId;
                model.ContactJob = create.ContactJob;
                model.ContactName = create.ContactName;
                model.ContactPhone = create.ContactPhone;
                model.Email = create.Email;
                model.IsFirst = create.IsFirst;
                model.QQNum = create.QQNum;
                model.CreateTime = DateTime.Now;
                model.CreateUser = Cookies.UserId;
                model.WechatNum = create.WechatNum;
                return Insert(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal int UpdateCompanyContact(CompanyContactUpdateDto update)
        {
            try
            {
                if (update.IsFirst == 1)
                {
                    var list = Entities.Where(o => o.IsFirst == 1).ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].IsFirst = 0;
                    }
                    Update(list);
                }
                var model = Entities.Where(o => o.Id == update.Id).FirstOrDefault();
                if (model == null) return 0;
                model.IdCard = update.IdCard;
                model.CId = update.CId;
                model.ContactJob = update.ContactJob;
                model.ContactName = update.ContactName;
                model.ContactPhone = update.ContactPhone;
                model.Email = update.Email;
                model.IsFirst = update.IsFirst;
                model.QQNum = update.QQNum;
                model.UpdateTime = DateTime.Now;
                model.UpdateUser = Cookies.UserId;
                model.WechatNum = update.WechatNum;
                return Update(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal int SetFirstContact(int id)
        {
            try
            {

                var list = Entities.Where(o => o.IsFirst == 1).ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].IsFirst = 0;
                }
                Update(list);
                var model = Entities.Where(o => o.Id == id).FirstOrDefault();
                if (model == null) return 0;
                model.IsFirst = 1;
                return Update(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }
    }
}