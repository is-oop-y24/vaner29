namespace Shops.Entities
{
    public class CustomerProduct
    {
        public CustomerProduct(Product product, uint amount)
        {
            CustomerProd = new Product(product);
            CustomerProductAmount = amount;
        }

        public Product CustomerProd { get; }
        public uint CustomerProductAmount { get; }
    }
}
