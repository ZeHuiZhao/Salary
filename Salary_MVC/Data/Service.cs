using Salary_MVC.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;

namespace Salary_MVC.Data
{
    //仓储的泛型实现类
    public class Service<TEntity> where TEntity : BaseEntity
    {
        //public EFUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// 当前登录的用户
        /// </summary>
        protected GZ_User UserInfo
        {
            get
            {
                return Cookies.User;
                //var tmp = System.Runtime.Remoting.Messaging.CallContext.GetData("__@UserInfo@__") as GZ_User;
                //tmp = tmp ?? new GZ_User() { UserName = "15616104092", Id = Guid.Empty, Name = "system" };//如果程序后台线程取当前的登陆用户，那么就设定为system
                //return tmp;
            }
        }

        protected   GZDbContext DbContext { set; get; }

        public Service()
        {
            //UnitOfWork = new EFUnitOfWork();
            this.DbContext = System.Runtime.Remoting.Messaging.CallContext.GetData("__@DbContext@__") as GZDbContext;
            if(this.DbContext==null)
            {//单次请求内共享同一个dbcontext, 在两个service内分别增删，然后再某个地方SaveChanges进行事物操作；
                this.DbContext = new GZDbContext();
                System.Runtime.Remoting.Messaging.CallContext.SetData("__@DbContext@__", this.DbContext);
            }
        }

        public virtual IQueryable<TEntity> Entities
        {
            get { return this.DbContext.Set<TEntity>(); }
        }

        
        public virtual int Delete(TEntity entity)
        {
            this.DbContext.Entry(entity).State = EntityState.Deleted;
            return this.DbContext.SaveChanges();
        }


        public virtual int Update(TEntity entity)
        {
            this.DbContext.Entry(entity).State = EntityState.Modified;
            return this.DbContext.SaveChanges();
        }

        public virtual int Update(IEnumerable<TEntity> entities)
        {
            foreach (var obj in entities)
            {
                this.DbContext.Entry(obj).State = EntityState.Modified;
            }
            return this.DbContext.SaveChanges();
        }
        
    }
}