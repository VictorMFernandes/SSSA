namespace SSSA.Etl.Domain.Entities
{
    public class Item
    {
        public string Id { get; }
        public decimal Price { get; }

        public Item(string id, decimal price)
        {
            Id = id;
            Price = price;
        }
    }
}
