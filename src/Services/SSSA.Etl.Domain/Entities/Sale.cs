using System.Collections.Generic;
using System.Linq;

namespace SSSA.Etl.Domain.Entities
{
    public class Sale
    {
        public string Id { get; }
        public string SalesmanName { get; }
        private readonly IEnumerable<SaleItem> _saleItems;
        public IReadOnlyCollection<SaleItem> SaleItems => _saleItems.ToList().AsReadOnly();
        public decimal Total => _saleItems.Sum(x => x.Value);

        public Sale(string id, string salesmanName, IEnumerable<SaleItem> saleItems)
        {
            Id = id;
            SalesmanName = salesmanName;
            _saleItems = saleItems;
        }
    }
}
