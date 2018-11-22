using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
namespace Salary_MVC.DataModel
{
    public class UpdateHistoryActivator<T> where T :BaseEntity
    {
        DataModel.GZ_User operatorUser;
        public UpdateHistoryActivator(DataModel.GZ_User operatorUser)
        {
            if (operatorUser == null)
                throw new ArgumentException("未获取到当前的用户信息！operatorUser不能为null");
            this.operatorUser = operatorUser;
        }
        static string tableName = typeof(T).Name;
        /// <summary>
        /// 创建修改记录
        /// </summary>
        /// <typeparam name="S">属性类型</typeparam>
        /// <param name="old">原记录的对象</param>
        /// <param name="colExp">修改的列</param>
        /// <param name="newValue">新值</param>
        /// <returns></returns>
        public  GZ_UpdateHistory Create<S>(T old, Expression<Func<T,S>> colExp, S newValue)
        {
            var MemExp= colExp.Body as MemberExpression;
            if (MemExp == null)
                throw new ArgumentException("不识别的参数，请输入形如x=>x.Name");
            var oldValue = colExp.Compile()(old);
            DataModel.GZ_UpdateHistory history = new GZ_UpdateHistory() {
                 TargetTable= tableName,
                ColumnName = MemExp.Member.Name,
                CreateDate = DateTime.Now,
                CreateUser =this.operatorUser.Id,
                Id = Guid.NewGuid(),
                NewValue =newValue?.ToString()??string.Empty,
                OldValue = oldValue?.ToString()??string.Empty,
                TargetId = old.Id
            };
            return history;
        }
    }
}