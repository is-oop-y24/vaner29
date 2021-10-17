using Shops.Tools;

namespace Shops.Entities
{
    public class ShopProduct
    {
        private decimal _shopProductPrice;
        public ShopProduct(Product product, decimal price, uint amount)
        {
            ShopProd = product;
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

        public ShopProduct ChangeProductPrice(decimal newPrice)
        {
            return new ShopProduct(ShopProd, newPrice, ShopProductAmount);
        }

        public ShopProduct DecreaseProductAmount(uint boughtAmount)
        {
            return new ShopProduct(ShopProd, ShopProductPrice, ShopProductAmount - boughtAmount);
        }

        public ShopProduct IncreaseProductAmount(uint gotAmount)
        {
            return new ShopProduct(ShopProd, ShopProductPrice, ShopProductAmount + gotAmount);
        }
    }
}