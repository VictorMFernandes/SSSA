namespace SSSA.Etl.Domain.Entities
{
    public class Salesman
    {
        public string Cpf { get; }
        public string Name { get; }
        public decimal Salary { get; }

        public Salesman()
        {
        }

        public Salesman(string cpf, string name, decimal salary)
        {
            Cpf = cpf;
            Name = name;
            Salary = salary;
        }
    }
}
