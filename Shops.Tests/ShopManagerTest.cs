using System;
using System.Collections.Generic;
using Shops.Entities;
using Shops.Managers;
using Shops.Tools;
using NUnit.Framework;

namespace Shops.Tests
{
    public class Tests
    {
        private IShopManager _shopManager;

        [SetUp]
        public void Setup()
        {
            _shopManager = new ShopManager();
        }

        [Test]
        public void ChangePriceForFakeProduct_ThrowException()
        {
            Assert.Catch<ShopException>(() =>
            {
                var spar = new Shop( Guid.NewGuid(), "Spar","Yesenina Street Caruselina House");
                var bread = new Product(Guid.NewGuid(),"Sweet Bread");
                _shopManager.ChangePriceForProductInShop(spar, bread, 10);
            });
        }
        [Test]
        public void BuyFakeSingleProduct_ThrowException()
        {
            Assert.Catch<ShopException>(() =>
            {
                var spar = new Shop(Guid.NewGuid(),"Spar", "Yesenina Street Caruselina House");
                var bread = new Product(Guid.NewGuid(),"Sweet Bread");
                var ant = new Customer("Anthon", 100, new List<CustomerProduct> { new CustomerProduct(bread, 1) });
                _shopManager.BuySingleProductInShop(spar, ant.ShoppingList[0], ant);
            });
        }
        
        [Test]
        public void BuyFakeProductList_ThrowException()
        {
            Assert.Catch<ShopException>(() =>
            {
                var spar = new Shop(Guid.NewGuid(),"Spar", "Yesenina Street Caruselina House");
                var bread = new Product(Guid.NewGuid(),"Sweet Bread");
                var ant = new Customer("Anthon", 100, new List<CustomerProduct> { new CustomerProduct(bread, 1) });
                _shopManager.BuyProductListInShop(spar, ant.ShoppingList, ant);
            });
        }
        
        [Test]
        public void BuyTooExpensiveProduct_ThrowException()
        {
            Assert.Catch<ShopException>(() =>
            {
                var spar = new Shop(Guid.NewGuid(),"Spar", "Yesenina Street Caruselina House");
                var bread = new Product(Guid.NewGuid(),"Sweet Bread");
                var shopBread = new ShopProduct(bread, 1200, 100);
                spar.AddProduct(shopBread);
                var shoppingList = new List<CustomerProduct> {new CustomerProduct( bread ,10)};
                var ant = new Customer("Anthon", 1000, shoppingList);
                spar.BuySingleProduct(shoppingList[0] , ant);
            });
            
        }

        [Test]
        public void BuyMoreProductThanExists_ThrowException()
        {
            Assert.Catch<ShopException>(() =>
            {
                var spar = new Shop(Guid.NewGuid(),"Spar", "Yesenina Street Caruselina House");
                var bread = new Product(Guid.NewGuid(),"Sweet Bread");
                var shopBread = new ShopProduct(bread, 10, 1);
                spar.AddProduct(shopBread);
                var shoppingList = new List<CustomerProduct> { new CustomerProduct(bread, 10) };
                var ant = new Customer("Anthon", 1000, shoppingList);
                spar.BuySingleProduct(shoppingList[0], ant);
            });
        }
        [Test]
        public void Create2ShopsWithSameNameAndAddress_AddProducts()
        {
            var spar1 = new Shop(Guid.NewGuid(),"Spar", "Yesenina Street Caruselina House");
                var spar2 = new Shop(Guid.NewGuid(),"Spar", "Yesenina Street Caruselina House");
                var bread = new Product(Guid.NewGuid(),"Sweet Bread");
                var shopBread = new ShopProduct(bread, 10, 1);
                var milk = new Product(Guid.NewGuid(),"Sour Milk");
                var shopMilk = new ShopProduct(milk, 100, 1);
                spar1.AddProduct(shopBread);
                spar2.AddProduct(shopMilk);
                Assert.Contains(shopBread, spar1.ShopProducts );
                Assert.Contains(shopMilk, spar2.ShopProducts);
        }

