using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary.Common;
using Salary.Data;
using Salary.DataModel.Entity;
using Salary.Models.ComapnyRecord;
using Salary.Models.CompanyRecord;

namespace Salary.Service
{
    public class ContactRecordService : EFRepository<YY_ContactRecord>
    {
        internal int AddComapnyRecord(CompanyRecordCreateDto create)
        {
            try
            {
                YY_ContactRecord model = new YY_ContactRecord { CId = UnitOfWork.context.Set<YY_CompanyContact>().Where(o => o.Id == create.ContactId).FirstOrDefault().CId, ContactId = create.ContactId, ContactTime = create.ContactTime, ContactSummary = create.ContactSummary, CreateTime = DateTime.Now, CreateUser = Cookies.UserId, SalesId = create.SalesId };
                return Insert(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal object GetCompanyRecordList(CompanyRecordQueryDto query)
        {
            var model = new QueryContactRecordPaging();
            try
            {
                int skip = query.PageSize * (query.PageIndex - 1);
                var a = Entities;

                if (query.SalesId.HasValue)
                {
                    a = a.Where(o => o.SalesId == query.SalesId);
                }
                if (!string.IsNullOrEmpty(query.CompanyName))
                {
                    a = a.Where(o => o.Company.CompanyName.Contains(query.CompanyName));
                }
                if (!string.IsNullOrEmpty(query.ContactPhone))
                {
                    a = a.Where(o => UnitOfWork.context.Set<YY_CompanyContact>().Where(cc => o.ContactId == cc.Id).FirstOrDefault().ContactPhone.Contains(query.ContactPhone));
                }
                if (!string.IsNullOrEmpty(query.ContactName))
                {
                    a = a.Where(o => UnitOfWork.context.Set<YY_CompanyContact>().Where(cc => o.ContactId == cc.Id).FirstOrDefault().ContactName.Contains(query.ContactName));
                }

                var list = a.OrderByDescending(o => o.CreateTime);
                model.CurrentPage = query.PageIndex;
                model.PageSize = query.PageSize;
                model.TotalCount = list.Count();
                model.TotalPage = (int)Math.Ceiling(model.TotalCount * 1.0 / model.PageSize);
                model.CompanyRecordList = list.Skip(skip).Take(query.PageSize).ToList().Select((o, i) => new CompanyRecordDto { IsAction = o.CreateTime.AddDays(1) > DateTime.Now ? 1 : 0, DisplayName = "添加之后24小时内才能操作", RowNum = ++i + query.PageSize * (query.PageIndex - 1), Id = o.Id, CompanyName = o.Company.CompanyName, ContactName = UnitOfWork.context.Set<YY_CompanyContact>().Where(cc => cc.Id == o.ContactId).FirstOrDefault()?.ContactName, ContactSummary = o.ContactSummary, ContactTime = o.ContactTime.ToString("yyyy-MM-dd hh:mm"), CurrentTrueName = UnitOfWork.context.Set<YY_Userinfo>().Where(u => u.Id == o.Company.SalesId).FirstOrDefault()?.TrueName, TrueName = o.Userinfo.TrueName }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return model;
            }
        }

        internal int UpdateComapnyRecord(CompanyRecordUpdateDto update)
        {
            try
            {
                var model = Entities.Where(o => o.Id == update.Id).FirstOrDefault();
                if (model == null) return 0;
                if (model.CreateTime.AddDays(1) < DateTime.Now)
                    return -1;
                model.ContactSummary = update.ContactSummary;
                return Update(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal int DeleteCompanyRecord(int id)
        {
            try
            {
                var model = Entities.Where(o => o.Id == id).FirstOrDefault();
                if (model == null) return 0;
                if (model.CreateTime.AddDays(1) < DateTime.Now)
                    return -1;
                return Delete(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal object GetCompanyRecordById(int id)
        {
            try
            {
                //cc.Where(c => c.Id == o.ContactId).FirstOrDefault().ContactPhone}(cc.Where(c => c.Id == o.ContactId).FirstOrDefault().ContactName})
                var cc = UnitOfWork.context.Set<YY_CompanyContact>();
                return Entities.Where(o => o.Id == id).ToList().Select(o => new { o.Id, o.ContactSummary, DisplayName = $"{cc.Where(c => c.Id == o.ContactId).FirstOrDefault().ContactPhone}({cc.Where(c => c.Id == o.ContactId).FirstOrDefault().ContactName})" }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }
    }
}