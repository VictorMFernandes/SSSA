namespace SSSA.Etl.Domain.Extract.EntityIdentifierStrategies
{
    public class PropertyIndexEntityIdentifierStrategy : IEntityIdentifierStrategy
    {
        public const string Salesman = "001";
        public const string Client = "002";
        public const string Sale = "003";

        private readonly int _propertyIndex;

        public PropertyIndexEntityIdentifierStrategy(int propertyIndex)
        {
            _propertyIndex = propertyIndex;
        }

        public bool IsSalesman(string[] properties) => properties[_propertyIndex] == Salesman;
        public bool IsClient(string[] properties) => properties[_propertyIndex] == Client;
        public bool IsSale(string[] properties) => properties[_propertyIndex] == Sale;
    }
}
