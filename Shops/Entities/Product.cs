using System;

namespace Shops.Entities
{
    public class Product : IEquatable<Product>
    {
        public Product(Guid id, string name)
        {
            Id = id;
            ProductName = name;
        }

        public Product(Product p)
            : this(p.Id, p.ProductName) { }

        public string ProductName { get; }
        public Guid Id { get; }

        public bool Equals(Product other)
        {
            return Id == other?.Id;
        }

        /*public override bool Equals(object obj)
        {
            return obj is Product product && Equals(product);
        }

        public override int GetHashCode()
        {
            return ProductName != null ? ProductName.GetHashCode() : 0;
        }*/
    }
}