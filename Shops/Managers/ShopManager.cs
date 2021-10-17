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

        public List<ShopProduct> ListAllProductsInShop(Shop shop)
        {
            return shop.ShopProducts;
        }
    }
}