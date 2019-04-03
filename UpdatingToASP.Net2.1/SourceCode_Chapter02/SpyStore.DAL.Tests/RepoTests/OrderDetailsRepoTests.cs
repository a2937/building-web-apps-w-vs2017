using SpyStore.DAL.Initializers;
using SpyStore.DAL.Repos;
using SpyStore.DAL.Tests.Base;
using SpyStore.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SpyStore.DAL.Tests.Repos
{
    [Collection("SpyStore.DAL")]
    public class OrderDetailRepoTests : TestBase
    {
        private readonly OrderDetailRepo _repo;

        private bool disposedValue = false;

        public OrderDetailRepoTests()
        {
            _repo = new OrderDetailRepo();
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
        public void ShouldGetAllOrderDetails()
        {
            List<OrderDetail> orders = _repo.GetAll().ToList();
            Assert.Equal(_repo.Count, orders.Count);
        }

        [Fact]
        public void ShouldGetLineItemTotal()
        {
            List<OrderDetail> orderDetails = _repo.GetAll().ToList();
            OrderDetail orderDetail = orderDetails[0];
            Assert.Equal(1799.9700M, orderDetail.LineItemTotal);
        }

        protected override void CleanDatabase()
        {
        }
    }
}