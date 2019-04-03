using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.Exceptions;
using SpyStore.DAL.Initializers;
using SpyStore.DAL.Repos;
using SpyStore.DAL.Tests.Base;
using SpyStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SpyStore.DAL.Tests.Repos
{
    [Collection("SpyStore.DAL")]
    public class ShoppingCartRepoTests : TestBase
    {
        private bool disposedValue = false;

        private readonly ShoppingCartRepo _repo;

        public ShoppingCartRepoTests()
        {
            _repo = new ShoppingCartRepo(new ProductRepo());
            StoreDataInitializer.ClearData(_repo.Context);
            StoreDataInitializer.InitializeData(_repo.Context);
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    StoreDataInitializer.ClearData(_repo.Context);
                    _repo.Dispose();
                }

                disposedValue = true;
                base.Dispose(disposing);
            }
        }

        [Fact]
        public void ShouldAddAnItemToTheCart()
        {
            ShoppingCartRecord item = new ShoppingCartRecord
            {
                ProductId = 2,
                Quantity = 3,
                DateCreated = DateTime.Now,
                CustomerId = 0
            };
            _repo.Add(item);
            List<ShoppingCartRecord> shoppingCartRecords = _repo.GetAll().ToList();
            Assert.Equal(2, shoppingCartRecords.Count);
            Assert.Equal(2, shoppingCartRecords[0].ProductId);
            Assert.Equal(3, shoppingCartRecords[0].Quantity);
        }

        [Fact]
        public void ShouldUpdateQuantityOnAddIfAlreadyInCart()
        {
            ShoppingCartRecord item = new ShoppingCartRecord
            {
                ProductId = 32,
                Quantity = 1,
                DateCreated = DateTime.Now,
                CustomerId = 0
            };
            _repo.Add(item);
            List<ShoppingCartRecord> shoppingCartRecords = _repo.GetAll().ToList();
            Assert.Single(shoppingCartRecords);
            Assert.Equal(2, shoppingCartRecords[0].Quantity);
        }

        [Fact]
        public void ShouldDeleteOnAddIfQuantityEqualZero()
        {
            ShoppingCartRecord item = new ShoppingCartRecord
            {
                ProductId = 30,
                Quantity = -1,
                DateCreated = DateTime.Now,
                CustomerId = 0
            };
            _repo.Add(item);
            List<ShoppingCartRecord> shoppingCartRecords = _repo.GetAll().ToList();
            Assert.Equal(0, shoppingCartRecords.Count(x => x.ProductId == 34));
        }

        [Fact]
        public void ShouldDeleteOnAddIfQuantityLessThanZero()
        {
            ShoppingCartRecord item = new ShoppingCartRecord
            {
                ProductId = 32,
                Quantity = -10,
                DateCreated = DateTime.Now,
                CustomerId = 0
            };
            _repo.Add(item);
            Assert.Equal(0, _repo.Count);
        }

        [Fact]
        public void ShouldUpdateQuantity()
        {
            ShoppingCartRecord item = _repo.Find(0, 32);
            item.Quantity = 5;
            item.DateCreated = DateTime.Now;
            _repo.Update(item);
            List<ShoppingCartRecord> shoppingCartRecords = _repo.GetAll().ToList();
            Assert.Single(shoppingCartRecords);
            Assert.Equal(5, shoppingCartRecords[0].Quantity);
        }

        [Fact]
        public void ShouldDeleteOnUpdateIfQuantityEqualsZero()
        {
            ShoppingCartRecord item = _repo.Find(0, 32);
            item.Quantity = 0;
            item.DateCreated = DateTime.Now;
            _repo.Update(item);
            List<ShoppingCartRecord> shoppingCartRecords = _repo.GetAll().ToList();
            Assert.Empty(shoppingCartRecords);
        }

        [Fact]
        public void ShouldDeleteOnUpdateIfQuantityLessThanZero()
        {
            ShoppingCartRecord item = _repo.Find(0, 32);
            item.Quantity = -10;
            item.DateCreated = DateTime.Now;
            _repo.Update(item);
            List<ShoppingCartRecord> shoppingCartRecords = _repo.GetAll().ToList();
            Assert.Empty(shoppingCartRecords);
        }

        [Fact]
        public void ShouldDeleteCartRecord()
        {
            ShoppingCartRecord item = _repo.Find(0, 32);
            _repo.Context.Entry(item).State = EntityState.Detached;
            _repo.Delete(item.Id, item.TimeStamp);
            Assert.Empty(_repo.GetAll());
        }

        [Fact]
        public void ShouldNotDeleteMissingCartRecord()
        {
            ShoppingCartRecord item = _repo.Find(0, 32);
            Assert.Throws<DbUpdateConcurrencyException>(() => _repo.Delete(200, item.TimeStamp));
        }

        [Fact]
        public void ShouldThrowWhenAddingToMuchQuantity()
        {
            _repo.Context.SaveChanges();
            ShoppingCartRecord item = new ShoppingCartRecord
            {
                ProductId = 2,
                Quantity = 500,
                DateCreated = DateTime.Now,
                CustomerId = 2
            };
            InvalidQuantityException ex = Assert.Throws<InvalidQuantityException>(() => _repo.Update(item));
            Assert.Equal("Can't add more product than available in stock", ex.Message);
        }

        [Fact]
        public void ShouldThrowWhenUpdatingTooMuchQuantity()
        {
            ShoppingCartRecord item = _repo.Find(0, 32);
            item.Quantity = 100;
            item.DateCreated = DateTime.Now;
            InvalidQuantityException ex = Assert.Throws<InvalidQuantityException>(() => _repo.Update(item));
            Assert.Equal("Can't add more product than available in stock", ex.Message);
        }

        [Fact]
        public void ShouldProcessAnOrder()
        {
            using (OrderRepo orderRepo = new OrderRepo(new OrderDetailRepo()))
            {
                IEnumerable<Order> orders1 = orderRepo.GetAll();
                Assert.Single(orders1.ToList());
                _repo.Purchase(0);
                using (OrderRepo orderRepo1 = new OrderRepo(new OrderDetailRepo()))
                {
                    IEnumerable<Order> orders2 = orderRepo1.GetAll();
                    Assert.Equal(2, orders2.ToList().Count);
                }
            }
        }

        protected override void CleanDatabase()
        {
        }

        //add tests for quantity check
    }
}