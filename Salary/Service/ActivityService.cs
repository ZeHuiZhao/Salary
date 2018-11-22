using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary.Common;
using Salary.Data;
using Salary.DataModel.Entity;
using Salary.Models.Activity;

namespace Salary.Service
{
    public class ActivityService : EFRepository<YY_Activity>
    {
        internal object GetActivityList(ActivityQueryDto dto)
        {
            try
            {
                var model = new ActivityPaging();
                int skip = dto.PageSize * (dto.PageIndex - 1);
                var list = Entities;
                if (!string.IsNullOrEmpty(dto.ActivityName))
                {
                    list = list.Where(o=>o.ActivityName.Contains(dto.ActivityName));
                }
                if (dto.ActivityStatus != ActivityStatusEnum.All)
                {
                    if (dto.ActivityStatus == ActivityStatusEnum.NotStarted)
                    {
                        list = list.Where(o => o.StartTime > DateTime.Now);
                    }
                    else if (dto.ActivityStatus == ActivityStatusEnum.Processing)
                    {
                        list = list.Where(o=>o.StartTime<DateTime.Now&&o.EndTime>DateTime.Now);
                    }
                    else if (dto.ActivityStatus == ActivityStatusEnum.Over)
                    {
                        list = list.Where(o => o.EndTime < DateTime.Now);
                    }
                }
                var a = list.OrderByDescending(o => o.CreateTime);
                model.CurrentPage = dto.PageIndex;
                model.PageSize = dto.PageSize;
                model.TotalCount = a.Count();
                model.TotalPage = (int)Math.Ceiling(model.TotalCount * 1.0 / model.PageSize);
                model.ActivityList = a.Skip(skip).Take(dto.PageSize).ToList().Select((o,i) => new { DisplayName = "活动24小时内才能删除", IsDelete = o.CreateTime.AddDays(1) > DateTime.Now ? 1 : 0, RowNum = ++i + dto.PageSize * (dto.PageIndex - 1), Id = o.Id, o.ActivityName, StartTime=o.StartTime.ToString("yyyy-MM-dd"),EndTime = o.EndTime.ToString("yyyy-MM-dd"),o.LeadTeacher,o.Principal,o.Address,ActivityStatus = o.CreateTime>DateTime.Now?"未开始":o.CreateTime<DateTime.Now&&o.EndTime>DateTime.Now?"进行中":"已结束"}).ToList<object>();
                return model;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return null;
            }
        }

        internal object GetActivityById(int id)
        {
            try
            {
                return Entities.Where(o => o.Id == id).ToList().Select(o=>new { o.Id,o.LeadTeacher,o.ActivityName,o.Address,o.CoverImg,o.Principal,StartTime=o.StartTime.ToString("yyyy-MM-dd"),EndTime=o.EndTime.ToString("yyyy-MM-dd"),ActivityStatus= o.CreateTime > DateTime.Now ? "未开始" : o.CreateTime < DateTime.Now && o.EndTime > DateTime.Now ? "进行中" : "已结束" }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return 0;
            }
        }

        internal int UpdateActivity(ActivityUpdateDto update)
        {
            try
            {
                var model = Entities.Where(o => o.Id == update.Id).FirstOrDefault();
                if (model == null) return 0;
                model.ActivityName = update.ActivityName;
                model.Address = update.Address;
                model.EndTime = update.EndTime;
                model.StartTime = update.StartTime;
                model.LeadTeacher = update.LeadTeacher;
                model.Principal = update.Principal;
                model.UpdateTime = DateTime.Now;
                model.UpdateUser = Cookies.UserId;
                return Update(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return 0;
            }
        }

        internal int AddActivity(ActivityCreateDto create)
        {
            try
            {
                var model = new YY_Activity { EndTime = create.EndTime, CreateTime = DateTime.Now, ActivityName = create.ActivityName, Address = create.Address, CoverImg = create.CoverImg, CreateUser = Cookies.UserId, LeadTeacher = create.LeadTeacher, Principal = create.Principal, StartTime = create.StartTime };
                return Insert(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return 0;
            }
        }

        internal int CopyActivity(int id)
        {
            try
            {
                var model = Entities.Where(o => o.Id == id).FirstOrDefault();
                if (model == null) return 0;
                var copy = new YY_Activity { ActivityName = model.ActivityName, Address = model.Address, CoverImg = model.CoverImg, CreateTime = DateTime.Now, CreateUser = Cookies.UserId, EndTime = model.EndTime, LeadTeacher = model.LeadTeacher, Principal = model.Principal, StartTime = model.StartTime };
                UnitOfWork.RegisterNew(copy);
                var materialList = UnitOfWork.context.Set<YY_Material>().Where(o => o.ActivityId == model.Id).ToList();
                foreach (var create in materialList)
                {
                    var material = new YY_Material { DisplayTime = create.DisplayTime, VirtualTimes = create.VirtualTimes, MaterialContent = create.MaterialContent, MaterialSummary = create.MaterialSummary, ActivityId = copy.Id, ChannelId = create.ChannelId, CoverImg = create.CoverImg, CreateTime = DateTime.Now, CreateUser = Cookies.UserId, IsActive = create.IsActive, MaterialTips = create.MaterialTips, MaterialTitle = create.MaterialTitle, MTId = create.MTId, MaterialType = create.MaterialType, ShareSource = create.ShareSource };
                    UnitOfWork.RegisterNew<YY_Material>(material);
                }
                return UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                UnitOfWork.Rollback();
                return 0;
            }
        }

        internal object GetActivityUseMaterialDetail(int id)
        {
            try
            {
               return UnitOfWork.context.Set<YY_Material>().Where(o => o.ActivityId == id).Select(o => new { ShareCount = o.YY_MaterialShare.Count, BrowseCount = o.MaterialBrower.Count, EnrollCount = o.Enroll.Count, PassCount = o.Enroll.Where(e => e.EnrollStatus >= (int)EnrollStatusEnum.NoSignIn).Count(), SignInCount = o.Enroll.Where(e => e.EnrollStatus == (int)EnrollStatusEnum.SignIn).Count() }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return null;
            }
        }

        internal int DeleteActivity(int id)
        {
            try
            {
                var model = Entities.Where(o => o.Id == id).FirstOrDefault();
                if (model == null) return 0;
                if (model.CreateTime.AddDays(1) > DateTime.Now) return -1;
                return Delete(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return 0;
            }
        }
    }
}