using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using Salary.Common;
using Salary.Data;
using Salary.DataModel.Entity;
using Salary.Models;
using Salary.Models.Enroll;

namespace Salary.Service
{
    public class EnrollService : EFRepository<YY_Enroll>
    {
        private readonly UserService _user = new UserService();
        internal object GetEnrollList(QueryEnrollDto query)
        {
            var model = new QueryEnrollPaging();
            try
            {
                int skip = query.PageSize * (query.PageIndex - 1);
                var a = Entities.Where(o => o.ChannelId == Cookies.ChannelId);
                if (query.EnrollStatus != EnrollStatusEnum.All)
                {
                    a = a.Where(o => o.EnrollStatus == (int)query.EnrollStatus);
                }
                if (Cookies.UserType == (int)UserTypeEnum.Sales)
                {
                    a = a.Where(o => o.SalesId == Cookies.UserId);
                }

                if (!string.IsNullOrEmpty(query.CompanyName))
                {
                    a = a.Where(o => o.CompanyName.Contains(query.CompanyName));
                }

                if (query.MaterialType.HasValue)
                {
                    a = a.Where(o => o.Material.MTId == query.MaterialType.Value);
                }

                if (query.Salesman.HasValue)
                {
                    a = a.Where(o => o.SalesId == query.Salesman.Value);
                }

                if (query.ActivityId.HasValue)
                {
                    a = a.Where(o => o.IsActivity == query.ActivityId.Value);
                }

                var list = a.OrderByDescending(o => o.CreateTime);
                model.CurrentPage = query.PageIndex;
                model.PageSize = query.PageSize;
                model.TotalCount = list.Count();
                model.TotalPage = (int)Math.Ceiling(model.TotalCount * 1.0 / model.PageSize);
                model.EnrollList = list.Skip(skip).Take(query.PageSize).ToList().Select((o, i) => new EnrollModel { RowNum = ++i + query.PageSize * (query.PageIndex - 1), Id = o.Id, CompanyName = o.CompanyName, ClassTime = o.ClassTime, ContactJob = o.ContactJob, ContactName = o.ContactName, ContactPhone = o.ContactPhone, EnrollStatus = o.EnrollStatus, EnrollStatusName = ((EnrollStatusEnum)o.EnrollStatus).GetDescription(), SalesName = o.User.TrueName, AuditOpinion = o.EnrollAudit.Where(cc => cc.EnrollId == o.Id).OrderByDescending(cc => cc.Id).FirstOrDefault()?.AuditOpinion, AuditTime = o.EnrollAudit.Where(cc => cc.EnrollId == o.Id).OrderByDescending(cc => cc.Id).FirstOrDefault()?.AuditTime.ToString("yyyy-MM-dd") }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return model;
            }
        }

