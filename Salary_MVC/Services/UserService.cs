using Qcloud.Sms;
using Salary.Common;
using Salary_MVC.Common;
using Salary_MVC.Data;
using Salary_MVC.DataModel;
using Salary_MVC.Enum;
using Salary_MVC.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Salary_MVC.DataModel.GZ_User;

namespace Salary_MVC.Services
{
    public class UserService : Service<GZ_User>
    {
        internal List<string> GetPhoneList(RoleEnum role)
        {
            //try
            //{
            //    return (from ur in DbContext.GZ_UserRole
            //            join u in DbContext.GZ_User on ur.UserId equals u.Id into a
            //            from user in a.DefaultIfEmpty()
            //                //where ur.RoleId == (int)role
            //            where user.UserName.Length == 11
            //            select user.UserName).ToList();
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.WriteErrorLog(ex.StackTrace, ex);
            //    throw;
            //}

            var roleInfo= this.DbContext.GZ_Role.Where(x => x.Code == (int)role).FirstOrDefault();
            if (roleInfo == null)
                throw new ArgumentException(string.Format("未找到指定的角色，指定的角色枚举值为{0}，名称为{1}",(int)role),role.ToString());
            var lst= this.DbContext.GZ_UserRole.Where(x => x.RoleId == roleInfo.Id)
                .Join(this.DbContext.GZ_User, x => x.UserId, y => y.Id, (x, y) => y.UserName).ToList();
            return lst;

        }

        /// <summary>
        /// 获取所有正常状态的用户
        /// </summary>
        /// <returns></returns>
        public List<GZ_User> GetAllUser()
        {
            var lst = this.DbContext.GZ_User.ToList();
            return lst;
        }

        internal object GetRoleName()
        {
            var list = DbContext.GZ_Role.ToList();
            return list;
        }

        internal object GetFunctionGroup()
        {
            var list = DbContext.GZ_FunctionGroup.ToList();
            return list;
        }

        internal int AddUser(AddUserDto Add)
        {

            GZ_User model = new GZ_User() { Id = new Guid(Guid.NewGuid().ToString("N")), CreateDate = DateTime.Now, CreateUser = Cookies.UserCode };

            model.UserName = Add.UserName;
            model.Name = Add.Name;
            model.Password = Add.Password;
            model.FunctionGroupId = Add.FunctionGroupId;
            model.Status = UserStatusEnum.Normal;
            this.DbContext.GZ_User.Add(model);

            var list_role = Add.Role.Split(",".ToArray());
            foreach (string item_role in list_role)
            {
                GZ_UserRole model1 = new GZ_UserRole() { Id = new Guid(Guid.NewGuid().ToString("N")), CreateDate = DateTime.Now, CreateUser = Cookies.UserCode };
                model1.RoleId = new Guid(item_role);
                model1.UserId = model.Id;
                this.DbContext.GZ_UserRole.Add(model1);
            }
            return this.DbContext.SaveChanges();
        }

        internal object GetUserList()
        {

            var list = this.DbContext.GZ_User.Where(u => u.Status == UserStatusEnum.Normal).Select(u => new
            {
                Id = u.Id,
                UserName = u.UserName,
                Name = u.Name,
                //Role = from o in this.DbContext.GZ_UserRole join d in this.DbContext.GZ_Role on o.RoleId equals d.Id where o.UserId == u.Id select d.Name,
                Role = this.DbContext.GZ_UserRole.Where(o => o.UserId == u.Id).Join(this.DbContext.GZ_Role, o => o.RoleId, d => d.Id, (o, d) => d.Name),
                FunctionGroupId = this.DbContext.GZ_FunctionGroup.Where(o => o.Id == u.FunctionGroupId).Select(o => o.Name)
            }).ToList();

            return list;
        }

        internal object GetOneUser(Guid id)
        {
            var one_list = this.DbContext.GZ_User.Where(u => u.Id == id).ToList().Select(u => new
            {
                Id = u.Id,
                UserName = u.UserName,
                Name = u.Name,
                Password = u.Password,
                Role = this.DbContext.GZ_UserRole.Where(o => o.UserId == u.Id).Select(o => o.RoleId),
                FunctionGroupId = this.DbContext.GZ_FunctionGroup.Where(o => o.Id == u.FunctionGroupId).Select(o => o.Id)
            }).FirstOrDefault();

