using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using ThoughtWorks.QRCode.Codec;
using Salary.Common;
using Salary.Data;
using Salary.DataModel.Entity;
using Salary.Models.Material;
using Salary.Models.Report;

namespace Salary.Service
{
    public class MaterialService : EFRepository<YY_Material>
    {

        internal object GetMaterialList(QueryMaterialDto query)
        {
            try
            {
                var model = new QueryMaterialPaging();
                int skip = query.PageSize * (query.PageIndex - 1);
                var a = Entities;
                if (query.MaterialType != MaterialTypeEnum.All)
                {
                    if (query.MaterialType == MaterialTypeEnum.ZLMaterial)
                    {
                        a = a.Where(o => o.MaterialType == (int)MaterialTypeEnum.ZLMaterial);
                        if (Cookies.UserType == (int)UserTypeEnum.ChannelManager || Cookies.UserType == (int)UserTypeEnum.Sales)
                        {
                            a = a.Where(o => o.ShareSource == 1 || o.MaterialChannel.Where(b => b.MId == o.Id).ToList().Count > 0);
                        }
                    }
                    if (query.MaterialType == MaterialTypeEnum.ChannelMaterial)
                    {
                        a = a.Where(o => o.MaterialType == (int)MaterialTypeEnum.ChannelMaterial && o.ChannelId == Cookies.ChannelId);
                    }
                }
                else
                {
                    if (Cookies.UserType == (int)UserTypeEnum.ChannelManager || Cookies.UserType == (int)UserTypeEnum.Sales)
                    {
                        a = a.Where(o => o.ShareSource == 1 || o.MaterialChannel.Where(b => b.MId == o.Id).ToList().Count > 0);
                    }
                }
                if (!string.IsNullOrEmpty(query.MaterialTitle))
                    a = a.Where(o => o.MaterialTitle.Contains(query.MaterialTitle));

                if (query.MTId.HasValue)
                    a = a.Where(o => o.MTId == query.MTId.Value);


                var list = a.Select(o => new MaterialModel { CreateTime = o.CreateTime, Id = o.Id, LastTime = o.YY_MaterialShare.Where(gg => gg.MId == o.Id).OrderByDescending(gg => gg.CreateTime).Select(gg => gg.CreateTime).FirstOrDefault(), MaterialTitle = o.MaterialTitle, CoverImg = o.CoverImg, IsActive = o.IsActive, MaterialTypeName = o.MaterialTypes.TypeName, ChannelShare = o.YY_MaterialShare.Where(b => b.MId == o.Id && b.ChannelId == Cookies.ChannelId).Count(), AllShare = o.YY_MaterialShare.Where(b => b.MId == o.Id).Count(), AllBrowse = o.MaterialBrower.Where(b => b.MId == o.Id).Count(), ChannelBrowse = o.MaterialBrower.Where(b => b.MId == o.Id && b.ChannelId == Cookies.ChannelId).Count(), LastShareTime = "", SalesBrowse = o.MaterialBrower.Where(mb => mb.MId == o.Id && mb.SalesId == Cookies.UserId).Count(), SalesShare = o.YY_MaterialShare.Where(ms => ms.MId == o.Id && ms.SalesId == Cookies.UserId).Count(), AllEnrollCount = o.Enroll.Where(e => e.MId == o.Id).Count(), SalesEnrollCount = o.Enroll.Where(e => e.MId == o.Id && e.SalesId == Cookies.UserId).Count(), AllParticipate = o.Enroll.Where(e => e.MId == o.Id && e.EnrollStatus == (int)EnrollStatusEnum.AlreadyParticipate).Count(), SalesParticipate = o.Enroll.Where(e => e.MId == o.Id && e.EnrollStatus == (int)EnrollStatusEnum.AlreadyParticipate && e.SalesId == Cookies.UserId).Count(), ChannelEnrollCount = o.Enroll.Where(e => e.MId == o.Id && e.ChannelId == Cookies.ChannelId).Count(), ChannelParticipate = o.Enroll.Where(e => e.MId == o.Id && e.ChannelId == Cookies.ChannelId && e.EnrollStatus == (int)EnrollStatusEnum.AlreadyParticipate).Count() });
                model.CurrentPage = query.PageIndex;
                model.PageSize = query.PageSize;
                model.TotalCount = list.Count();
                model.TotalPage = (int)Math.Ceiling(model.TotalCount * 1.0 / model.PageSize);
                if (query.BrowseOrder > 0)
                {
                    if (query.BrowseOrder == OrderEnum.ASC)
                    {
                        list = list.OrderBy(o => o.AllBrowse);
                    }
                    else
                    {
                        list = list.OrderByDescending(o => o.AllBrowse);
                    }
                }
                if (query.ShareOrder > 0)
                {
                    if (query.ShareOrder == OrderEnum.ASC)
                    {
                        list = list.OrderBy(o => o.AllShare);
                    }
                    else
                    {
                        list = list.OrderByDescending(o => o.AllShare);
                    }
                }
                if (query.ParticipateOrder > 0)
                {
                    if (query.ParticipateOrder == OrderEnum.ASC)
                    {
                        list = list.OrderBy(o => o.AllParticipate);
                    }
                    else
                    {
                        list = list.OrderByDescending(o => o.AllParticipate);
                    }
                }
                if (query.EnrollOrder > 0)
                {
                    if (query.EnrollOrder == OrderEnum.ASC)
                    {
                        list = list.OrderBy(o => o.AllEnrollCount);
                    }
                    else
                    {
                        list = list.OrderByDescending(o => o.AllEnrollCount);
                    }
                }
                if (query.BrowseOrder == 0 && query.EnrollOrder == 0 && query.ParticipateOrder == 0 && query.ShareOrder == 0)
                {
                    list = list.OrderByDescending(o => o.Id);
                }

                var resultList = list.Skip(skip).Take(query.PageSize).ToList();
                foreach (var item in resultList)
                {
                    item.DeleteIsable = DateTime.Now > item.CreateTime.AddDays(1) ? 0 : 1;
                    item.DisplayName = "素材24小时内才能删除";
                    item.LastShareTime = item.LastTime.ToString("yyyy-MM-dd") == "0001-01-01" ? string.Empty : item.LastTime.ToString("yyyy-MM-dd");
                }
                model.MaterialList = resultList;
                return model;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return null;
            }
        }

