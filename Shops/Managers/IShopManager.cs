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
        List<ShopProduct> ListAllProductsInShop(Shop shop);
    }
}