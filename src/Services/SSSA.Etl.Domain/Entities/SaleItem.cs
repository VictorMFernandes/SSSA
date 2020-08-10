namespace SSSA.Etl.Domain.Entities
{
    public class SaleItem
    {
        public Item Item { get; }
        public int Quantity { get; }
        public decimal Value => Item.Price * Quantity;

        public SaleItem(Item item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }
}
