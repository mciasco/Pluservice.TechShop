using System;
using TechShop.Contracts.Data;
using TechShop.Contracts.Entities;

namespace TechShop.Domain
{
    public interface IProductsRepository : IRepository<Prodotto, int>
    {
    }


    public class Prodotto : BaseEntity<int>
    {
        public string Description { get; set; }
        public Categoria ParentCatergory { get; set; }

        internal Prodotto()
        {
        }

        public Prodotto(int id, string description, Categoria parentCatergory)
        {
            Id = id;
            Description = description;
            ParentCatergory = parentCatergory;
        }
    }
}
