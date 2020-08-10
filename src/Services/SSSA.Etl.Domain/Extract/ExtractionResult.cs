using SSSA.Etl.Domain.Entities;
using System.Collections.Generic;

namespace SSSA.Etl.Domain.Extraction
{
    public class ExtractionResult
    {
        public IEnumerable<Salesman> Salesmen { get; }
        public IEnumerable<Client> Clients { get; }
        public IEnumerable<Sale> Sales { get; }
        public string ErrorMessage { get; private set; }
        public bool Succeeded => string.IsNullOrWhiteSpace(ErrorMessage);

        public ExtractionResult(
            IEnumerable<Salesman> salesmen,
            IEnumerable<Client> client,
            IEnumerable<Sale> sales)
        {
            Salesmen = salesmen;
            Clients = client;
            Sales = sales;
        }

        public ExtractionResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
