using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using Salary.DataModel.Entity;

namespace Salary.Data
{
    //仓储的泛型实现类
    public class EFRepository<TEntity> where TEntity : BaseEntity
    {
        public EFUnitOfWork UnitOfWork { get; set; }

        public EFRepository()
        {
            UnitOfWork = new EFUnitOfWork();
        }

        public virtual IQueryable<TEntity> Entities
        {
            get { return UnitOfWork.context.Set<TEntity>(); }
        }

        public virtual TEntity GetByKey(object key)
        {
            return UnitOfWork.context.Set<TEntity>().Find(key);
        }

        #region 加入linq动态查询

       

        public bool Any(Expression<Func<TEntity, bool>> express)
        {
            Func<TEntity, bool> lamada = express.Compile();
            DbSet<TEntity> ds = UnitOfWork.context.Set<TEntity>();
            return ds.Any(lamada);
        }

        public int Count(Expression<Func<TEntity, bool>> express)
        {
            Func<TEntity, bool> lamada = express.Compile();
            DbSet<TEntity> ds = UnitOfWork.context.Set<TEntity>();
            return ds.Count(lamada);
        }

        #endregion        

        public virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> express)
        {
            //return Entities.Where(express);
            Func<TEntity, bool> lamada = express.Compile();
            DbSet<TEntity> ds = UnitOfWork.context.Set<TEntity>();
            return ds.Where(lamada).AsQueryable<TEntity>();
        }

        public virtual IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> express)
        {
            Func<TEntity, bool> lamada = express.Compile();
            DbSet<TEntity> ds = UnitOfWork.context.Set<TEntity>();
            return ds.Where(lamada).AsEnumerable<TEntity>();
        }

        public virtual int Insert(TEntity entity)
        {
            UnitOfWork.RegisterNew(entity);
            return UnitOfWork.Commit();
        }
        public virtual int Insert(IEnumerable<TEntity> entities)
        {
            foreach (var obj in entities)
            {
                UnitOfWork.RegisterNew(obj);
            }
            return UnitOfWork.Commit();
        }

        public virtual int Delete(object id)
        {
            var obj = UnitOfWork.context.Set<TEntity>().Find(id);
            if (obj == null)
            {
                return 0;
            }
            UnitOfWork.RegisterDeleted(obj);
            return UnitOfWork.Commit();
        }

        public virtual int Delete(TEntity entity)
        {
            UnitOfWork.RegisterDeleted(entity);
            return UnitOfWork.Commit();
        }

        public virtual int Delete(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                UnitOfWork.RegisterDeleted(entity);
            }
            return UnitOfWork.Commit();
        }

        public virtual int Delete(Expression<Func<TEntity, bool>> express)
        {
            Func<TEntity, bool> lamada = express.Compile();
            var lstEntity = UnitOfWork.context.Set<TEntity>().Where(lamada);
            foreach (var entity in lstEntity)
            {
                UnitOfWork.RegisterDeleted(entity);
            }
            return UnitOfWork.Commit();
        }

        public virtual int Update(TEntity entity)
        {
            UnitOfWork.RegisterModified(entity);
            return UnitOfWork.Commit();
        }

        public virtual int Update(IEnumerable<TEntity> entities)
        {
            foreach (var obj in entities)
            {
                UnitOfWork.RegisterModified(obj);
            }
            return UnitOfWork.Commit();
        }
        public IEnumerable<TEntity> GetQueryByPredicateNoPage(string QueryPredicate, object[] ParamValues, string OrderBy, int PageIndex, int PageSize, ref int Count)
        {


            try
            {
                DbSet<TEntity> ds = UnitOfWork.context.Set<TEntity>();

                var query = ds.Where(QueryPredicate, ParamValues);
                Count = query.Count();
                var res = query.OrderBy(OrderBy).Skip(PageSize * (PageIndex - 1)).Take(Count);

                return res;
            }
            catch (Exception ex)
            {
                // LogHelper.WriteErrorLog(ex.Message, ex);

                return null;
            }
        }
    }
}