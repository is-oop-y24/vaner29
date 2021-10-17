using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Tools;

namespace Shops.Entities
{
    public class Shop
    {
        public Shop(Guid id, string name, string address)
        {
            ShopId = id;
            ShopName = name;
            ShopAddress = address;
        }

        public List<ShopProduct> ShopProducts { get; set; } = new ();
        public string ShopName { get; }
        public Guid ShopId { get; }
        public string ShopAddress { get; }
        public void AddProduct(ShopProduct shopProd)
        {
            if (ShopProducts.Contains(shopProd))
            {
                uint prevAmount = FindShopProductInList(shopProd.ShopProd).ShopProductAmount;
                ShopProducts.Remove(shopProd);
                ShopProducts.Add(shopProd.IncreaseProductAmount(prevAmount));
            }
            else
            {
                ShopProducts.Add(shopProd);
            }
        }

        public decimal TotalPriceForProductBatch(CustomerProduct customerProduct)
        {
            if (ShopProducts.Any(prod => prod.ShopProd.Equals(customerProduct.CustomerProd)))
            {
                @ShopProduct curProduct = FindShopProductInList(customerProduct.CustomerProd);
                if (curProduct.ShopProductAmount < customerProduct.CustomerProductAmount)
                {
                    return decimal.MaxValue;
                }

                return customerProduct.CustomerProductAmount * curProduct.ShopProductPrice;
            }

            return decimal.MaxValue;
        }

        public void ChangePriceForProduct(Product product, decimal price)
        {
            ShopProduct newProd = FindShopProductInList(product).ChangeProductPrice(price);
            ShopProducts.Remove(FindShopProductInList(product));
            ShopProducts.Add(newProd);
        }

        public void BuySingleProduct(CustomerProduct product, ref Customer customer)
        {
            decimal productPrice = TotalPriceForProductBatch(product);
            if (productPrice == decimal.MaxValue)
                throw new ShopException("Not Enough Product In Shop");
            if (productPrice > customer.Cash)
                throw new ShopException("Customer Does Not Have Enough Money");
            customer = customer.SpendMoney(productPrice);
            RemoveSingleProductFromShop(product);
        }

        public void BuyProductList(List<CustomerProduct> shoppingList, ref Customer customer)
        {
            decimal totalPrice = 0;
            foreach (CustomerProduct product in shoppingList)
            {
                decimal curPrice = TotalPriceForProductBatch(product);
                if (curPrice == uint.MaxValue)
                    throw new ShopException("Not Enough Product In Shop");
                totalPrice += curPrice;
            }

            if (totalPrice > customer.Cash)
                throw new ShopException("Customer Does Not Have Enough Money");
            customer = customer.SpendMoney(totalPrice);
            RemoveProductListFromShop(shoppingList);
        }

        public @ShopProduct FindShopProductInList(Product product)
        {
            @ShopProduct curProduct = @ShopProducts.FirstOrDefault(newProduct => newProduct.ShopProd.Equals(product));
            if (curProduct == null)
                throw new ShopException("No Such Product In Shop");
            return curProduct;
        }

        private void RemoveSingleProductFromShop(CustomerProduct customerProduct)
        {
            @ShopProduct product = FindShopProductInList(customerProduct.CustomerProd);
            if (product.ShopProductAmount == customerProduct.CustomerProductAmount)
            {
                ShopProducts.Remove(product);
            }
            else if (product.ShopProductAmount > customerProduct.CustomerProductAmount)
            {
                uint boughtAmount = customerProduct.CustomerProductAmount;
                ShopProduct boughtProduct = FindShopProductInList(customerProduct.CustomerProd)
                    .DecreaseProductAmount(boughtAmount);
                ShopProducts.Remove(FindShopProductInList(customerProduct.CustomerProd));
                ShopProducts.Add(boughtProduct);
            }
        }

        private void RemoveProductListFromShop(List<CustomerProduct> shoppingList)
        {
            foreach (CustomerProduct customerProduct in shoppingList)
            {
                RemoveSingleProductFromShop(customerProduct);
            }
        }
    }
}