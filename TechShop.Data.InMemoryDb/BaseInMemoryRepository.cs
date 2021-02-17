using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TechShop.Contracts.Data;
using TechShop.Contracts.Entities;

namespace TechShop.Data.InMemoryDb
{
    public abstract class BaseInMemoryRepository<TEntity, TKey> : BaseRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : struct
    {
        protected BaseInMemoryRepository()
        {
            Entities = new Dictionary<TKey, TEntity>();
        }

        protected Dictionary<TKey, TEntity> Entities { get; private set; }

        public override async Task<IEnumerable<TEntity>> GetWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.FromResult(Entities.Values.Where(predicate.Compile().Invoke).ToArray());
        }
    }
}