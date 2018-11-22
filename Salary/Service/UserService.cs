using Newtonsoft.Json;
using Qcloud.Sms;
//using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Salary.Common;
using Salary.Data;
using Salary.DataModel.Entity;
using Salary.Models.Base;
using Salary.Models.Home;
using Salary.Models.User;

namespace Salary.Service
{
    public class UserService : EFRepository<YY_Userinfo>
    {
        public List<UserinfoDto> GetUserList(int id)
        {
            try
            {
                var list = Entities.Where(o => o.UserStatus <= (int)UserStatusEnum.Normal && o.ChannelId == id && (o.UserType == (int)UserTypeEnum.ChannelManager || o.UserType == (int)UserTypeEnum.Sales)).OrderBy(o => o.UserType).Select(o => new UserinfoDto { UserType = o.UserType, TrueName = o.TrueName, PhoneNum = o.PhoneNum, Job = o.UserType == (int)UserTypeEnum.ChannelManager ? "渠道管理员" : o.UserType == (int)UserTypeEnum.ZLChannelManager ? "中力渠道管理员" : o.UserType == (int)UserTypeEnum.Sales ? "渠道销售员" : o.UserType == (int)UserTypeEnum.Admin ? "超级管理员" : "", Id = o.Id, UserStatus = o.UserStatus });
                return list.ToList();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return new List<UserinfoDto>();
            }
        }


        public List<YY_Userinfo> GetUserList()
        {
            return Entities.ToList();
        }

        public int AddUser(Userinfo user)
        {

            try
            {
                var model = new YY_Userinfo { ChannelId = user.ChannelId, PhoneNum = user.PhoneNum, TrueName = user.TrueName, UserType = user.UserType, IsContacts = user.IsContacts, HeadImage = user.HeadImage, UserStatus = user.UserStatus };
                var isExists = Entities.Where(o => o.PhoneNum == model.PhoneNum && o.UserType == model.UserType && o.UserStatus != (int)UserStatusEnum.Deleted).FirstOrDefault();
                if (isExists != null)
                {
                    return 2;
                }
                model.CreateUser = string.IsNullOrEmpty(Cookies.UserCode) ? 0 : Convert.ToInt32(Cookies.UserCode);
                model.CreateTime = DateTime.Now;
                var pwd = this.RandomPwd(4);

                model.UserPwd = new Password_Encrypt_ASC.Password_Encrypt_ASC().set_password_ASC(pwd);
                int result = Insert(model);
                if (result == 1)
                {
                    SendMessage(model.PhoneNum, pwd);
                }
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }

        }

        internal YY_Userinfo ErpLogin(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return null;
                }
                //token = token.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+");
               // token = HttpUtility.UrlEncode(token, System.Text.Encoding.UTF8);
                var pea = new Password_Encrypt_ASC.Password_Encrypt_ASC();
                token = HttpUtility.UrlDecode(token);
                token = pea.get_password_ASC(token);
                ErpModel erpModel = JsonConvert.DeserializeObject(token.TrimStart('[').TrimEnd(']'),typeof(ErpModel)) as ErpModel;
                if (erpModel.CreateDate.Date != DateTime.Now.Date) return null;
                if (erpModel == null) return null;
                var isExists = Entities.Where(o => o.UserType == (int)UserTypeEnum.ZLChannelManager && o.PhoneNum == erpModel.UserPhone).FirstOrDefault();
                if (isExists != null) return isExists;
                var user = new YY_Userinfo() { ChannelId = 0, CreateTime = DateTime.Now, CreateUser = 0, HeadImage = erpModel.UserHeadImage, IsContacts = 0, PhoneNum = erpModel.UserPhone, TrueName = erpModel.UserName, UserPwd = erpModel.UserPassword, UserStatus = 1, UserType = 1 };
                var i = Insert(user);
                if (i > 0)
                {
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }

        //public int AddUser(CreateWechatUser user)
        //{
        //    try
        //    {
        //        var model = new YY_Userinfo { OpenId = user.OpenId, ChannelId = user.ChannelId, PhoneNum = user.PhoneNum, TrueName = user.TrueName, UserType = user.UserType, IsContacts = 0, HeadImage = "", WechatHeadImage = "", UserStatus = user.UserStatus, CreateTime = DateTime.Now, CreateUser = Convert.ToInt32(Cookies.UserCode) };
        //        var isExists = Entities.Where(o => o.PhoneNum == model.PhoneNum && o.UserStatus == model.UserStatus && o.UserStatus != (int)UserStatusEnum.Deleted).FirstOrDefault();
        //        if (isExists != null)
        //        {
        //            return 2;
        //        }
        //        var isOpenId = Entities.Where(o => o.OpenId == user.OpenId).FirstOrDefault();
        //        if (isOpenId != null)
        //        {
        //            return 3;
        //        }
        //        var pwd = this.RandomPwd(4);

