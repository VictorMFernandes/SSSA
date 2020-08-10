using SSSA.Etl.Domain.Entities;
using System.Collections.Generic;
using System.Globalization;

namespace SSSA.Etl.Domain.Extract.SaleItemExtractorStrategies
{
    public interface ISaleItemExtractorStrategy
    {
        IEnumerable<SaleItem> ExtractSaleItems(string saleItemsText, CultureInfo cultureInfo);
    }
}
