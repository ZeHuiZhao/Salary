using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Salary.Common;
using Salary.Data;
using Salary.DataModel.Entity;
using Salary.Models.Report;

namespace Salary.Service
{
    public class ReportService : EFRepository<YY_Enroll>
    {
        private readonly UserService _user = new UserService();
        internal object GetMaterialDataById(MaterialDataQueryDto dto)
        {
            try
            {
                var model = new MaterialReportDto();
                var material = UnitOfWork.context.Set<YY_Material>().Where(o => o.Id == dto.Id).FirstOrDefault();
                if (material == null) return null;
                model.MaterialTitle = material.MaterialTitle;
                MaterialTemp mt = Entities.Where(o => o.MId == dto.Id).Select(o => new MaterialTemp { ShareCount = o.Material.YY_MaterialShare.Count, BrowseCount = o.Material.MaterialBrower.Count, EnrollCount = Entities.Where(e => e.MId == o.MId).Count(), ParticipateCount = Entities.Where(e => e.MId == o.MId && e.EnrollStatus == (int)EnrollStatusEnum.AlreadyParticipate).Count() }).FirstOrDefault();

                if (mt == null) mt = new MaterialTemp { BrowseCount = 0, EnrollCount = 0, ParticipateCount = 0, ShareCount = 0 };
                model.BarData = new { DisplayName = new string[] { "素材转发数", "素材浏览数", "素材报名人数", "参课人数" }, DisplayValue = new int[] { mt.ShareCount, mt.BrowseCount, mt.EnrollCount, mt.ParticipateCount } };
                return model;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return null;
            }
        }

        internal DataTable ExportSalesList(ChannelExportQueryDto query)
        {
            return GetChannelExportUserList(query).ToDataTable();
        }


        internal object GetChannelExportList(ChannelExportQueryDto query)
        {
            var model = new ChannelReportDto();

            var list = GetChannelExportUserList(query);
            model.FunnelData = new { DisplayName = new string[] { "素材浏览总数", "报名人数", "参课人数" }, DisplayValue = new object[] { new { value = list.Sum(l => l.BrowseCount), name = "素材浏览总数" }, new { value = list.Sum(l => l.EnrollCount), name = "报名人数" }, new { value = list.Sum(l => l.ParticipateCount), name = "参课人数" } } };
            var channelCount = query.ChannelId.HasValue ? 1 : _user.GetActiveChannelCount();
            model.CircleData = new { ChannelCount = channelCount, SalesCount = _user.GetSalesCount(query.ChannelId), ShareCount = list.Sum(l => l.ShareCount), BrowseCount = list.Sum(l => l.BrowseCount), EnrollCount = list.Sum(l => l.EnrollCount), ParticipateCount = list.Sum(l => l.ParticipateCount) };
            //model.SalesData = list;
            return model;
        }

