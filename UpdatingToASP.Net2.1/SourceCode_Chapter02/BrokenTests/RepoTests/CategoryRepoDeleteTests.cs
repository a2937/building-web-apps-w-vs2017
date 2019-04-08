using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.Repos;
using SpyStore.DAL.Tests.Base;
using SpyStore.Models.Entities;
using System.Collections.Generic;
using Xunit;

namespace SpyStore.DAL.Tests.RepoTests
{
    [Collection("SpyStore.DAL")]
    public class CategoryRepoDeleteTests : TestBase
    {
        private readonly CategoryRepo _repo;

        private bool disposedValue = false;

        public CategoryRepoDeleteTests()
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
        public void ShouldDeleteACategoryEntityFromDbSet()
        {
            _repo.AddRange(new List<Category>
            {
                new Category { CategoryName = "Foo" },
            });
            Assert.Equal(1, _repo.Count);
            Category category = _repo.GetFirst();
            int count = _repo.Delete(category);
            Assert.Equal(1, count);
            Assert.Equal(0, _repo.Count);
        }

        [Fact]
        public void ShouldDeleteACategoryRangeFromDbSet()
        {
            List<Category> categories = new List<Category>
            {
                new Category { CategoryName = "Foo" },
                new Category { CategoryName = "Bar" },
                new Category { CategoryName = "FooBar" }
            };
            _repo.AddRange(categories);
            Assert.Equal(3, _repo.Count);
            int count = _repo.DeleteRange(categories);
            Assert.Equal(3, count);
            Assert.Equal(0, _repo.Count);
        }

        [Fact]
        public void ShouldDeleteACategoryRangeAndPersistManuallyFromDbSet()
        {
            List<Category> categories = new List<Category>
            {
                new Category { CategoryName = "Foo" },
                new Category { CategoryName = "Bar" },
                new Category { CategoryName = "FooBar" }
            };
            _repo.AddRange(categories);
            Assert.Equal(3, _repo.Count);
            int count = _repo.DeleteRange(categories, false);
            Assert.Equal(0, count);
            count = _repo.SaveChanges();
            Assert.Equal(3, count);
            Assert.Equal(0, _repo.Count);
        }

        [Fact]
        public void ShouldDeleteACategoryEntityFromContext()
        {
            _repo.AddRange(new List<Category>
            {
                new Category { CategoryName = "Foo" },
            });
            Assert.Equal(1, _repo.Count);
            Category category = _repo.GetFirst();
            _repo.Context.Remove(category);
            int count = _repo.SaveChanges();
            Assert.Equal(1, count);
            Assert.Equal(0, _repo.Count);
        }

        [Fact]
        public void ShouldDeleteACategoryEntityAndNotPersist()
        {
            _repo.AddRange(new List<Category>
            {
                new Category { CategoryName = "Foo" },
            });
            Assert.Equal(1, _repo.Count);
            Category category = _repo.GetFirst();
            int count = _repo.Delete(category, false);
            Assert.Equal(0, count);
            Assert.Equal(1, _repo.Count);
        }

        [Fact]
        public void ShouldDeleteACategoryFromDifferentContext()
        {
            _repo.AddRange(new List<Category>
            {
                new Category { CategoryName = "Foo" },
            });
            Assert.Equal(1, _repo.Count);
            Category category = _repo.GetFirst();
            using (CategoryRepo repo = new CategoryRepo())
            {
                int count = repo.Delete(category.Id, category.TimeStamp, false);
                Assert.Equal(0, count);
                count = repo.Context.SaveChanges();
                Assert.Equal(1, count);
                Assert.Equal(0, repo.Count);
            }
        }

        [Fact]
        public void ShouldDeleteACategoryFromSameContext()
        {
            Category category = new Category { CategoryName = "Foo" };
            _repo.Add(category);
            Assert.Equal(1, _repo.Count);
            int count = _repo.Delete(category.Id, category.TimeStamp, false);
            Assert.Equal(0, count);
            count = _repo.SaveChanges();
            Assert.Equal(1, count);
            Assert.Equal(0, _repo.Count);
        }
    }
}