        //        model.UserPwd = pwd;
        //        var wxUser = UserApi.Info(WxLib.WXP_Config.appID, user.OpenId);
        //        model.WechatHeadImage = wxUser.headimgurl;
        //        int result = Insert(model);

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteErrorLog(ex.Message, ex);
        //        return 0;
        //    }
        //}

        /// <summary>
        ///  2.用户不存在 3.密码不匹配
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int CustomPwd(UpdatePwd dto)
        {
            try
            {
                var pea = new Password_Encrypt_ASC.Password_Encrypt_ASC();
                var model = Entities.Where(o => o.Id == dto.Id).FirstOrDefault();
                if (model == null)
                {
                    return -1;
                }
                if (model.UserPwd != pea.set_password_ASC(dto.OldPwd))
                {
                    return -2;
                }
                var pwd = pea.set_password_ASC(dto.NewPwd);
                model.UserPwd = pwd;
                return Update(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        /// <summary>
        /// 发新增消息
        /// </summary>
        private void SendMessage(string phone, string pwd)
        {
            SmsSingleSenderResult singleResult;
            SmsSingleSender singleSender = new SmsSingleSender(Config.sdkappid, Config.appkey);
            List<string> templParams = new List<string>();
            templParams.Add(phone);
            templParams.Add(pwd);
            templParams.Add(Config.websiteUrl);
            
            singleResult = singleSender.SendWithParam("86", phone, 196387, templParams, "", "", "");
        }

        private void UpdatePwdMessage(string phone, string pwd)
        {
            SmsSingleSenderResult singleResult;
            SmsSingleSender singleSender = new SmsSingleSender(Config.sdkappid, Config.appkey);
            List<string> templParams = new List<string>();
            templParams.Add(pwd);
            singleResult = singleSender.SendWithParam("86", phone, 196418, templParams, "", "", "");
        }


        internal object GetChannelTypeList()
        {
            try
            {
                return UnitOfWork.context.Database.SqlQuery<BaseFilter>("select CBC_ID as Id, CBC_CATEGORY_NAME as Name from AP_CHANNEL_BUSINESS_CATEGORY").ToList();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }

        internal List<ChannelModel> GetChannelList(QueryChannelDto dto)
        {
            try
            {
                string sql = " select CB_ID as Id,CB_BUSINESS_NAME as Name,CB_YY_ACTIVE as IsActive from AP_CHANNEL_BUSINESS where CB_YY_ACTIVE=1 ";
                List<SqlParameter> paras = new List<SqlParameter>();
                if (dto.Type.HasValue)
                {
                    sql = sql + " and CB_CATEGORY_ID = @CB_CATEGORY_ID ";
                    paras.Add(new SqlParameter("@CB_CATEGORY_ID", dto.Type.Value));
                }
                if (!string.IsNullOrEmpty(dto.ChannelName))
                {
                    sql = sql + " and CB_BUSINESS_NAME like @CB_BUSINESS_NAME ";
                    paras.Add(new SqlParameter("@CB_BUSINESS_NAME", "%" + dto.ChannelName + "%"));
                }
                return UnitOfWork.context.Database.SqlQuery<ChannelModel>(sql, paras.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }

        public int GetActiveChannelCount()
        {
            try
            {
                string sql = " select count(1) from AP_CHANNEL_BUSINESS where CB_YY_ACTIVE=1 ";                
                return UnitOfWork.context.Database.SqlQuery<int>(sql).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        public int GetSalesCount(int? channelId)
        {
            try
            {
                var a = Entities.Where(o=>o.UserStatus==(int)UserStatusEnum.Normal&&o.UserType==(int)UserTypeEnum.Sales);
                if (channelId.HasValue)
                {
                    a = a.Where(o=>o.ChannelId==channelId.Value);
                }
                return a.Count();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        public int AddUser(List<Userinfo> list)
        {
            try
            {
                if (list != null && list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        int result = this.AddUser(list[i]);
                        if (result != 1)
                        {
                            return result;
                        }
                    }
                }
                else
                {
                    return 0;
                }
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        public int ResetPassword(int id)
        {
            try
            {
                var model = Entities.Where(o => o.Id == id).FirstOrDefault();
                if (model != null)
                {
                    var pwd = this.RandomPwd(4);
                    model.UserPwd = new Password_Encrypt_ASC.Password_Encrypt_ASC().set_password_ASC(pwd);
                    int result = Update(model);
                    if (result == 1)
                    {
                        UpdatePwdMessage(model.PhoneNum, pwd);
                    }
                    return result;
                }
                return 0;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }

        }

        public int ResetPassword(List<int> list)
        {
            try
            {
                if (list != null && list.Count > 0)
                {

                    for (int i = 0; i < list.Count; i++)
                    {
                        int result = this.ResetPassword(list[i]);
                        if (result != 1)
                        {
                            return 0;
                        }
                    }

                }
                else
                {
                    return 0;
                }

                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }

        }       

        internal int ToggleChannelUser(ToggleChannelStatus status)
        {
            try
            {
                var model = Entities.Where(o => o.Id == status.Id).FirstOrDefault();
                model.UserStatus = status.UserStatus;
                return Update(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        private string RandomPwd(int length)
        {
            try
            {
                string reValue = string.Empty;
                Random r = new Random();
                while (reValue.Length < length)
                {
                    string s1 = r.Next(0, 10).ToString();
                    reValue += s1;
                }
                return reValue;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return string.Empty;
            }
        }

        public int UpdateUser(UpdateUserDto user)
        {
            try
            {
                var model = Entities.Where(o => o.Id == user.Id).FirstOrDefault();
                if (model != null)
                {
                    model.TrueName = user.TrueName;
                    model.PhoneNum = user.PhoneNum;
                    if (!string.IsNullOrEmpty(Cookies.UserCode))
                    {
                        model.UpdateUser = Convert.ToInt32(Cookies.UserCode);
                    }
                    model.UpdateTime = DateTime.Now;
                    int result = Update(model);
                    if (result == 1 && model.IsContacts > 0)
                    {
                        UnitOfWork.context.Database.ExecuteSqlCommand("UPDATE AP_CHANNEL_USER SET CN_PHONE=@CN_PHONE,CN_NAME=@CN_NAME WHERE CN_USER_ID=@CN_USER_ID", new SqlParameter("@CN_PHONE", model.PhoneNum), new SqlParameter("@CN_NAME", model.TrueName), new SqlParameter("@CN_USER_ID", model.IsContacts));
                    }
                    return result;
                }
                return 0;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal int UpdateChannelUser(UpdateChannelUserDto dto)
        {
            try
            {
                var model = Entities.Where(o => o.Id == dto.Id).FirstOrDefault();
                if (model != null)
                {
                    model.TrueName = dto.TrueName;
                    model.PhoneNum = dto.PhoneNum;
                    model.UserStatus = dto.UserStatus;
                    if (!string.IsNullOrEmpty(Cookies.UserCode))
                    {
                        model.UpdateUser = Convert.ToInt32(Cookies.UserCode);
                    }
                    model.UpdateTime = DateTime.Now;
                    int result = Update(model);
                    if (result == 1 && model.IsContacts > 0)
                    {
                        UnitOfWork.context.Database.ExecuteSqlCommand("UPDATE AP_CHANNEL_USER SET CN_PHONE=@CN_PHONE,CN_NAME=@CN_NAME WHERE CN_USER_ID=@CN_USER_ID", new SqlParameter("@CN_PHONE", model.PhoneNum), new SqlParameter("@CN_NAME", model.TrueName), new SqlParameter("@CN_USER_ID", model.IsContacts));
                    }
                    return result;
                }
                return 0;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal int DeleteUser(List<int> deleteList)
        {
            var list = Entities.Where(o => deleteList.Contains(o.Id)).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].UserStatus = (int)UserStatusEnum.Deleted;
            }
            return Update(list);
        }

        internal object GetSaleList(int id)
        {
            try
            {
                return Entities.Where(o => o.ChannelId == id && o.UserType == (int)UserTypeEnum.Sales).Select(o => new { Id = o.Id, TrueName = o.TrueName }).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }

        internal object GetAllRole()
        {
            try
            {
                return UnitOfWork.context.Set<YY_Role>().Select(o => new { Name = o.Name, NavigateId = o.NavigateId, Id = o.Id }).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }

        internal QueryUserPaging GetUserListByPaging(QueryUserDto query)
        {
            var model = new QueryUserPaging();
            try
            {
                int skip = query.PageSize * (query.PageIndex - 1);
                var a = Entities.Where(o => o.UserType == query.UserType);
                if (Cookies.UserType != (int)UserTypeEnum.Admin)
                {
                    a = a.Where(o => o.ChannelId == Cookies.ChannelId);
                }
                if (query.UserType == (int)UserTypeEnum.ChannelManager)
                {
                    a = a.Where(o => o.UserStatus == (int)UserStatusEnum.Normal || o.UserStatus == (int)UserStatusEnum.Disable);
                }

                if (query.UserStatus.HasValue)
                {
                    a = a.Where(o => o.UserStatus == query.UserStatus.Value);
                }
                if (!string.IsNullOrEmpty(query.TrueName))
                {
                    a = a.Where(o => o.TrueName.Contains(query.TrueName));
                }
                if (!string.IsNullOrEmpty(query.PhoneNum))
                {
                    a = a.Where(o => o.PhoneNum.Contains(query.PhoneNum));
                }
                var list = a.OrderByDescending(o => o.CreateTime);
                model.UserList = list.Skip(skip).Take(query.PageSize).ToList().Select((o, i) => new UserinfoDto() { DisplayName = "用户24小时内才能删除", IsDelete = o.CreateTime.AddDays(1) > DateTime.Now ? 1 : 0, RowNum = ++i + query.PageSize * (query.PageIndex - 1), TrueName = o.TrueName, PhoneNum = o.PhoneNum, WechatHeadImage = o.WechatHeadImage, Job = o.UserType == (int)UserTypeEnum.ChannelManager ? "渠道管理员" : o.UserType == (int)UserTypeEnum.ZLChannelManager ? "中力渠道管理员" : o.UserType == (int)UserTypeEnum.Sales ? "渠道销售员" : o.UserType == (int)UserTypeEnum.Admin ? "超级管理员" : "", Id = o.Id, UserStatus = o.UserStatus, UserType = o.UserType, UserStatusName = o.UserStatus == 0 ? "取消开通" : o.UserStatus == 1 ? "可用" : o.UserStatus == 2 ? "停用" : o.UserStatus == 3 ? "已删除" : "未审核" }).ToList();
                model.CurrentPage = query.PageIndex;
                model.PageSize = query.PageSize;
                model.TotalCount = list.Count();
                model.TotalPage = (int)Math.Ceiling(model.TotalCount * 1.0 / model.PageSize);
                return model;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return model;
            }
        }

        internal int ToggleAuditStatus(UserStatus status)
        {
            try
            {
                var model = Entities.Where(o => o.Id == status.Id).FirstOrDefault();
                if (model != null)
                {
                    model.UserStatus = status.Status;
                    if (!string.IsNullOrEmpty(Cookies.UserCode))
                    {
                        model.UpdateUser = Convert.ToInt32(Cookies.UserCode);
                    }
                    model.UpdateTime = DateTime.Now;
                    var pwd = this.RandomPwd(4);
                    model.UserPwd = new Password_Encrypt_ASC.Password_Encrypt_ASC().set_password_ASC(pwd);
                    int result = Update(model);
                    if (result == 1)
                    {
                        SendMessage(model.PhoneNum, pwd);
                    }
                    return result;
                }
                return 0;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal int UpdateRolePrivilege(RolePrivilege dto)
        {
            try
            {
                var model = UnitOfWork.context.Set<YY_Role>().Find(dto.Id);
                model.NavigateId = dto.Privilege;
                if (!string.IsNullOrEmpty(Cookies.UserCode))
                {
                    model.UpdateUser = Convert.ToInt32(Cookies.UserCode);
                }
                model.UpdateTime = DateTime.Now;
                return UnitOfWork.context.SaveChanges();

            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        public int ToggleUser(int id)
        {
            try
            {
                var model = Entities.Where(o => o.Id == id).FirstOrDefault();
                if (model != null && model.UserStatus <= (int)UserStatusEnum.Normal)
                {
                    model.UserStatus = model.UserStatus == (int)UserStatusEnum.Normal ? (int)UserStatusEnum.NotFound : (int)UserStatusEnum.Normal;
                    if (!string.IsNullOrEmpty(Cookies.UserCode))
                    {
                        model.UpdateUser = Convert.ToInt32(Cookies.UserCode);
                    }
                    model.UpdateTime = DateTime.Now;
                    return Update(model);
                }
                return 0;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        public int ToggleUser(UserStatus status)
        {
            try
            {
                var model = Entities.Where(o => o.Id == status.Id).FirstOrDefault();
                model.UserStatus = status.Status;
                return Update(model);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        public YY_Userinfo ToggleRole(RoleModel model)
        {
            try
            {
                return Entities.Where(o => o.PhoneNum == model.PhoneNum && o.UserType == model.UserType).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return new YY_Userinfo();
            }

        }

        public UserinfoDto GetUserById(int id)
        {
            try
            {
                return Entities.Where(o => o.Id == id).Select(o => new UserinfoDto { Id = o.Id, TrueName = o.TrueName, PhoneNum = o.PhoneNum, UserStatus = o.UserStatus }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return new UserinfoDto();
            }
        }

        public List<UserinfoDto> GetUserByPhone(string phoneNum)
        {
            try
            {
                return Entities.Where(o => o.PhoneNum == phoneNum).Select(o => new UserinfoDto { Id = o.Id, TrueName = o.TrueName, PhoneNum = o.PhoneNum }).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return new List<UserinfoDto>();
            }
        }

        public int GetChannelStatus(int id)
        {
            try
            {
                int? result = UnitOfWork.context.Database.SqlQuery<int?>("SELECT CB_YY_ACTIVE FROM AP_CHANNEL_BUSINESS WHERE CB_ID=@ID", new SqlParameter("@ID", id)).FirstOrDefault();
                if (result.HasValue)
                {
                    return result.Value;
                }
                return 0;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal int UpdateChannelStatus(ChannelStatue channel)
        {
            try
            {
                int result = UnitOfWork.context.Database.ExecuteSqlCommand("UPDATE AP_CHANNEL_BUSINESS SET CB_YY_ACTIVE=@CB_YY_ACTIVE WHERE CB_ID=@ID", new SqlParameter("@ID", channel.Id), new SqlParameter("@CB_YY_ACTIVE", channel.Status));

                return result;

            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return 0;
            }
        }

        internal ChannelModel GetChannel(int id)
        {
            try
            {
                var result = UnitOfWork.context.Database.SqlQuery<ChannelModel>("select CB_ID as Id,CB_BUSINESS_NAME as Name,CB_YY_ACTIVE as IsActive from AP_CHANNEL_BUSINESS WHERE CB_ID=@ID", new SqlParameter("@ID", id)).FirstOrDefault();

                return result;

            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }


        internal ChannelModel GetChannel(string channelName)
        {
            try
            {
                var result = UnitOfWork.context.Database.SqlQuery<ChannelModel>("select CB_ID as Id,CB_BUSINESS_NAME as Name,CB_YY_ACTIVE as IsActive from AP_CHANNEL_BUSINESS WHERE CB_BUSINESS_NAME=@CB_BUSINESS_NAME", new SqlParameter("@CB_BUSINESS_NAME", channelName)).FirstOrDefault();

                return result;

            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }


        public YY_Userinfo Login(LoginModel model)
        {
            try
            {
                var pwd = new Password_Encrypt_ASC.Password_Encrypt_ASC().set_password_ASC(model.UserPwd);
                var user = Entities.Where(o => o.UserStatus == (int)UserStatusEnum.Normal && o.PhoneNum == model.PhoneNum && o.UserPwd == pwd).FirstOrDefault();
                if (user.UserType != (int)UserTypeEnum.Admin)
                {
                    int? active = UnitOfWork.context.Database.SqlQuery<int?>("SELECT CB_YY_ACTIVE FROM AP_CHANNEL_BUSINESS WHERE CB_ID=@ID", new SqlParameter("@ID", user.ChannelId)).FirstOrDefault();
                    if (!active.HasValue || active.Value == 0)
                    {
                        return null;
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }

        public YY_Userinfo AutoLogin(int id)
        {
            try
            {
                return Entities.Where(o => o.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }


        public object GetRoleList(string phoneNum)
        {
            try
            {
                var user = Entities.Where(o => o.PhoneNum == phoneNum && o.UserStatus == (int)UserStatusEnum.Normal);
                var b = from u in user
                        join r in UnitOfWork.context.Set<YY_Role>() on u.UserType equals r.Id
                        // where r.Id <= (int)UserTypeEnum.Sales
                        select new { Id = r.Id, Name = r.Name } into s
                        group s by new { Id = s.Id, Name = s.Name } into m
                        select new { Id = m.Key.Id, Name = m.Key.Name };
                return b.ToList();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// 根据用户的openid查询用户
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public YY_Userinfo GetSalesUser(string openId)
        {
            try
            {
                return Entities.Where(o => o.OpenId == openId && o.UserStatus != (int)UserStatusEnum.Deleted).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex.StackTrace, ex);
                return null;
            }
        }

        public YY_Userinfo GetUserinfoById(int id)
        {
            return Entities.Where(o => o.Id == id).FirstOrDefault();
        }
    }
}