        [Test]
        public void SearchThroughShopsWithoutAllProducts_ThrowException()
        {
            Assert.Catch<ShopException>(() =>
            {
                var spar = new Shop(Guid.NewGuid(),"Spar", "Yesenina Street Caruselina House");
                var fiveka = new Shop(Guid.NewGuid(),"5ka", "Pushkina Street kolotushkina House");
                var bread = new Product(Guid.NewGuid(),"Sweet Bread");
                var milk = new Product(Guid.NewGuid(),"Sour Milk");
                spar.AddProduct(new ShopProduct(bread, 10, 1));
                spar.AddProduct(new ShopProduct(milk, 15, 1));
                fiveka.AddProduct(new ShopProduct(bread, 1, 10));
                fiveka.AddProduct(new ShopProduct(milk, 5, 10));
                _shopManager.RegisterShop(spar);
                _shopManager.RegisterShop(fiveka);
                var shoppingList = new List<CustomerProduct>
                    { new CustomerProduct(bread, 15), new CustomerProduct(milk, 15) };
                _shopManager.FindShopWithBestPriceForProductList(shoppingList);
            });
            
        }

        [Test]
        public void SearchThroughShopsWithoutOneProduct_ThrowException()
        {
            Assert.Catch<ShopException>(() =>
            {
                var spar = new Shop(Guid.NewGuid(),"Spar", "Yesenina Street Caruselina House");
                var fiveka = new Shop(Guid.NewGuid(),"5ka", "Pushkina Street kolotushkina House");
                var bread = new Product(Guid.NewGuid(),"Sweet Bread");
                var milk = new Product(Guid.NewGuid(),"Sour Milk");
                spar.AddProduct(new ShopProduct(bread, 10, 20));
                spar.AddProduct(new ShopProduct(milk, 15, 1));
                fiveka.AddProduct(new ShopProduct(bread, 1, 10));
                fiveka.AddProduct(new ShopProduct(milk, 5, 20));
                _shopManager.RegisterShop(spar);
                _shopManager.RegisterShop(fiveka);
                var shoppingList = new List<CustomerProduct>
                    { new CustomerProduct(bread, 15), new CustomerProduct(milk, 15) };
                _shopManager.FindShopWithBestPriceForProductList(shoppingList);
            });
        }
        [Test]
        public void SearchThroughShopsWithtAllProducts_GetShop()
        {
            var spar = new Shop(Guid.NewGuid(),"Spar", "Yesenina Street Caruselina House");
            var fiveka = new Shop(Guid.NewGuid(),"5ka", "Pushkina Street kolotushkina House");
            var bread = new Product(Guid.NewGuid(),"Sweet Bread");
            var milk = new Product(Guid.NewGuid(),"Sour Milk");
            spar.AddProduct(new ShopProduct(bread, 10, 200));
            spar.AddProduct(new ShopProduct(milk, 15, 200));
            fiveka.AddProduct(new ShopProduct(bread, 1, 200));
            fiveka.AddProduct(new ShopProduct(milk, 5, 200));
            _shopManager.RegisterShop(spar);
            _shopManager.RegisterShop(fiveka);
            var shoppingList = new List<CustomerProduct>
                { new CustomerProduct(bread, 10), new CustomerProduct(milk, 10) };
            Shop bestShop = _shopManager.FindShopWithBestPriceForProductList(shoppingList);
            Assert.AreEqual(bestShop, fiveka);
        }
        [Test]
        public void ChangeShopProductsPrice()
        {
            var spar = new Shop(Guid.NewGuid(),"Spar", "Yesenina Street Caruselina House");
            var bread = new Product(Guid.NewGuid(),"Cheap Bread");
            var cheapBread = new ShopProduct(bread, 10, 1);
            spar.AddProduct(cheapBread);
            spar.FindShopProductInList(bread).ChangeProductPrice(100);
            Assert.AreEqual(100, spar.FindShopProductInList(bread).ShopProductPrice);
        }

    }
}
    