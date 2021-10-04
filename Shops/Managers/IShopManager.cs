using System.Collections.Generic;

using Shops.Entities;
namespace Shops.Managers
{
    public interface IShopManager
    {
        void RegisterShop(Shop shop);
        void RegisterShopList(List<Shop> shopList);
        void AddShipment(List<ShopProduct> shipment, Shop shop);
        Shop FindShopWithBestPriceForProductList(List<CustomerProduct> shoppingList);
        void ChangePriceForProductInShop(Shop shop, Product product, uint newPrice);
        void BuySingleProductInShop(Shop shop, CustomerProduct product, Customer customer);
        void BuyProductListInShop(Shop shop, List<CustomerProduct> shoppingList, Customer customer);
        List<ShopProduct> ListAllProductsInShop(Shop shop);
        void ChangePriceForProduct(Shop shop, Product product, decimal price);
    }
}