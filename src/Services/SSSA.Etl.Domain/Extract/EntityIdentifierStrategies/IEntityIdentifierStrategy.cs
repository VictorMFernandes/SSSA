namespace SSSA.Etl.Domain.Extract.EntityIdentifierStrategies
{
    public interface IEntityIdentifierStrategy
    {
        bool IsSalesman(string[] properties);
        bool IsClient(string[] properties);
        bool IsSale(string[] properties);
    }
}
