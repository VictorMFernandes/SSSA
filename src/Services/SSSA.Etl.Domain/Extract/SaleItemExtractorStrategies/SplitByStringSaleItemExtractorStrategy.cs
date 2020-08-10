using SSSA.Etl.Domain.Entities;
using SSSA.Helper.Extensions;
using System.Collections.Generic;
using System.Globalization;

namespace SSSA.Etl.Domain.Extract.SaleItemExtractorStrategies
{
    public class SplitByStringSaleItemExtractorStrategy : ISaleItemExtractorStrategy
    {
        private readonly string _itemSeparator;
        private readonly string _itemPropertiesSeparator;
        private readonly string[] _stringsToRemove;

        public SplitByStringSaleItemExtractorStrategy(
            string itemSeparator,
            string itemPropertiesSeparator,
            params string[] stringsToRemove)
        {
            _itemSeparator = itemSeparator;
            _itemPropertiesSeparator = itemPropertiesSeparator;
            _stringsToRemove = stringsToRemove;
        }

        public IEnumerable<SaleItem> ExtractSaleItems(string saleItemsText, CultureInfo cultureInfo)
        {
            var itemsText = saleItemsText.RemoveTexts(_stringsToRemove).Split(_itemSeparator);

            foreach (var itemText in itemsText)
            {
                var itemParams = itemText.Split(_itemPropertiesSeparator);
                var itemId = itemParams[0];
                if (!int.TryParse(itemParams[1], out var itemQuantity))
                {
                    continue;
                }

                if (!decimal.TryParse(itemParams[2], NumberStyles.Currency, cultureInfo, out var itemPrice))
                {
                    continue;
                }

                yield return new SaleItem(new Item(itemId, itemPrice), itemQuantity);
            }
        }
    }
}
