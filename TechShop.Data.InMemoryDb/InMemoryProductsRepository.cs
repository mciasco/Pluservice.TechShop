using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechShop.Domain;

namespace TechShop.Data.InMemoryDb
{
    public class InMemoryProductsRepository : BaseInMemoryRepository<Prodotto, int>, IProductsRepository
    {
        public override async Task<IEnumerable<Prodotto>> GetAll()
        {
            return await Task.FromResult(this.Entities.Values);
        }

        public override async Task<Prodotto> GetByKey(int key)
        {
            return await Task.FromResult(Entities.TryGetValue(key, out var foundItem) ? foundItem : null);
        }

        public override async Task Add(Prodotto newEntity)
        {
            if (!Entities.TryGetValue(newEntity.Id, out var existingItem))
                Entities.Add(newEntity.Id, newEntity);
            await Task.CompletedTask;
        }
    }
}