            return one_list;
        }

        internal int DelOneUser(Guid id)
        {
            var model = this.DbContext.GZ_User.Where(u => u.Id == id).FirstOrDefault();
            if (model != null)
            {
                return Delete(model);
            }
            else
            {
                return 0;
            }
        }

        internal int UpdateUser(UpdateUserDto update)
        {
            var model = this.DbContext.GZ_User.Where(o => o.Id == update.Id).FirstOrDefault();

            if (model != null)
            {
                model.UserName = update.UserName;
                model.Name = update.Name;
                model.Password = update.Password;
                model.FunctionGroupId = update.FunctionGroup;

                //var model_del = this.DbContext.GZ_UserRole.Where(u => u.UserId == update.Id).FirstOrDefault();

                //if (model_del != null)
                //{
                //    this.DbContext.GZ_UserRole.Remove(model_del);
                //}
                //2018年11月8日
                var lstUserRole = this.DbContext.GZ_UserRole.Where(x => x.UserId == update.Id).ToList();
                this.DbContext.GZ_UserRole.RemoveRange(lstUserRole);

                var list_role = update.Role.Split(",".ToArray());

                foreach (string item_role in list_role)
                {
                    GZ_UserRole model_add = new GZ_UserRole() { Id = new Guid(Guid.NewGuid().ToString("N")), CreateDate = DateTime.Now, CreateUser = Cookies.UserCode };
                    model_add.RoleId = new Guid(item_role);
                    model_add.UserId = model.Id;
                    this.DbContext.GZ_UserRole.Add(model_add);
                }
                int i = Update(model);
                return i;
            }
            else
            {
                return 0;
            }
        }

        private void Delete(GZ_UserRole model_del)
        {
            throw new NotImplementedException();
        }


        internal List<GZ_Function> GetOneUserFunctionRight(Guid? id)
        {
            //var list = this.DbContext.GZ_UserFunctionRight.Where(u => u.UserId == id).Select(u => new 
            //{
            //    Function = u.FunctionId,
            //    URL = from f in this.DbContext.GZ_Function where f.Id == u.FunctionId select f.URL,
            //    Ico = from f in this.DbContext.GZ_Function where f.Id == u.FunctionId select f.Ico
            //}).ToList();

            var list = from r in this.DbContext.GZ_UserFunctionRight
                       join f in this.DbContext.GZ_Function on r.FunctionId equals f.Id
                       where r.UserId == id
                       select f;

            return list.OrderBy(o => o.Order).ToList();
        }

        internal List<GZ_Function> GetOneGroupFunctionRight(Guid? id)
        {
            //var list = this.DbContext.GZ_FunctionGroupRight.Where(u => u.FunctionGroupId == id).Select(u => new
            //{
            //    FunctionGroup = u.FunctionGroupId,
            //    URL = from f in this.DbContext.GZ_Function where f.Id == u.FunctionId select f.URL,
            //    Ico = from f in this.DbContext.GZ_Function where f.Id == u.FunctionId select f.Ico
            //}).ToList();

            //return list;

            var list = from r in this.DbContext.GZ_FunctionGroupRight
                       join f in this.DbContext.GZ_Function on r.FunctionId equals f.Id
                       where r.FunctionGroupId == id
                       select f;

            return list.OrderBy(o => o.Order).ToList();


        }

