using System;

namespace TechShop.Domain
{
    public class Prodotto
    {
        public string Description { get; set; }
        public int Id { get; set; }
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
