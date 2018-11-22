using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary.Common;
using Salary.Data;
using Salary.DataModel.Entity;
using Salary.Models.Company;

namespace Salary.Service
{
    public class CompanyService : EFRepository<YY_Company>
    {
        public int AddComapny(YY_Company model, YY_CompanyContact contact)
        {
            try
            {
                int result = Insert(model);
                if (model.Id > 0)
                {
                    contact.CId = model.Id;
                    UnitOfWork.context.Set<YY_CompanyContact>().Add(contact);
                    UnitOfWork.context.SaveChanges();
                }
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return 0;
            }
        }

        internal object GetCompanyList(CompanyQueryDto query)
        {
            var model = new QueryCompanyPaging();
            try
            {
                int skip = query.PageSize * (query.PageIndex - 1);
                var a = Entities;

                if (query.SalesId.HasValue && query.SalesId > 0)
                {
                    a = a.Where(o => o.SalesId == query.SalesId.Value);
                }
                if (!string.IsNullOrEmpty(query.CompanyName))
                {
                    a = a.Where(o => o.CompanyName.Contains(query.CompanyName));
                }
                if (query.ChannelType == ChannelTypeEnum.Mine)
                {
                    if ((int)UserTypeEnum.ChannelManager == Cookies.UserType)
                    {
                        var dbUser = UnitOfWork.context.Set<YY_Userinfo>();
                        a = a.Where(o => dbUser.Where(u => u.Id == o.SalesId).FirstOrDefault().ChannelId == Cookies.ChannelId);
                    }
                    else
                    {
                        a = a.Where(o => o.SalesId == Cookies.UserId);
                    }
                }
                if (query.ChannelType == ChannelTypeEnum.Public)
                {
                    a = a.Where(o => o.SalesId == 0);
                }
                if (query.ChannelType == ChannelTypeEnum.Recycle)
                {
                    a = a.Where(o => o.SalesId == -1);
                }

                //if ((int)UserTypeEnum.Sales == Cookies.UserType)
                //{
                //    a = a.Where(o => o.SalesId == Cookies.UserId);
                //}

                var list = a.OrderByDescending(o => o.CreateTime);
                model.CurrentPage = query.PageIndex;
                model.PageSize = query.PageSize;
                model.TotalCount = list.Count();
                model.TotalPage = (int)Math.Ceiling(model.TotalCount * 1.0 / model.PageSize);

                var ccList = list.Skip(skip).Take(query.PageSize).ToList().Select((o, i) => new ComapnyDto { IsNew = o.AssignTime.HasValue && o.AssignTime.Value.AddDays(1) > DateTime.Now ? 1 : 0, RowNum = ++i + query.PageSize * (query.PageIndex - 1), Id = o.Id, CompanyName = o.CompanyName, ContactName = o.CompanyContact.Where(cc => cc.CId == o.Id && cc.IsFirst == 1).FirstOrDefault()?.ContactName, ContactPhone = o.CompanyContact.Where(cc => cc.CId == o.Id && cc.IsFirst == 1).FirstOrDefault()?.ContactPhone, ContactJob = o.CompanyContact.Where(cc => cc.CId == o.Id && cc.IsFirst == 1).FirstOrDefault()?.ContactJob, Industry = o.Industry, TrueName = UnitOfWork.context.Set<YY_Userinfo>().Where(u => u.Id == o.SalesId).FirstOrDefault()?.TrueName, SourceType = ((SourceTypeEnum)o.SourceType).GetDescription(), CreateTime = o.CreateTime.ToString("yyyy-MM-dd") }).ToList();
                for (int i = 0; i < ccList.Count; i++)
                {
                    if (string.IsNullOrEmpty(ccList[i].ContactName))
                        ccList[i].ContactName = string.Empty;
                    if (string.IsNullOrEmpty(ccList[i].ContactPhone))
                        ccList[i].ContactPhone = string.Empty;
                    if (string.IsNullOrEmpty(ccList[i].ContactJob))
                        ccList[i].ContactJob = string.Empty;
                    if (string.IsNullOrEmpty(ccList[i].TrueName))
                        ccList[i].TrueName = string.Empty;
                }
                model.CompanyList = ccList;
                return model;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return model;
            }
        }

