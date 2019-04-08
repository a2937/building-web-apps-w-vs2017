using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.Repos;
using SpyStore.DAL.Tests.Base;
using SpyStore.Models.Entities;
using System.Collections.Generic;
using Xunit;

namespace SpyStore.DAL.Tests.RepoTests
{
    [Collection("SpyStore.DAL")]
    public class CategoryRepoUpdateTests : TestBase
    {
        private readonly CategoryRepo _repo;

        private bool disposedValue = false;

        public CategoryRepoUpdateTests()
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

        [Fact]
        public void ShouldUpdateACategoryEntity()
        {
            Category category = new Category { CategoryName = "Foo" };
            _repo.AddRange(new List<Category>
            {
                category,
            });
            category.CategoryName = "Bar";
            _repo.Update(category, false);
            int count = _repo.SaveChanges();
            Assert.Equal(1, count);
            using (CategoryRepo repo = new CategoryRepo())
            {
                Category cat = repo.GetFirst();
                Assert.Equal(cat.CategoryName, category.CategoryName);
            }
        }

        [Fact]
        public void ShouldUpdateARangeOfCategoryEntities()
        {
            List<Category> categories = new List<Category>
            {
                new Category { CategoryName = "Foo" },
                new Category { CategoryName = "Bar" },
                new Category { CategoryName = "FooBar" }
            };
            _repo.AddRange(categories);
            categories[0].CategoryName = "Foo1";
            categories[1].CategoryName = "Foo2";
            categories[2].CategoryName = "Foo3";
            _repo.UpdateRange(categories, false);
            int count = _repo.SaveChanges();
            Assert.Equal(3, count);
            using (CategoryRepo repo = new CategoryRepo())
            {
                Category cat = repo.GetFirst();
                Assert.Equal("Foo1", cat.CategoryName);
            }
        }
    }
}