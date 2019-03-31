using SpyStore.DAL.Initializers;
using SpyStore.DAL.Repos;
using SpyStore.DAL.Tests.Base;
using System.Linq;
using Xunit;

namespace SpyStore.DAL.Tests.Repos
{
    [Collection("SpyStore.DAL")]
    public class OrderRepoTests : TestBase
    {
        private readonly OrderRepo _repo;

        private bool disposedValue = false;

        public OrderRepoTests()
        {
            _repo = new OrderRepo(new OrderDetailRepo());
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
        public void ShouldGetAllOrders()
        {
            System.Collections.Generic.List<Models.Entities.Order> orders = _repo.GetAll().ToList();
            Assert.Equal(1, orders.Count);
        }

        protected override void CleanDatabase()
        {
        }
    }
}