using Shops.Tools;

namespace Shops.Entities
{
    public class ShopProduct
    {
        private decimal _shopProductPrice;
        public ShopProduct(Product product, decimal price, uint amount)
        {
            ShopProd = new Product(product);
            ShopProductAmount = amount;
            ShopProductPrice = price;
        }

        // public Guid Id { get; }
        public Product ShopProd { get; }

        public decimal ShopProductPrice
        {
            get => _shopProductPrice;
            private set
            {
                if (value < 0)
                    throw new ShopException("Product price less than zero");
                _shopProductPrice = value;
            }
        }

        public uint ShopProductAmount { get; private set; }

        public void ChangeProductPrice(decimal newPrice)
        {
            ShopProductPrice = newPrice;
        }

        public void DecreaseProductAmount(uint boughtAmount)
        {
            ShopProductAmount -= boughtAmount;
        }

        public void IncreaseProductAmount(uint gotAmount)
        {
            ShopProductAmount += gotAmount;
        }
    }
}