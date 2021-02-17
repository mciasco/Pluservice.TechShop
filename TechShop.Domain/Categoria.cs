using System.Threading.Tasks;
using TechShop.Contracts.Data;
using TechShop.Contracts.Entities;

namespace TechShop.Domain
{
    public interface ICategoriesRepository : IRepository<Categoria, int>
    {
        Task<Categoria> GetByDescription(string description);
    }


    public class Categoria : BaseEntity<int>
    {
        public string Description { get; set; }

        internal Categoria()
        {

        }

        public Categoria(int id, string description)
        {
            Id = id;
            Description = description;
        }
    }
}