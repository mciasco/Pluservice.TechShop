namespace TechShop.Domain
{
    public class Categoria
    {
        public int Id { get; set; }
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