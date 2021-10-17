using System.Collections.Generic;

namespace Shops.Entities
{
    public class Customer
    {
        public Customer(string name, decimal cash)
        {
            Name = name;
            Cash = cash;
        }

        public string Name { get; }
        public decimal Cash { get; }

        public Customer SpendMoney(decimal price)
        {
            return new Customer(Name, Cash - price);
        }
    }
}