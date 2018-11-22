using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using ThoughtWorks.QRCode.Codec;
using Salary.Common;
using Salary.Data;
using Salary.DataModel.Entity;
using Salary.Models.MaterialType;

namespace Salary.Service
{
    public class MaterialTypeService : EFRepository<YY_MaterialType>
    {
        internal object GetZLMaterialTypeList(QueryMaterialType query)
        {
            try
            {
                var a = Entities;
                if (query != null && !string.IsNullOrEmpty(query.TypeName))
                {
                    a = a.Where(o => o.TypeName.Contains(query.TypeName));
                }
                var list = a.ToList().Select((o, i) => new MaterialTypeModel() { RowNum = ++i, Id = o.Id, TypeName = o.TypeName, CoverImg = o.CoverImg, IsActive = o.IsActive, IsActiveName = o.IsActive == 0 ? "未发布" : "发布" });
                
                return list.ToList();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }

        internal object GetOneMaterialTypeById(int id)
        {
            try
            {
                var list = Entities.Where(o => o.Id == id).Select(o => new { Id = o.Id, TypeName = o.TypeName, CoverImg = o.CoverImg, IsActive = o.IsActive }).FirstOrDefault();
                return list;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }

        internal object GetMaterialListByWechat(QueryWechatMaterialTypeDto dto)
        {
            try
            {
                var a = Entities.Where(o => o.IsActive == 1);
                if (!string.IsNullOrEmpty(dto.TypeName))
                    a = a.Where(o => o.TypeName.Contains(dto.TypeName));
                var list = a.Select(o => new { Id = o.Id, TypeName = o.TypeName, CoverImg = o.CoverImg }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }

        internal object GetMaterialTypeList()
        {
            try
            {
                var list = Entities.Where(o => o.IsActive == 1).Select(o => new { Id = o.Id, TypeName = o.TypeName }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }

        internal int AddMaterialType(CreateMaterialTypeDto create)
        {
            try
            {
                YY_MaterialType model = new YY_MaterialType();
                if (!string.IsNullOrEmpty(Cookies.UserCode))
                {
                    model.CreateUser = Convert.ToInt32(Cookies.UserCode);
                }
                else
                {
                    model.CreateUser = 0;
                }
                model.CreateTime = DateTime.Now;
                model.IsActive = 1;
                model.TypeName = create.TypeName;
                model.CoverImg = create.CoverImg;
                return Insert(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal int UpdateMaterialType(UpdateMaterialTypeDto obj)
        {
            YY_MaterialType model = Entities.Where(o => o.Id == obj.Id).FirstOrDefault();
            if (model != null)
            {
                model.TypeName = obj.TypeName;
                model.CoverImg = obj.CoverImg;
                model.UpdateUser = Convert.ToInt32(Cookies.UserCode);
                model.UpdateTime = DateTime.Now;
                return Update(model);
            }
            else
            {
                return 0;
            }
        }

        internal int ToggleStauts(int id)
        {
            try
            {
                var model = Entities.Where(o => o.Id == id).FirstOrDefault();
                if (model != null && model.IsActive == 1)
                {
                    model.IsActive = 0;
                }
                else
                {
                    model.IsActive = 1;
                }

                model.UpdateUser = Convert.ToInt32(Cookies.UserCode);
                model.UpdateTime = DateTime.Now;
                return Update(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal string GetQRcodeUrl(int id)
        {
            try
            {
                string str_url = Config.websiteUrl + "/WXViews/Enroll.html?materialtype=" + id + "&test=1";
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

    }
}