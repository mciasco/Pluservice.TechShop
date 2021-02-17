using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShop.Contracts.Data;
using TechShop.Domain;

namespace TechShop.Data.InMemoryDb
{
    public class InMemoryCategoriesRepository : BaseInMemoryRepository<Categoria, int>, ICategoriesRepository
    {
        public override async Task<IEnumerable<Categoria>> GetAll()
        {
            return await Task.FromResult(this.Entities.Values);
        }

        public override async Task<Categoria> GetByKey(int key)
        {
            return await Task.FromResult(
                Entities.TryGetValue(key, out var foundItem) ? foundItem : null);
        }

        public override async Task Add(Categoria newEntity)
        {
            if(!Entities.TryGetValue(newEntity.Id, out var existingItem))
                Entities.Add(newEntity.Id, newEntity);
            await Task.CompletedTask;
        }

        public async Task<Categoria> GetByDescription(string description)
        {
            return await Task.FromResult(
                Entities.Values.FirstOrDefault(c => c.Description.Equals(description, StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}
