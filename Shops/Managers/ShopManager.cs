using System;
using System.Collections.Generic;
using Shops.Entities;
using Shops.Tools;

namespace Shops.Managers // id для продуктов
{
    public class ShopManager : IShopManager
    {
        private List<Shop> _shops = new ();
        private List<Product> _products = new ();

        public void RegisterProduct(Product product)
        {
            _products.Add(product);
        }

        public void RegisterShop(Shop shop)
        {
            _shops.Add(shop);
        }

        public void RegisterShopList(List<Shop> shopList)
        {
            _shops.AddRange(shopList);
        }

        public void AddShipment(List<ShopProduct> shipment, Shop shop)
        {
            foreach (ShopProduct curShopProduct in shipment)
            {
                shop.AddProduct(curShopProduct);
            }
        }

        public void AddProduct(ShopProduct product, Shop shop)
        {
            shop.AddProduct(product);
        }

        public Shop FindShopWithBestPriceForProductList(List<CustomerProduct> shoppingList)
        {
            decimal minimumPrice = decimal.MaxValue;
            var tempId = Guid.NewGuid();
            var curBestShop = new Shop(tempId, "SirThisIsWendy's", "Grove street or smth idfc");
            foreach (Shop curShop in _shops)
            {
                decimal curPrice = 0;
                foreach (CustomerProduct curProduct in shoppingList)
                {
                    decimal curProductPrice = curShop.TotalPriceForProductBatch(curProduct);
                    if (curProductPrice == decimal.MaxValue)
                    {
                        curPrice = decimal.MaxValue;
                        break;
                    }

                    curPrice += curProductPrice;
                }

                if (curPrice < minimumPrice)
                {
                    minimumPrice = curPrice;
                    curBestShop = curShop;
                }
            }

            if (minimumPrice == uint.MaxValue || curBestShop.ShopId == tempId)
                throw new ShopException("No Shop With All Of The Products");
            return curBestShop;
        }

        public void ChangePriceForProductInShop(Shop shop, Product product, uint newPrice)
        {
            shop.ChangePriceForProduct(product, newPrice);
        }

        public void BuySingleProductInShop(Shop shop, CustomerProduct product, Customer customer)
        {
            shop.BuySingleProduct(product, customer);
        }

        public void BuyProductListInShop(Shop shop, List<CustomerProduct> shoppingList, Customer customer)
        {
            shop.BuyProductList(shoppingList, customer);
        }

        public List<ShopProduct> ListAllProductsInShop(Shop shop)
        {
            return shop.ShopProducts;
        }

        public void ChangePriceForProduct(Shop shop, Product product, decimal price)
        {
            shop.FindShopProductInList(product).ChangeProductPrice(price);
        }
    }
}