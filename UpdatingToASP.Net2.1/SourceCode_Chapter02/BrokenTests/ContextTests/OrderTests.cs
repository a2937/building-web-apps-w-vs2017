using SpyStore.DAL.EF;
using SpyStore.DAL.Initializers;
using SpyStore.DAL.Tests.Base;
using SpyStore.Models.Entities;
using System;
using System.Linq;
using Xunit;

namespace SpyStore.DAL.Tests.Context
{
    [Collection("SpyStore.DAL")]
    public class OrderTests : TestBase
    {
        public OrderTests() : base()
        {
            using (StoreContext storeContext = new StoreContext())
                StoreDataInitializer.InitializeData(storeContext);
            }
        }

        [Fact]
        public void ShouldGetOrderTotal()
        {
            Order order = _db.Orders.FirstOrDefault();
            Assert.Equal(4424.90M, order.OrderTotal.Value);
        }

        [Fact]
        public void ShouldUpdateAnOrder()
        {
            Order order = _db.Orders.FirstOrDefault();
            order.ShipDate = DateTime.Now;
            _db.SaveChanges();
            order = _db.Orders.FirstOrDefault();
            Assert.Equal(DateTime.Now.ToString("d"),
                order.ShipDate.ToString("d"));
        }

        [Fact]
        public void ShouldGetOrderTotalAfterAddingAnOrderDetail()
        {
            Order order = _db.Orders.FirstOrDefault();
            OrderDetail orderDetail = new OrderDetail { OrderId = order.Id, ProductId = 2, Quantity = 5, UnitCost = 100M };
            _db.OrderDetails.Add(orderDetail);
            _db.SaveChanges();
            using (StoreContext storeContext = new StoreContext())
            {
                //Need to use a new DbContext to get the updated value
                order = storeContext.Orders.FirstOrDefault();
                //order = _db.Orders.FirstOrDefault();
                Assert.Equal(4924.90M, order.OrderTotal);
            }
        }

        protected override void CleanDatabase()
        {
            using (StoreContext storeContext = new StoreContext())
            {
                StoreDataInitializer.ClearData(storeContext);
            }
        }
    }
}