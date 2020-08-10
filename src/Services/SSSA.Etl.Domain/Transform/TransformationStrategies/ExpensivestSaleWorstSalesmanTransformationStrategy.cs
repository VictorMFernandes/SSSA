using SSSA.Etl.Domain.Extraction;
using SSSA.Helper.Extensions;
using System.Linq;

namespace SSSA.Etl.Domain.Transform.TransformationStrategies
{
    public class ExpensivestSaleWorstSalesmanTransformationStrategy : ITransformationStrategy
    {
        public TransformationResult Transform(ExtractionResult extractionResult)
        {
            if (extractionResult.Salesmen?.Any() != true || extractionResult.Clients?.Any() != true || extractionResult.Sales?.Any() != true)
            {
                return new TransformationResult(
                    $"To carry out the {nameof(ExpensivestSaleWorstSalesmanTransformationStrategy)} transformation, it is necessary to have at least one salesman, one customer and one sale");
            }

            var clientQty = extractionResult.Clients.Count();
            var salesmenQty = extractionResult.Salesmen.Count();
            var expensivestSale = extractionResult.Sales.Higher(x => x.Total).Id;
            var worstSalesman = extractionResult.Sales.GroupBy(x => x.SalesmanName).Lower(x => x.Sum(y => y.Total)).Key;
            return new TransformationResult(new object[] { clientQty, salesmenQty, expensivestSale, worstSalesman });
        }
    }
}
