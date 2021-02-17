using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechShop.Contracts.Entities;

namespace TechShop.Contracts.Data
{
    public interface IRepository
    {
    }

    public interface  IRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : struct
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> GetByKey(TKey key);
        Task Add(TEntity newEntity);
        Task<IEnumerable<TEntity>> GetWhere(Expression<Func<TEntity, bool>> predicate);
    }


    public abstract class BaseRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : struct
    {
        protected BaseRepository()
        {
        }

        public abstract Task Add(TEntity newEntity);
        public abstract Task<IEnumerable<TEntity>> GetWhere(Expression<Func<TEntity, bool>> predicate);
        public abstract Task<IEnumerable<TEntity>> GetAll();
        public abstract Task<TEntity> GetByKey(TKey key);
    }
    

    public interface IRepositoryInitializer
    {
    }




}
