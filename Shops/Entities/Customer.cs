using System.Collections.Generic;

namespace Shops.Entities
{
    public class Customer
    {
        public Customer(string name, decimal cash, List<CustomerProduct> shoppinglist)
        {
            Name = name;
            Cash = cash;
            ShoppingList = shoppinglist;
        }

        public string Name { get; }
        public decimal Cash { get; private set; }
        public List<CustomerProduct> ShoppingList { get; }

        public void SpendMoney(decimal price)
        {
            Cash -= price;
        }
    }
}