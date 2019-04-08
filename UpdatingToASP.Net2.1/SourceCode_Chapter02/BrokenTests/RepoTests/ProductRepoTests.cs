using SpyStore.DAL.Initializers;
using SpyStore.DAL.Repos;
using SpyStore.DAL.Tests.Base;
using System.Linq;
using Xunit;

namespace SpyStore.DAL.Tests.Repos
{
    [Collection("SpyStore.DAL")]
    public class ProductRepoTests : TestBase
    {
        private bool disposedValue = false;

        private readonly ProductRepo _repo;

        public ProductRepoTests()
        {
            _repo = new ProductRepo();
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

        [Theory]
        [InlineData(0, 5)]
        [InlineData(1, 5)]
        [InlineData(2, 6)]
        [InlineData(3, 6)]
        [InlineData(4, 3)]
        [InlineData(5, 7)]
        [InlineData(6, 9)]
        public void ShouldGetAllProductsForACategory(int catId, int productCount)
        {
            System.Collections.Generic.List<Models.ViewModels.Base.ProductAndCategoryBase> prods = _repo.GetProductsForCategory(catId).ToList();
            Assert.Equal(productCount, prods.Count);
        }

        protected override void CleanDatabase()
        {
        }
    }
}