        internal DataTable ExportMateriaSaleslList(int id)
        {
            try
            {
                var material = Entities.Where(o => o.Id == id).FirstOrDefault();
                if (material == null) return null;
                var a = UnitOfWork.context.Set<YY_MaterialShare>().Where(o => o.MId == id).GroupBy(o=>o.SalesId).ToList().Select(o=>new { ShareCount=o.Count(), BrowserCount = UnitOfWork.context.Set<YY_MaterialBrower>().Where(mb=>mb.MId==id&&mb.SalesId==o.Key).Count(),TrueName = UnitOfWork.context.Set<YY_Userinfo>().Where(u=>u.Id==o.Key).FirstOrDefault().TrueName, EnrollCount=UnitOfWork.context.Set<YY_Enroll>().Where(e=>e.MId==id&&e.SalesId==o.Key).Count(), ParticipateCount = UnitOfWork.context.Set<YY_Enroll>().Where(e => e.MId == id && e.SalesId == o.Key&&e.EnrollStatus==(int)EnrollStatusEnum.AlreadyParticipate).Count(),LastTime=o.Max(c=>c.CreateTime).ToString("yyyy-MM-dd") });
                return a.ToList().ToDataTable();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }

        internal DataTable ExportMaterialList(MaterialQueryDto query)
        {
            try
            {
                var model = new QueryMaterialPaging();

                var a = Entities;

                if (Cookies.UserType == (int)UserTypeEnum.ChannelManager || Cookies.UserType == (int)UserTypeEnum.Sales)
                {
                    a = a.Where(o => o.ShareSource == 1 || o.MaterialChannel.Where(b => b.MId == o.Id).ToList().Count > 0);
                }

                if (!string.IsNullOrEmpty(query.MaterialTitle))
                    a = a.Where(o => o.MaterialTitle.Contains(query.MaterialTitle));

                if (query.MTId.HasValue&&query.MTId.Value>0)
                    a = a.Where(o => o.MTId == query.MTId.Value);

                if ((int)UserTypeEnum.ZLChannelManager == Cookies.UserType||(int)UserTypeEnum.Admin==Cookies.UserType)
                {
                   var b = a.Select(o => new MaterialTemp
                    {
                        Id = o.Id,
                        LastTime = o.YY_MaterialShare.Where(gg => gg.MId == o.Id).OrderByDescending(gg => gg.CreateTime).Select(gg => gg.CreateTime).FirstOrDefault(),
                        MaterialTitle = o.MaterialTitle,
                        MaterialTypeName = o.MaterialTypes.TypeName,
                        ShareCount = o.YY_MaterialShare.Count.ToString(),
                        BrowserCount = o.MaterialBrower.Count.ToString(),
                        EnrollCount = o.Enroll.Count.ToString(),
                        ParticipateCount = o.Enroll.Where(e => e.EnrollStatus == (int)EnrollStatusEnum.AlreadyParticipate).Count().ToString()
                    });
                    var list = b.ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].LastTime.ToString("yyyy-MM-dd").Contains("0001-01-01"))
                        {
                            list[i].LastTimeString = string.Empty;
                        }else
                        {
                            list[i].LastTimeString = list[i].LastTime.ToString("yyyy-MM-dd");
                        }
                    }
                    return list.ToDataTable();
                }
                else if ((int)UserTypeEnum.ChannelManager == Cookies.UserType)
                {
                   var b = a.Select(o => new MaterialTemp
                    {
                        Id = o.Id,
                        LastTime = o.YY_MaterialShare.Where(gg => gg.MId == o.Id).OrderByDescending(gg => gg.CreateTime).Select(gg => gg.CreateTime).FirstOrDefault(),
                        MaterialTitle = o.MaterialTitle,
                        MaterialTypeName = o.MaterialTypes.TypeName,
                        ShareCount = o.YY_MaterialShare.Count + "(" + o.YY_MaterialShare.Where(ms => ms.ChannelId == Cookies.ChannelId).Count() + ")",
                        BrowserCount = o.MaterialBrower.Count + "(" + o.MaterialBrower.Where(ms => ms.ChannelId == Cookies.ChannelId).Count() + ")",
                        EnrollCount = o.Enroll.Count + "(" + o.Enroll.Where(ms => ms.ChannelId == Cookies.ChannelId).Count() + ")",
                        ParticipateCount = o.Enroll.Where(e => e.EnrollStatus == (int)EnrollStatusEnum.AlreadyParticipate).Count() + "(" + o.Enroll.Where(e => e.EnrollStatus == (int)EnrollStatusEnum.AlreadyParticipate && e.ChannelId == Cookies.ChannelId).Count() + ")"
                    });
                    var list = b.ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].LastTime.ToString("yyyy-MM-dd").Contains("0001-01-01"))
                        {
                            list[i].LastTimeString = string.Empty;
                        }
                        else
                        {
                            list[i].LastTimeString = list[i].LastTime.ToString("yyyy-MM-dd");
                        }
                    }
                    return list.ToDataTable();
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }

        class MaterialTemp
        {
            public int Id { get; set; }
            public DateTime LastTime { get; set; }
            public string LastTimeString { get; set; }
            public string MaterialTitle { get; set; }
            public string MaterialTypeName { get; set; }
            public string ShareCount { get; set; }
            public string BrowserCount { get; set; }
            public string EnrollCount { get; set; }
            public string ParticipateCount { get; set; }
        }


        internal object GetMaterialListByWechat(QueryWechatMaterialDto dto)
        {
            try
            {
                var model = new QueryWechatMaterialPaging();
                int skip = dto.PageSize * (dto.PageIndex - 1);
                var a = UnitOfWork.context.Set<YY_Material>().Where(o => o.IsActive == 1 && o.MaterialType == (int)dto.MaterialType && o.MTId == dto.MTId);
                if (dto.MaterialType == MaterialTypeEnum.ZLMaterial)
                {
                    a = a.Where(o => o.MaterialType == (int)MaterialTypeEnum.ZLMaterial);
                    a = a.Where(o => o.ShareSource == 1 || o.MaterialChannel.Where(b => b.MId == o.Id).ToList().Count > 0);
                }
                if (dto.MaterialType == MaterialTypeEnum.ChannelMaterial)
                {
                    var user = new UserService().GetSalesUser(dto.OpenId);
                    if (user != null)
                    {
                        a = a.Where(o => o.MaterialType == (int)MaterialTypeEnum.ChannelMaterial && o.ChannelId == user.ChannelId);
                    }
                    else
                        return null;
                }
                if (!string.IsNullOrEmpty(dto.MaterialTitle))
                {
                    a = a.Where(o => o.MaterialTitle.Contains(dto.MaterialTitle));
                }
                var cc = a.OrderBy(o => o.Id).Skip(skip).Take(dto.PageSize).ToList();
                var list = cc.Select(o => new MaterialWechatModel { Id = o.Id, MaterialTitle = o.MaterialTitle, CoverImg = o.CoverImg, CreateTime = o.CreateTime.ToString("yyyy-MM-dd"), Share = o.YY_MaterialShare.Where(b => b.MId == o.Id).Count(), Browse = o.MaterialBrower.Where(b => b.MId == o.Id).Count(), Enroll = o.Enroll.Where(b => b.MId == o.Id).Count() }).ToList();
                model.CurrentPage = dto.PageIndex;
                model.PageSize = dto.PageSize;
                model.TotalCount = cc.Count;
                model.TotalPage = (int)Math.Ceiling(model.TotalCount * 1.0 / model.PageSize);
                model.MaterialList = list.ToList();
                return model;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return null;
            }
        }

        internal int AddMaterial(CreateMaterialDto create)
        {
            try
            {
                YY_Material model = new YY_Material() { DisplayTime = DateTime.Now, VirtualTimes = create.VirtualTimes, MaterialContent = HttpUtility.UrlEncode(create.MaterialContent), MaterialSummary = create.MaterialSummary, ActivityId = create.ActivityId, ChannelId = create.ChannelId, CoverImg = create.CoverImg, CreateTime = DateTime.Now, CreateUser = Convert.ToInt32(Cookies.UserCode), IsActive = create.IsActive, MaterialTips = create.MaterialTips, MaterialTitle = create.MaterialTitle, MTId = create.MTId, MaterialType = create.MaterialType, ShareSource = create.ShareSource };

                //return Insert(model);
                UnitOfWork.RegisterNew<YY_Material>(model);
                if (model.ShareSource == 3 && !string.IsNullOrEmpty(create.ChannelIds))
                {
                    string[] strs = create.ChannelIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    var mcList = UnitOfWork.context.Set<YY_MaterialChannel>().Where(o => o.MId == model.Id).ToList();
                    for (int i = 0; i < mcList.Count; i++)
                    {
                        UnitOfWork.RegisterDeleted<YY_MaterialChannel>(mcList[i]);
                    }
                    for (int i = 0; i < strs.Length; i++)
                    {
                        YY_MaterialChannel mc = new YY_MaterialChannel() { ChannelId = Convert.ToInt32(strs[i]), MId = model.Id, CreateTime = DateTime.Now, CreateUser = Convert.ToInt32(Cookies.UserCode) };
                        UnitOfWork.RegisterNew<YY_MaterialChannel>(mc);
                    }
                }
                if (model.ShareSource == 2)
                {
                    var channelModel = new UserService().GetChannel("内部推荐");
                    if (channelModel != null)
                    {
                        YY_MaterialChannel mc = new YY_MaterialChannel() { ChannelId = channelModel.Id, MId = model.Id, CreateTime = DateTime.Now, CreateUser = Convert.ToInt32(Cookies.UserCode) };
                        UnitOfWork.RegisterNew<YY_MaterialChannel>(mc);
                    }
                }
                return UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                UnitOfWork.Rollback();
                return 0;
            }
        }

        internal int UpdateMaterial(UpdateMaterialDto update)
        {
            try
            {
                YY_Material model = Entities.Where(o => o.Id == update.Id).FirstOrDefault();
                model.MaterialContent = update.MaterialContent;
                model.MaterialSummary = update.MaterialSummary;
                model.ActivityId = update.ActivityId;
                model.ChannelId = update.ChannelId;
                model.CoverImg = update.CoverImg;
                model.IsActive = update.IsActive;
                model.MaterialTips = update.MaterialTips;
                model.MaterialTitle = update.MaterialTitle;
                model.MaterialType = update.MaterialType;
                model.ShareSource = update.ShareSource;
                model.MTId = update.MTId;
                model.DisplayTime = update.DisplayTime;
                model.VirtualTimes = update.VirtualTimes;

                model.UpdateTime = DateTime.Now;
                model.UpdateUser = Convert.ToInt32(Cookies.UserCode);


                UnitOfWork.RegisterModified<YY_Material>(model);
                if (model.ShareSource == 3 && !string.IsNullOrEmpty(update.ChannelIds))
                {
                    string[] strs = update.ChannelIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    var mcList = UnitOfWork.context.Set<YY_MaterialChannel>().Where(o => o.MId == model.Id).ToList();
                    for (int i = 0; i < mcList.Count; i++)
                    {
                        UnitOfWork.RegisterDeleted<YY_MaterialChannel>(mcList[i]);
                    }
                    for (int i = 0; i < strs.Length; i++)
                    {
                        YY_MaterialChannel mc = new YY_MaterialChannel() { ChannelId = Convert.ToInt32(strs[i]), MId = model.Id, CreateTime = DateTime.Now, CreateUser = Convert.ToInt32(Cookies.UserCode) };
                        UnitOfWork.RegisterNew<YY_MaterialChannel>(mc);
                    }
                }

                return UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                UnitOfWork.Rollback();
                return 0;
            }
        }

        internal object GetBrowseData(BrowseDataDto query)
        {
            try
            {
                var model = new BrowseDataQuery();
                int skip = query.PageSize * (query.PageIndex - 1);
                var user = new UserService().GetSalesUser(query.OpenId);
                if (user == null) return null;
                var a = Entities.Where(o => o.MaterialBrower.Where(mb => mb.SalesId == user.Id).Count() > 0);
                model.CurrentPage = query.PageIndex;
                model.PageSize = query.PageSize;
                model.TotalCount = a.Count();
                model.TotalPage = (int)Math.Ceiling(model.TotalCount * 1.0 / model.PageSize);

                var list = a.Select(o => new BrowseDataModel { MaterialTitle = o.MaterialTitle, CoverImg = o.CoverImg, MaterialTypeName = o.MaterialTypes.TypeName, LastShareTime = o.CreateTime, BrowseCount = o.MaterialBrower.Where(mb => mb.MId == o.Id && mb.SalesId == user.Id).Count() }).OrderByDescending(ccc => ccc.BrowseCount).Skip(skip).Take(query.PageSize).ToList();
                foreach (var item in list)
                {
                    var time = item.LastShareTime.ToString("yyyy-MM-dd");
                    item.LastShareTimeToString = time == "0001-01-01" ? "" : time;
                }
                model.MaterialList = list;

                return model;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }

        internal int AddMaterialBrowse(MaterialBrowswModel dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.DeviceCode))
                    return 0;


                var material = Entities.Where(o => o.Id == dto.MId && o.IsActive == 1).FirstOrDefault();
                if (material == null) return 0;
                var user = new UserService().GetSalesUser(dto.OpenId);
                if (user == null) return 0;
                var isExists = UnitOfWork.context.Set<YY_MaterialBrower>().Where(o => o.DeviceCode == dto.DeviceCode && o.MId == dto.MId && o.SalesId == user.Id).FirstOrDefault();
                if (isExists != null)
                {
                    return 0;
                }
                var model = new YY_MaterialBrower() { ChannelId = user.ChannelId, MId = material.Id, SalesId = user.Id, DeviceCode = dto.DeviceCode, CreateTime = DateTime.Now, CreateUser = Convert.ToInt32(Cookies.UserCode) };
                UnitOfWork.context.Set<YY_MaterialBrower>().Add(model);
                return UnitOfWork.context.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal int AddMaterialRelay(MaterialShareModel dto)
        {
            try
            {
                var material = Entities.Where(o => o.Id == dto.MId && o.IsActive == 1).FirstOrDefault();
                if (material == null) return 0;
                var user = new UserService().GetSalesUser(dto.OpenId);
                if (user == null) return 0;
                var model = new YY_MaterialShare() { ChannelId = user.ChannelId, MId = material.Id, SalesId = user.Id, CreateTime = DateTime.Now, CreateUser = Convert.ToInt32(Cookies.UserCode) };
                UnitOfWork.context.Set<YY_MaterialShare>().Add(model);
                return UnitOfWork.context.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal int DeleteChannelMaterial(int id)
        {
            try
            {
                var model = Entities.Where(o => o.Id == id).FirstOrDefault();
                if (model.CreateTime.AddDays(1) <= DateTime.Now)
                {
                    return -1;
                }
                return Delete(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal string GetQRcodeUrlByZL(int id)
        {
            try
            {
                string str_url = Config.websiteUrl + "/WXViews/MaterialCategoryDetails.html?mTId=" + id + "&test=1";
                QRCodeEncoder rq = new QRCodeEncoder();
                rq.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                rq.QRCodeScale = 20;
                Bitmap image = rq.Encode(str_url);
                string str_path = Config.FileUrl + id + ".png";
                image.Save(HttpContext.Current.Server.MapPath(str_path), System.Drawing.Imaging.ImageFormat.Jpeg);
                return str_path.Replace("~", Config.websiteUrl);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return string.Empty;
            }
        }

        internal int ToggleStatusByZL(int id)
        {
            try
            {
                var model = Entities.Where(o => o.Id == id).FirstOrDefault();
                model.IsActive = model.IsActive == 0 ? 1 : 0;
                return Update(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal object GetMaterialById(int id)
        {
            try
            {
                List<int> list = new List<int>();
                return Entities.Where(o => o.Id == id).ToList().Select(o => new { o.VirtualTimes, DisplayTime = o.DisplayTime.ToString("yyyy-MM-dd"), MaterialContent = HttpUtility.UrlDecode(o.MaterialContent), o.MaterialSummary, o.Id, o.ActivityId, o.ChannelId, o.CoverImg, o.IsActive, o.MaterialTips, o.MaterialTitle, o.MTId, o.MaterialType, o.ShareSource, ChannelIds = string.Join(",", o.MaterialChannel.Where(b => b.MId == id).Select(b => b.ChannelId).ToList()) }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }

        internal object GetWechatMaterialById(QueryMaterialDetail dto)
        {
            try
            {
                if (dto.IsTest == 1)
                {
                    return Entities.Where(o => o.Id == dto.Id).ToList().Select(o => new { MaterialContent = HttpUtility.UrlDecode(o.MaterialContent), CoverImg = o.CoverImg, MaterialSummary = o.MaterialSummary, Id = o.Id, MaterialTypeName = o.MaterialTypes.TypeName, CreateTime = o.CreateTime.ToString("yyyy-MM-dd"), BrosweCount = o.MaterialBrower.Where(c => c.MId == o.Id).Count() + o.VirtualTimes, MaterialTitle = o.MaterialTitle, MTId = o.MTId, }).FirstOrDefault();
                }
                UserService _user = new UserService();
                var user = _user.GetSalesUser(dto.OpenId);
                var channel = _user.GetChannel(user.ChannelId);
                List<int> list = new List<int>();
                return Entities.Where(o => o.Id == dto.Id).ToList().Select(o => new { MaterialContent = HttpUtility.UrlDecode(o.MaterialContent), CoverImg = o.CoverImg, MaterialSummary = o.MaterialSummary, Id = o.Id, MaterialTypeName = o.MaterialTypes.TypeName, CreateTime = o.CreateTime.ToString("yyyy-MM-dd"), BrosweCount = o.MaterialBrower.Where(c => c.MId == o.Id).Count() + o.VirtualTimes, ChannelId = channel.Id, ChannelName = channel.Name, MaterialTitle = o.MaterialTitle, MTId = o.MTId, }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }


    }
}