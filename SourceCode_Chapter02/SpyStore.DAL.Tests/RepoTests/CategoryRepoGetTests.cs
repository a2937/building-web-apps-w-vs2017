using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.Repos;
using SpyStore.DAL.Tests.Base;
using SpyStore.Models.Entities;
using System.Collections.Generic;
using Xunit;

namespace SpyStore.DAL.Tests.RepoTests
{
    [Collection("SpyStore.DAL")]
    public class CategoryRepoGetTests : TestBase
    {
        private readonly CategoryRepo _repo;

        private bool disposedValue = false;

        public CategoryRepoGetTests()
        {
            _repo = new CategoryRepo();
            CleanDatabase();
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _repo.Dispose();
                }

                disposedValue = true;
                base.Dispose(disposing);
            }
        }

        protected override void CleanDatabase()
        {
            _repo.Context.Database.ExecuteSqlCommand("Delete from Store.Categories");
            _repo.Context.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (\"Store.Categories\", RESEED, -1);");
        }

        //private IList<Product> CreateProducts()
        //{
        //    var prods = new List<Product>
        //    {
        //        new Product() {CurrentPrice = 12.99M, ModelName = "Product 1", ModelNumber = "P1"},
        //        new Product() {CurrentPrice = 9.99M, ModelName = "Product 2", ModelNumber = "P2"},
        //    };
        //    return prods;
        //}

        [Fact]
        public void ShouldGetFirstCategory()
        {
            List<Category> categories = new List<Category>()
            {
                new Category { CategoryName = "Foo" },
                new Category { CategoryName = "Bar" },
                new Category { CategoryName = "FooBar" }
            };
            _repo.AddRange(categories);
            Assert.Equal(0, _repo.GetFirst().Id);
        }

        [Fact]
        public void ShouldGetACategoryWithProductInfo()
        {
            Category category = new Category { CategoryName = "Foo" };
            _repo.Add(category, true);
            //var cat = _repo.GetOneWithProducts(2);
            //Assert.Equal(2, cat.Products.Count());
        }

        [Fact]
        public void ShouldGetCategory()
        {
            Category category = new Category { CategoryName = "Foo" };
            _repo.Add(category);
            _repo.Find(category.Id);
        }
    }
}