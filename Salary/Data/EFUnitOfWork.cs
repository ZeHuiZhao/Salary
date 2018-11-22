using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Salary.DataModel.Entity;

namespace Salary.Data
{
    /// <summary>
    /// 工作单实现类
    /// </summary>
    //[Export(typeof(IEFUnitOfWork))]
    public class EFUnitOfWork 
    
    {
        #region 属性
        public DbContext context { get; set; }
        #endregion

        #region 构造函数
        public EFUnitOfWork()
        {
            context = DbContextFactory.GetCurrentDbContext();
        }
        #endregion

        #region IUnitOfWorkRepositoryContext接口
        public void RegisterNew<TEntity>(TEntity obj) where TEntity : BaseEntity
        {
            var state = context.Entry(obj).State;
            if (state == EntityState.Detached)
            {
                context.Entry(obj).State = EntityState.Added;
            }
            IsCommitted = false;
        }

        public void RegisterModified<TEntity>(TEntity obj) where TEntity : BaseEntity
        {
            if (context.Entry(obj).State == EntityState.Detached)
            {
                context.Set<TEntity>().Attach(obj);
            }
            context.Entry(obj).State = EntityState.Modified;
            IsCommitted = false;
        }

        public void RegisterDeleted<TEntity>(TEntity obj) where TEntity : BaseEntity
        {
            context.Entry(obj).State = EntityState.Deleted;
            IsCommitted = false;
        }
        #endregion

        #region IUnitOfWork接口

        public bool IsCommitted { get; set; }

        public int Commit()
        {
            if (IsCommitted)
            {
                return 0;
            }
            try
            {
                int result = context.SaveChanges();
                IsCommitted = true;
                return result;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                throw e;
            }
        }

        public void Rollback()
        {
            IsCommitted = false;
        }
        #endregion

        #region IDisposable接口
        public void Dispose()
        {
            if (!IsCommitted)
            {
                Commit();
            }
            context.Dispose();
        }
        #endregion
    }
}