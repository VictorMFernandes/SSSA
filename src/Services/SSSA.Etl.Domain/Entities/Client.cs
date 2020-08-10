namespace SSSA.Etl.Domain.Entities
{
    public class Client
    {
        public string Cnpj { get; }
        public string Name { get; }
        public string BusinessArea { get; }

        public Client()
        {
        }

        public Client(string cnpj, string name, string businessArea)
        {
            Cnpj = cnpj;
            Name = name;
            BusinessArea = businessArea;
        }
    }
}