        internal int AddEnroll(CreateEnrollDto create)
        {
            try
            {
                var user = _user.GetSalesUser(create.OpenId);
                if (user == null)
                {
                    return 0;
                }
                var model = new YY_Enroll { ChannelId = create.ChannelId, ClassTime = create.ClassTime, CompanyName = create.CompanyName, ContactJob = create.ContactJob, ContactName = create.ContactName, ContactPhone = create.ContactPhone, CreateTime = DateTime.Now, CreateUser = Convert.ToInt32(Cookies.UserCode), EnrollStatus = (int)EnrollStatusEnum.ChannelUnApproved, IsActivity = create.IsActivity, MId = create.MId, TCId = 0, SalesId = user.Id };

                int result = Insert(model);
                if (result > 0)
                {
                    YY_Company company = new YY_Company { CompanyName = model.CompanyName, CreateTime = DateTime.Now, CreateUser = user.Id, SalesId = user.Id, SourceType = (int)SourceTypeEnum.Wechat };
                    YY_CompanyContact companyContact = new YY_CompanyContact { CreateTime = DateTime.Now, CreateUser = user.Id, ContactName = model.ContactName, ContactJob = model.ContactJob, ContactPhone = model.ContactPhone, IsFirst = 1 };
                    new CompanyService().AddComapny(company, companyContact);
                }
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal object GetClassTime(int materialType)
        {
            var model = new MaterialTypeService().GetByKey(materialType);
            if (model == null) return null;
            var classTime = new ClassTime();
            string sql;
            if (model.ErpYsCode == "01")
            {
                sql = "SELECT top 4 case DATEDIFF(MONTH ,CPS_BEGIN_DATE,CPS_END_DATE)  when 0 then CONVERT(CHARACTER(2),[CPS_BEGIN_DATE],22)+'月'+CONVERT(CHARACTER(2),[CPS_BEGIN_DATE],103)+'日'+'-'+CONVERT(CHARACTER(2),[CPS_END_DATE],105)+'日'+'('+ISNULL(CPS_REGION,'') +')'   else CONVERT(CHARACTER(2),[CPS_BEGIN_DATE],22)+'月'+CONVERT(CHARACTER(2),[CPS_BEGIN_DATE],103)+'日'+'-'+CONVERT(CHARACTER(2),[CPS_END_DATE],22)+'月'+CONVERT(CHARACTER(2),[CPS_END_DATE],105)+'日'+'('+ISNULL(CPS_REGION,'') +')'   end AS time   FROM dbo.AP_STATUS  INNER JOIN [AP_COURSE_PlAN_SECTION] ON [CPS_SECTION_TYPE]=YS_CODE WHERE YS_TYPE='CPS_SECTION_TYPE' AND YS_CODE in('01','11','12','13','14','15') AND [CPS_BEGIN_DATE]>GETDATE() order by CPS_BEGIN_DATE";
            }
            else
            {
                sql = "SELECT top 4 case DATEDIFF(MONTH ,CPS_BEGIN_DATE,CPS_END_DATE) when 0 then CONVERT(CHARACTER(2),[CPS_BEGIN_DATE],22)+'月'+CONVERT(CHARACTER(2),[CPS_BEGIN_DATE],103)+'日'+'-'+CONVERT(CHARACTER(2),[CPS_END_DATE],105)+'日'  else CONVERT(CHARACTER(2),[CPS_BEGIN_DATE],22)+'月'+CONVERT(CHARACTER(2),[CPS_BEGIN_DATE],103)+'日'+'-'+CONVERT(CHARACTER(2),[CPS_END_DATE],22)+'月'+CONVERT(CHARACTER(2),[CPS_END_DATE],105)+'日' end AS time   FROM dbo.AP_STATUS  INNER JOIN [AP_COURSE_PlAN_SECTION] ON [CPS_SECTION_TYPE]=YS_CODE WHERE YS_TYPE='CPS_SECTION_TYPE' AND YS_CODE='02' AND [CPS_BEGIN_DATE]>GETDATE() order by CPS_BEGIN_DATE";
            }
            var list = UnitOfWork.context.Database.SqlQuery<string>(sql).ToList();
            classTime.Time = list;
            classTime.CoverImg = model.CoverImg;
            return classTime;
        }

        internal int UpdateEnroll(UpdateEnrollDto update)
        {
            try
            {
                var model = Entities.Where(o => o.Id == update.Id).FirstOrDefault();
                if (model != null)
                {
                    model.ContactJob = update.ContactJob;
                    model.CompanyName = update.CompanyName;
                    model.ContactName = update.ContactName;
                    model.ContactPhone = update.ContactPhone;
                    return Update(model);
                }
                return 0;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return 0;
            }
        }

        internal object GetEnrollById(int id)
        {
            try
            {
                return Entities.Where(o => o.Id == id).Select(o => new { o.Id, o.CompanyName, o.ContactName, o.ContactPhone, o.ContactJob }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return null;
            }
        }

        internal object GetEnrollResultList(QueryWechatEnrollResult query)
        {
            try
            {
                var entity = _user.GetSalesUser(query.OpenId);
                if (entity == null) return null;
                int skip = query.PageSize * (query.PageIndex - 1);
                var a = Entities.Where(o => o.ChannelId == entity.ChannelId);
                WechatEnrollDto model = new WechatEnrollDto();
                var user = _user.GetSalesUser(query.OpenId);
                if (user == null) return null;
                a = a.Where(o => o.SalesId == user.Id);
                if (query.MTId.HasValue)
                {
                    a = a.Where(o => o.Material.MTId == query.MTId.Value);
                }
                if (!string.IsNullOrEmpty(query.GlobalSearch))
                {
                    a = a.Where(o => o.CompanyName.Contains(query.GlobalSearch) || o.ContactName.Contains(query.GlobalSearch) || o.ContactPhone.Contains(query.GlobalSearch));
                }
                var list = a.OrderByDescending(o => o.CreateTime);
                model.CurrentPage = query.PageIndex;
                model.PageSize = query.PageSize;
                model.TotalCount = list.Count();
                model.TotalPage = (int)Math.Ceiling(model.TotalCount * 1.0 / model.PageSize);
                model.EnrollList = list.Skip(skip).Take(query.PageSize).ToList().Select(o => new WechatEnrollModel { CompanyName = o.CompanyName, ContactName = o.ContactName, ContactPhone = o.ContactPhone, CreateTime = o.CreateTime.ToString("yyyy-MM-dd"), MaterialTypeName = o.Material.MaterialTypes.TypeName }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return null;
            }
        }

        internal object GetEnrollParticipateList(QueryWechatEnrollResult query)
        {
            try
            {
                var entity = _user.GetSalesUser(query.OpenId);
                if (entity == null) return null;
                int skip = query.PageSize * (query.PageIndex - 1);
                var a = Entities.Where(o => o.ChannelId == entity.ChannelId && o.EnrollStatus >= (int)EnrollStatusEnum.NotParticipate);
                WechatEnrollDto model = new WechatEnrollDto();
                var user = _user.GetSalesUser(query.OpenId);
                if (user == null) return null;
                a = a.Where(o => o.SalesId == user.Id);
                if (query.MTId.HasValue)
                {
                    a = a.Where(o => o.Material.MTId == query.MTId.Value);
                }
                if (!string.IsNullOrEmpty(query.GlobalSearch))
                {
                    a = a.Where(o => o.CompanyName.Contains(query.GlobalSearch) || o.ContactName.Contains(query.GlobalSearch) || o.ContactPhone.Contains(query.GlobalSearch));
                }
                var list = a.OrderByDescending(o => o.CreateTime);
                model.CurrentPage = query.PageIndex;
                model.PageSize = query.PageSize;
                model.TotalCount = list.Count();
                model.TotalPage = (int)Math.Ceiling(model.TotalCount * 1.0 / model.PageSize);
                model.EnrollList = list.Skip(skip).Take(query.PageSize).ToList().Select(o => new WechatEnrollModel { CompanyName = o.CompanyName, ContactName = o.ContactName, ContactPhone = o.ContactPhone, CreateTime = o.CreateTime.ToString("yyyy-MM-dd"), MaterialTypeName = o.Material.MaterialTypes.TypeName, EnrollStatusName = ((EnrollStatusEnum)o.EnrollStatus).GetDescription() }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return null;
            }
        }

        internal object GetEnrollAuditResult(QueryWechatEnrollResult query)
        {
            try
            {
                var entity = _user.GetSalesUser(query.OpenId);
                if (entity == null) return null;
                int skip = query.PageSize * (query.PageIndex - 1);
                var a = Entities.Where(o => o.ChannelId == entity.ChannelId && o.EnrollStatus < (int)EnrollStatusEnum.NotParticipate);
                WechatEnrollDto model = new WechatEnrollDto();
                var user = _user.GetSalesUser(query.OpenId);
                if (user == null) return null;
                a = a.Where(o => o.SalesId == user.Id);
                if (query.MTId.HasValue)
                {
                    a = a.Where(o => o.Material.MTId == query.MTId.Value);
                }
                if (!string.IsNullOrEmpty(query.GlobalSearch))
                {
                    a = a.Where(o => o.CompanyName.Contains(query.GlobalSearch) || o.ContactName.Contains(query.GlobalSearch) || o.ContactPhone.Contains(query.GlobalSearch));
                }
                var list = a.OrderByDescending(o => o.CreateTime);
                model.CurrentPage = query.PageIndex;
                model.PageSize = query.PageSize;
                model.TotalCount = list.Count();
                model.TotalPage = (int)Math.Ceiling(model.TotalCount * 1.0 / model.PageSize);
                model.EnrollList = list.Skip(skip).Take(query.PageSize).ToList().Select(o => new WechatEnrollModel { CompanyName = o.CompanyName, ContactName = o.ContactName, ContactPhone = o.ContactPhone, CreateTime = o.CreateTime.ToString("yyyy-MM-dd"), MaterialTypeName = o.Material.MaterialTypes.TypeName, EnrollStatusName = ((EnrollStatusEnum)o.EnrollStatus).GetDescription() }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return null;
            }
        }

        internal int ApprovalEnroll(AuditEnrollDto obj)
        {
            try
            {
                var enroll = Entities.Where(o => o.Id == obj.Id).FirstOrDefault();
                if (enroll != null && enroll.EnrollStatus == (int)EnrollStatusEnum.ChannelUnApproved)
                {
                    if (obj.IsPass == 1)
                    {
                        //调用erp接口
                        WebClient wc = new WebClient();
                        string url = Config.ErpAddress + "/Ajax/ajax_trainee_company.aspx?action=AddCompany&ACC_CB_ID=" + enroll.ChannelId + "&ACC_COMPANY_NAME=" + obj.CompanyName + "&ACC_NAME_FIRSTLY=" + obj.ContactName + "&ACC_POSITION_FIRSTLY=" + obj.ContactJob + "&ACC_PHONE_FIRSTLY=" + obj.ContactPhone + "&ACC_PEOPLE_NUMBER=1&ACC_REMARK=&ACC_PROVINCE=&ACC_CITY=";

                        string json = wc.DownloadString(url);
                        ErpResult erp = JsonConvert.DeserializeObject<ErpResult>(json);
                        if (erp.status != 1)
                        {
                            return 0;
                        }
                        enroll.CCId = erp.primary_id;

                    }
                    enroll.EnrollStatus = obj.IsPass == 1 ? (int)EnrollStatusEnum.ZLUnApproved : (int)EnrollStatusEnum.ChannelNoPass;
                    enroll.UpdateTime = DateTime.Now;
                    enroll.UpdateUser = Cookies.UserId;
                    enroll.CompanyName = obj.CompanyName;
                    enroll.ContactJob = obj.ContactJob;
                    enroll.ContactName = obj.ContactName;
                    enroll.ContactPhone = obj.ContactPhone;
                    UnitOfWork.RegisterModified(enroll);
                    var audit = new YY_EnrollAudit();
                    audit.AuditName = Cookies.UserName;
                    audit.AuditOpinion = obj.Opinion;
                    audit.AuditStatus = obj.IsPass == 1 ? (int)EnrollStatusEnum.ZLUnApproved : (int)EnrollStatusEnum.ChannelNoPass;
                    audit.AuditTime = DateTime.Now;
                    audit.CreateTime = DateTime.Now;
                    audit.CreateUser = Convert.ToInt32(Cookies.UserCode);
                    audit.IsSuccess = obj.IsPass;
                    audit.EnrollId = enroll.Id;
                    UnitOfWork.RegisterNew<YY_EnrollAudit>(audit);
                    return UnitOfWork.Commit();
                }
                return 0;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                UnitOfWork.Rollback();
                return 0;
            }
        }

        internal int Reapproval(AuditModel obj)
        {
            try
            {
                var enroll = Entities.Where(o => o.Id == obj.Id).FirstOrDefault();
                if (enroll != null && enroll.EnrollStatus == (int)EnrollStatusEnum.ChannelNoPass)
                {
                    enroll.EnrollStatus = (int)EnrollStatusEnum.ChannelUnApproved;
                    UnitOfWork.RegisterModified(enroll);
                    var audit = new YY_EnrollAudit();
                    audit.AuditName = Cookies.UserName;
                    audit.AuditStatus = (int)EnrollStatusEnum.ChannelUnApproved;
                    audit.AuditTime = DateTime.Now;
                    audit.CreateTime = DateTime.Now;
                    audit.CreateUser = Convert.ToInt32(Cookies.UserCode);
                    audit.IsSuccess = obj.IsPass;
                    audit.EnrollId = enroll.Id;
                    UnitOfWork.RegisterNew<YY_EnrollAudit>(audit);
                    return UnitOfWork.Commit();
                }
                return 0;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return 0;
            }
        }
    }
}