        //获取用户与用户组权限的交集
        internal List<GZ_Function> GetGroupAndUserFunctionRight()
        {
            var list_user_right = GetOneUserFunctionRight(Cookies.UserCode);//获取一个用户的权限列表

            var list_group_right = GetOneGroupFunctionRight(Cookies.FunctionGroupId);//获取一个用户组的权限列表

            var list_guid = new List<string>(); //保持guid字符串
            var list_group_and_user_right = new List<GZ_Function>();//返回列表

            foreach (GZ_Function item_right in list_user_right)//处理用户权限
            {
                string str_guid = item_right.Id.ToString();//取出这个功能的Guid
                if (!list_guid.Contains(str_guid))//如果list_guid不包含这个Guid
                {
                    list_guid.Add(str_guid);//把Guid加入list_guid中
                    list_group_and_user_right.Add(item_right);//把这个功能加入返回列表中
                }
            }

            foreach (GZ_Function item_right in list_group_right)//处理用户组权限
            {
                string str_guid = item_right.Id.ToString();
                if (!list_guid.Contains(str_guid))
                {
                    list_guid.Add(str_guid);
                    list_group_and_user_right.Add(item_right);
                }
            }

            return list_group_and_user_right;
        }

        //由于暂时没有登录需要显示全部功能出来
        internal List<GZ_Function> GetGroupAndUserFunctionRight1()
        {
            var list = from o in this.DbContext.GZ_Function select o;
            return list.OrderBy(o => o.Order).ToList();
        }

        internal List<Models.MenuDto> GetFunctionRightDto()
        {
            List<Models.MenuDto> list_menu = new List<Models.MenuDto>();
            List<GZ_Function> list_function = GetGroupAndUserFunctionRight();//调用上面的函数获取该用户所有的权限列表

            List<GZ_Function> list_function1 = this.DbContext.GZ_Function.Where(o => o.ParentId == null).OrderBy(o => o.Order).ToList();//1级菜单


            //1、把所有的1级菜单加进去
            foreach (GZ_Function item_function in list_function1)
            {
                Models.MenuDto item_menu1 = new Models.MenuDto() { id = item_function.Id, text = item_function.Name, icon = item_function.Ico };
                //查询1级菜单是否有二级菜单，有就直接加入进去
                var list2 = list_function.Where(o => o.ParentId == item_function.Id);
                if (list2 != null)
                {
                    var list_children = new List<Models.MenuDto>();
                    foreach (GZ_Function item_function2 in list2)
                    {
                        Models.MenuDto item_menu2 = new Models.MenuDto() { id = item_function2.Id, text = item_function2.Name, icon = item_function2.Ico, url = item_function2.URL, targetType = "iframe-tab", urlType = "abosulte" };
                        list_children.Add(item_menu2);
                    }
                    item_menu1.children = list_children;
                }
                if (item_menu1.children.Count > 0)
                {
                    list_menu.Add(item_menu1);
                }
            }
            return list_menu;
        }

        internal GZ_User Login(UserDto dto)
        {
            var list = this.DbContext.GZ_User.Where(u => u.Status == UserStatusEnum.Normal && u.UserName == dto.UserName).ToList().Where(u => u.Password == dto.Password).FirstOrDefault();
            return list;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        internal GZ_User GetUserInfo(Guid id)
        {
            var list = this.DbContext.GZ_User.Where(u => u.Id == id).FirstOrDefault();
            return list;
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int UpdatePassword(UpdatePwdDto dto)
        {
            var model = this.DbContext.GZ_User.Where(u => u.Id == dto.Id).FirstOrDefault();

            if (model == null)
            {
                return -1;
            }

            else if (model.Password != dto.OldPwd)
            {
                return -2;
            }

            model.Password = dto.NewPwd;
            return Update(model);
        }


        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <returns></returns>
        internal int ForgetPassword(Guid id)
        {
            var model = this.DbContext.GZ_User.Where(u => u.Id == id).FirstOrDefault();

            if (model != null)
            {
                UpdatePwdMessage(model.UserName, model.Password);
                return 1;
            }
            return 0;
        }

        internal GZ_User GetUserByPhone(string UserName)
        {
            var model = this.DbContext.GZ_User.Where(u => u.UserName == UserName).FirstOrDefault();
            return model;
        }


        private void UpdatePwdMessage(string phone, string pwd)
        {
            SmsSingleSenderResult singleResult;
            SmsSingleSender singleSender = new SmsSingleSender(Config.sdkappid, Config.appkey);
            List<string> templParams = new List<string>();
            templParams.Add(pwd);
            singleResult = singleSender.SendWithParam("86", phone, 221470, templParams, "", "", "");
        }









    }
}