        internal object GetChannelYYList(ChannelYYQueryDto query)
        {
            if (Cookies.Current == null || Cookies.UserType <= 0) return null;
            var model = new ChannelYYPaging();
            try
            {
                int skip = query.PageSize * (query.PageIndex - 1);
                var a = GetChannelSalesDetailList(new ChannelYYQueryNoPagingDto { ChannelId = query.ChannelId, MaterialTitle = query.MaterialTitle, MTId = query.MTId, SalesId = query.SalesId });


                var list = from m in a
                           from ms in m.YY_MaterialShare
                           select new ChannelYYDtoTemp { SalesId = ms.SalesId,Id = m.Id, TrueName = ms.User.TrueName, MaterialTitle = m.MaterialTitle, MaterialTypeName = m.MaterialTypes.TypeName, BrowseCount = m.MaterialBrower.Where(mb => mb.MId == ms.MId && mb.ChannelId == Cookies.ChannelId && mb.SalesId == ms.SalesId).Count(), EnrollCount = m.Enroll.Where(e => e.MId == ms.MId && e.ChannelId == Cookies.ChannelId && e.SalesId == ms.SalesId).Count(), LastTime = m.YY_MaterialShare.Where(s => s.MId == ms.MId).Max(c => c.CreateTime), ShareCount = m.YY_MaterialShare.Where(c => c.MId == ms.MId && c.ChannelId == Cookies.ChannelId && c.SalesId == ms.SalesId).Count(), ParticipateCount = m.Enroll.Where(e => e.MId == ms.MId && e.EnrollStatus == (int)EnrollStatusEnum.AlreadyParticipate && e.ChannelId == Cookies.ChannelId && e.SalesId == ms.SalesId).Count(), AllBrowseCount = m.MaterialBrower.Where(mb => mb.MId == ms.MId && mb.SalesId == ms.SalesId).Count(), AllEnrollCount = m.Enroll.Where(e => e.MId == ms.MId && e.SalesId == ms.SalesId).Count(), AllShareCount = m.YY_MaterialShare.Where(c => c.MId == ms.MId && c.SalesId == ms.SalesId).Count(), AllParticipateCount = m.Enroll.Where(e => e.MId == ms.MId && e.EnrollStatus == (int)EnrollStatusEnum.AlreadyParticipate && e.SalesId == ms.SalesId).Count() };
                list = list.Distinct();

                if (query.SalesId.HasValue && query.SalesId.Value > 0)
                {
                    list = list.Where(o => o.SalesId==query.SalesId.Value);
                }

                if (query.ChannelId.HasValue && query.ChannelId.Value > 0)
                {
                    var uu = UnitOfWork.context.Set<YY_Userinfo>();
                    list = list.Where(o => uu.Where(u => u.Id == o.SalesId).FirstOrDefault().ChannelId == query.ChannelId.Value);
                }
                model.CurrentPage = query.PageIndex;
                model.PageSize = query.PageSize;
                model.TotalCount = list.Count();
                model.TotalPage = (int)Math.Ceiling(model.TotalCount * 1.0 / model.PageSize);
                if (query.BrowseOrder > 0)
                {
                    if (query.BrowseOrder == OrderEnum.ASC)
                    {
                        list = list.OrderBy(o => o.AllBrowseCount);
                    }
                    else
                    {
                        list = list.OrderByDescending(o => o.AllBrowseCount);
                    }
                }
                if (query.ShareOrder > 0)
                {
                    if (query.ShareOrder == OrderEnum.ASC)
                    {
                        list = list.OrderBy(o => o.AllShareCount);
                    }
                    else
                    {
                        list = list.OrderByDescending(o => o.AllShareCount);
                    }
                }
                if (query.ParticipateOrder > 0)
                {
                    if (query.ParticipateOrder == OrderEnum.ASC)
                    {
                        list = list.OrderBy(o => o.AllParticipateCount);
                    }
                    else
                    {
                        list = list.OrderByDescending(o => o.AllParticipateCount);
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
                List<ChannelYYDto> reslut = new List<ChannelYYDto>();
                foreach (var item in resultList)
                {
                    ChannelYYDto yy = new ChannelYYDto();
                    if (Cookies.UserType == (int)UserTypeEnum.Admin || Cookies.UserType == (int)UserTypeEnum.ZLChannelManager)
                    {
                        yy.BrowseCount = item.AllBrowseCount.ToString();
                        yy.EnrollCount = item.AllEnrollCount.ToString();
                        yy.LastTime = item.LastTime.ToString("yyyy") == "0001" ? string.Empty : item.LastTime.ToString("yyyy-MM-dd");
                        yy.MaterialTitle = item.MaterialTitle;
                        yy.MaterialTypeName = item.MaterialTypeName;
                        yy.ParticipateCount = item.AllParticipateCount.ToString();
                        yy.ShareCount = item.AllShareCount.ToString();
                        yy.TrueName = item.TrueName;
                    }
                    else if (Cookies.UserType == (int)UserTypeEnum.ChannelManager)
                    {
                        yy.BrowseCount = item.AllBrowseCount.ToString() + "(" + item.BrowseCount.ToString() + ")";
                        yy.EnrollCount = item.AllEnrollCount.ToString() + "(" + item.EnrollCount.ToString() + ")";
                        yy.LastTime = item.LastTime.ToString("yyyy") == "0001" ? string.Empty : item.LastTime.ToString("yyyy-MM-dd");
                        yy.MaterialTitle = item.MaterialTitle;
                        yy.MaterialTypeName = item.MaterialTypeName;
                        yy.ParticipateCount = item.AllParticipateCount.ToString() + "(" + item.ParticipateCount.ToString() + ")";
                        yy.ShareCount = item.AllShareCount.ToString() + "(" + item.ShareCount.ToString() + ")";
                        yy.TrueName = item.TrueName;
                    }
                    else
                    {
                        return null;
                    }
                    reslut.Add(yy);
                }
                model.ChannelYYList = reslut;
                return model;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return model;
            }
        }

        internal object GetChannelSalesExportList(ChannelSalesQueryDto query)
        {
            var list =  this.GetChannelExportUserList(query);
            if (query.BrowseOrder > 0)
            {
                if (query.BrowseOrder == OrderEnum.ASC)
                {
                    list = list.OrderBy(o => o.BrowseCount).ToList();
                }
                else
                {
                    list = list.OrderByDescending(o => o.BrowseCount).ToList();
                }
            }
            if (query.ShareOrder > 0)
            {
                if (query.ShareOrder == OrderEnum.ASC)
                {
                    list = list.OrderBy(o => o.ShareCount).ToList();
                }
                else
                {
                    list = list.OrderByDescending(o => o.ShareCount).ToList();
                }
            }
            if (query.ParticipateOrder > 0)
            {
                if (query.ParticipateOrder == OrderEnum.ASC)
                {
                    list = list.OrderBy(o => o.ParticipateCount).ToList();
                }
                else
                {
                    list = list.OrderByDescending(o => o.ParticipateCount).ToList();
                }
            }
            if (query.EnrollOrder > 0)
            {
                if (query.EnrollOrder == OrderEnum.ASC)
                {
                    list = list.OrderBy(o => o.EnrollCount).ToList();
                }
                else
                {
                    list = list.OrderByDescending(o => o.EnrollCount).ToList();
                }
            }
            return list;
        }

        public delegate bool CompareDelegate<T>(T x, T y);
        public class Compare<T> : IEqualityComparer<T>
        {
            private CompareDelegate<T> _compare;
            public Compare(CompareDelegate<T> d)
            {
                this._compare = d;
            }

            public bool Equals(T x, T y)
            {
                if (_compare != null)
                {
                    return this._compare(x, y);
                }
                else
                {
                    return false;
                }
            }

            public int GetHashCode(T obj)
            {
                return obj.ToString().GetHashCode();
            }
        }
        internal object GetMaterialSalesDataById(MaterialSalesDataQueryDto dto)
        {
            try
            {
                var material = UnitOfWork.context.Set<YY_Material>().Where(o => o.Id == dto.Id).FirstOrDefault();
                if (material == null) return null;

                var list = UnitOfWork.context.Set<YY_MaterialShare>().Where(o => o.MId == dto.Id).ToList().Distinct(new Compare<YY_MaterialShare>((x, y) => (x != null && y != null) && (x.SalesId == y.SalesId))).Select(o => new MaterialSales { TrueName = o.User.TrueName, ShareCount = o.Material.YY_MaterialShare.Where(ms => ms.SalesId == o.SalesId).Count(), BrowseCount = o.Material.MaterialBrower.Where(mb => mb.SalesId == o.SalesId).Count(), EnrollCount = o.Material.Enroll.Where(e => e.SalesId == o.SalesId).Count(), ParticipateCount = o.Material.Enroll.Where(e => e.EnrollStatus == (int)EnrollStatusEnum.AlreadyParticipate && e.SalesId == o.SalesId).Count(), LastTime = o.Material.YY_MaterialShare.Max(ms => ms.CreateTime).ToString("yyyy-MM-dd") });


                if (dto.BrowseOrder > 0)
                {
                    if (dto.BrowseOrder == OrderEnum.ASC)
                    {
                        list = list.OrderBy(o => o.BrowseCount);
                    }
                    else
                    {
                        list = list.OrderByDescending(o => o.BrowseCount);
                    }
                }
                if (dto.ShareOrder > 0)
                {
                    if (dto.ShareOrder == OrderEnum.ASC)
                    {
                        list = list.OrderBy(o => o.ShareCount);
                    }
                    else
                    {
                        list = list.OrderByDescending(o => o.ShareCount);
                    }
                }
                if (dto.ParticipateOrder > 0)
                {
                    if (dto.ParticipateOrder == OrderEnum.ASC)
                    {
                        list = list.OrderBy(o => o.ParticipateCount);
                    }
                    else
                    {
                        list = list.OrderByDescending(o => o.ParticipateCount);
                    }
                }
                if (dto.EnrollOrder > 0)
                {
                    if (dto.EnrollOrder == OrderEnum.ASC)
                    {
                        list = list.OrderBy(o => o.EnrollCount);
                    }
                    else
                    {
                        list = list.OrderByDescending(o => o.EnrollCount);
                    }
                }
                return list.ToList();

            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return null;
            }
        }

        internal object GetSalesMaterialList(MaterialSalesDataQueryDto dto)
        {
            var list = this.GetSalesUserMaterialList(dto.Id);
            if (dto.BrowseOrder > 0)
            {
                if (dto.BrowseOrder == OrderEnum.ASC)
                {
                    list = list.OrderBy(o => o.BrowseCount).ToList();
                }
                else
                {
                    list = list.OrderByDescending(o => o.BrowseCount).ToList();
                }
            }
            if (dto.ShareOrder > 0)
            {
                if (dto.ShareOrder == OrderEnum.ASC)
                {
                    list = list.OrderBy(o => o.ShareCount).ToList();
                }
                else
                {
                    list = list.OrderByDescending(o => o.ShareCount).ToList();
                }
            }
            if (dto.ParticipateOrder > 0)
            {
                if (dto.ParticipateOrder == OrderEnum.ASC)
                {
                    list = list.OrderBy(o => o.ParticipateCount).ToList();
                }
                else
                {
                    list = list.OrderByDescending(o => o.ParticipateCount).ToList();
                }
            }
            if (dto.EnrollOrder > 0)
            {
                if (dto.EnrollOrder == OrderEnum.ASC)
                {
                    list = list.OrderBy(o => o.EnrollCount).ToList();
                }
                else
                {
                    list = list.OrderByDescending(o => o.EnrollCount).ToList();
                }
            }
            return list;
        }

        public class MaterialSales
        {
            public string TrueName { get; set; }
            public int ShareCount { get; set; }
            public int BrowseCount { get; set; }
            public int EnrollCount { get; set; }
            public int ParticipateCount { get; set; }
            public string LastTime { get; set; }

        }

        internal DataTable ExportChannelYYList(ChannelYYQueryNoPagingDto dto)
        {
            try
            {
                var a = GetChannelSalesDetailList(dto);
                var list = from m in a
                           from ms in m.YY_MaterialShare
                           select new ChannelYYDtoTemp { Id = m.Id, TrueName = ms.User.TrueName, MaterialTitle = m.MaterialTitle, MaterialTypeName = m.MaterialTypes.TypeName, BrowseCount = m.MaterialBrower.Where(mb => mb.MId == ms.MId && mb.ChannelId == Cookies.ChannelId && mb.SalesId == ms.SalesId).Count(), EnrollCount = m.Enroll.Where(e => e.MId == ms.MId && e.ChannelId == Cookies.ChannelId && e.SalesId == ms.SalesId).Count(), LastTime = m.YY_MaterialShare.Where(s => s.MId == ms.MId).Max(c => c.CreateTime), ShareCount = m.YY_MaterialShare.Where(c => c.MId == ms.MId && c.ChannelId == Cookies.ChannelId && c.SalesId == ms.SalesId).Count(), ParticipateCount = m.Enroll.Where(e => e.MId == ms.MId && e.EnrollStatus == (int)EnrollStatusEnum.AlreadyParticipate && e.ChannelId == Cookies.ChannelId && e.SalesId == ms.SalesId).Count(), AllBrowseCount = m.MaterialBrower.Where(mb => mb.MId == ms.MId && mb.SalesId == ms.SalesId).Count(), AllEnrollCount = m.Enroll.Where(e => e.MId == ms.MId && e.SalesId == ms.SalesId).Count(), AllShareCount = m.YY_MaterialShare.Where(c => c.MId == ms.MId && c.SalesId == ms.SalesId).Count(), AllParticipateCount = m.Enroll.Where(e => e.MId == ms.MId && e.EnrollStatus == (int)EnrollStatusEnum.AlreadyParticipate && e.SalesId == ms.SalesId).Count() };
                list = list.Distinct();
                var resultList = list.Distinct().ToList();
                List<ChannelYYDto> result = new List<ChannelYYDto>();
                foreach (var item in resultList)
                {
                    ChannelYYDto yy = new ChannelYYDto();
                    if (Cookies.UserType == (int)UserTypeEnum.Admin || Cookies.UserType == (int)UserTypeEnum.ZLChannelManager)
                    {
                        yy.BrowseCount = item.AllBrowseCount.ToString();
                        yy.EnrollCount = item.AllEnrollCount.ToString();
                        yy.LastTime = item.LastTime.ToString("yyyy") == "0001" ? string.Empty : item.LastTime.ToString("yyyy-MM-dd");
                        yy.MaterialTitle = item.MaterialTitle;
                        yy.MaterialTypeName = item.MaterialTypeName;
                        yy.ParticipateCount = item.AllParticipateCount.ToString();
                        yy.ShareCount = item.AllShareCount.ToString();
                        yy.TrueName = item.TrueName;
                    }
                    else if (Cookies.UserType == (int)UserTypeEnum.ChannelManager)
                    {
                        yy.BrowseCount = item.AllBrowseCount.ToString() + "(" + item.BrowseCount.ToString() + ")";
                        yy.EnrollCount = item.AllEnrollCount.ToString() + "(" + item.EnrollCount.ToString() + ")";
                        yy.LastTime = item.LastTime.ToString("yyyy") == "0001" ? string.Empty : item.LastTime.ToString("yyyy-MM-dd");
                        yy.MaterialTitle = item.MaterialTitle;
                        yy.MaterialTypeName = item.MaterialTypeName;
                        yy.ParticipateCount = item.AllParticipateCount.ToString() + "(" + item.ParticipateCount.ToString() + ")";
                        yy.ShareCount = item.AllShareCount.ToString() + "(" + item.ShareCount.ToString() + ")";
                        yy.TrueName = item.TrueName;
                    }
                    else
                    {
                        return null;
                    }
                    result.Add(yy);
                }
                return result.ToDataTable();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return null;
            }
        }

        private IQueryable<YY_Material> GetChannelSalesDetailList(ChannelYYQueryNoPagingDto query)
        {
            try
            {
                var a = UnitOfWork.context.Set<YY_Material>().Where(o => true);
                if (query.MTId.HasValue && query.MTId > 0)
                {
                    a = a.Where(o => o.MTId == query.MTId.Value);
                }
                if (query.SalesId.HasValue && query.SalesId.Value > 0)
                {
                    a = a.Where(o => o.YY_MaterialShare.Where(ms => ms.SalesId == query.SalesId.Value).Count() > 0);
                }
                if (!string.IsNullOrEmpty(query.MaterialTitle))
                {
                    a = a.Where(o => o.MaterialTitle.Contains(query.MaterialTitle));
                }
                if (query.ChannelId.HasValue && query.ChannelId.Value > 0)
                {
                    a = a.Where(o => o.ShareSource == 1 || (o.MaterialChannel.Where(mc => mc.ChannelId == query.ChannelId).Count() > 0));
                }
                if (Cookies.UserType == (int)UserTypeEnum.ChannelManager || Cookies.UserType == (int)UserTypeEnum.Sales)
                {
                    a = a.Where(o => o.ShareSource == 1 || o.MaterialChannel.Where(b => b.MId == o.Id).ToList().Count > 0);
                }
                return a;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return null;
            }
        }

        internal DataTable ExportSalesDataById(int id)
        {
            return GetSalesUserMaterialList(id).ToDataTable();
        }

        internal object GetSalesDataById(int id)
        {
            var model = new SalesDataReportDto();
            var list = GetSalesUserMaterialList(id);
            model.FunnelData = new { DisplayName = new string[] { "素材浏览总数", "报名人数", "参课人数" }, DisplayValue = new object[] { new { value = list.Sum(l => l.BrowseCount), name = "素材浏览总数" }, new { value = list.Sum(l => l.EnrollCount), name = "报名人数" }, new { value = list.Sum(l => l.ParticipateCount), name = "参课人数" } } };
            model.CircleData = new { MaterialCount = list.Count, ShareCount = list.Sum(l => l.ShareCount), BrowseCount = list.Sum(l => l.BrowseCount), EnrollCount = list.Sum(l => l.EnrollCount), ParticipateCount = list.Sum(l => l.ParticipateCount) };
            return model;
        }

        private List<UseMaterialTemp> GetSalesUserMaterialList(int id)
        {
            try
            {
                var user = UnitOfWork.context.Set<YY_Userinfo>().Where(o => o.Id == id).FirstOrDefault();
                if (user == null) return null;

                return UnitOfWork.context.Set<YY_Material>().Where(o => o.YY_MaterialShare.Where(ms => ms.SalesId == user.Id).Count() > 0).Select(o => new UseMaterialTemp { MaterialTitle = o.MaterialTitle, BrowseCount = o.MaterialBrower.Where(mb => mb.SalesId == user.Id).Count(), EnrollCount = o.Enroll.Where(e => e.SalesId == user.Id).Count(), ParticipateCount = o.Enroll.Where(e => e.SalesId == user.Id && e.EnrollStatus == (int)EnrollStatusEnum.AlreadyParticipate).Count(), ShareCount = o.YY_MaterialShare.Where(ms => ms.SalesId == user.Id).Count() }).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return null;
            }
        }

        private List<ChannelTemp> GetChannelExportUserList(ChannelExportQueryDto query)
        {
            try
            {
                var a = UnitOfWork.context.Set<YY_Userinfo>().Where(o => o.UserType == (int)UserTypeEnum.Sales);
                if (query.ChannelId.HasValue && query.ChannelId.Value > 0)
                {
                    a = a.Where(c => c.ChannelId == query.ChannelId.Value);
                }
                if (query.SalesId.HasValue && query.SalesId.Value > 0)
                {
                    a = a.Where(c => c.Id == query.SalesId.Value);
                }
                return a.Select(o => new ChannelTemp { Id = o.Id, TrueName = o.TrueName, ShareCount = o.YY_MaterialShare.Count, BrowseCount = o.MaterialBrower.Count, EnrollCount = o.Enroll.Count, ParticipateCount = o.Enroll.Where(e => e.EnrollStatus == (int)EnrollStatusEnum.AlreadyParticipate).Count() }).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return null;
            }
        }



        public class MaterialTemp
        {
            public int ShareCount { get; set; }
            public int BrowseCount { get; set; }
            public int EnrollCount { get; set; }
            public int ParticipateCount { get; set; }
        }

        public class UseMaterialTemp : MaterialTemp
        {
            public string MaterialTitle { get; set; }

        }

        public class ChannelTemp : MaterialTemp
        {
            public int Id { get; set; }
            public string TrueName { get; set; }
        }
    }
}