        internal int UpdateComapny(CompanyUpdateDto update)
        {
            try
            {
                var model = Entities.Where(o => o.Id == update.Id).FirstOrDefault();
                if (model != null)
                {
                    model.Address = update.Address;
                    model.City = update.City;
                    model.CompanyName = update.CompanyName;
                    model.CompanySize = update.CompanySize;
                    model.EstablishedTime = !string.IsNullOrEmpty(update.EstablishedTime) ? Convert.ToDateTime(update.EstablishedTime) : (DateTime?)null;
                    model.Industry = update.Industry;
                    model.Province = update.Province;
                    model.SalesId = update.SalesId;
                }
                return Update(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal object GetCompanyById(int id)
        {
            try
            {
                var model = Entities.Where(o => o.Id == id).FirstOrDefault();
                if (model != null)
                {
                    var dto = new CompanyUpdateDto
                    {
                        Address = !string.IsNullOrEmpty(model.Address) ? model.Address : string.Empty,
                        City = !string.IsNullOrEmpty(model.City) ? model.City : string.Empty,
                        CompanyName = !string.IsNullOrEmpty(model.CompanyName) ? model.CompanyName : string.Empty,
                        CompanySize = !string.IsNullOrEmpty(model.CompanySize) ? model.CompanySize : string.Empty,
                        EstablishedTime = model.EstablishedTime?.ToString("yyyy-MM-dd"),
                        Id = model.Id,
                        Industry = !string.IsNullOrEmpty(model.Industry) ? model.Industry : string.Empty,
                        Province = !string.IsNullOrEmpty(model.Province) ? model.Province : string.Empty,
                        SalesId = model.SalesId,
                        SourceType = ((SourceTypeEnum)model.SourceType).GetDescription()
                    };
                    return dto;
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }

        internal int AddComapny(CompanyCreateDto create)
        {
            try
            {
                var model = new YY_Company { CreateTime = DateTime.Now, Address = create.Address, AssignTime = create.AssignTime, City = create.City, CompanyName = create.CompanyName, CompanySize = create.CompanySize, CreateUser = Cookies.UserId, EstablishedTime = create.EstablishedTime, Industry = create.Industry, Province = create.Province, SalesId = create.SalesId, SourceType = create.SourceType };
                return Insert(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal int ToggleCompany(ToggleCompany toggle)
        {
            try
            {
                List<YY_Company> list = new List<YY_Company>();
                for (int i = 0; i < toggle.Ids.Count; i++)
                {
                    int id = toggle.Ids[i];
                    var model = Entities.Where(o => o.Id == id).FirstOrDefault();
                    if (model != null)
                    {
                        model.SalesId = toggle.SalesId;
                        if (toggle.SalesId > 0) model.AssignTime = DateTime.Now;
                        list.Add(model);
                    }
                }
                return Update(list);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        /// <summary>
        /// 切换客户到公海或回收站
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="isPublic">0表示回收站，1表示公海</param>
        /// <returns></returns>
        internal int ToggleCompany(List<int> ids, int isPublic)
        {
            try
            {
                List<YY_Company> list = new List<YY_Company>();
                for (int i = 0; i < ids.Count; i++)
                {
                    int id = ids[i];
                    var model = Entities.Where(o => o.Id == id).FirstOrDefault();
                    if (model != null)
                    {
                        if (isPublic == 1)
                        {
                            model.SalesId = 0;
                        }
                        else
                        {
                            model.SalesId = -1;
                        }
                        list.Add(model);
                    }
                }
                return Update(list);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }
    }
}