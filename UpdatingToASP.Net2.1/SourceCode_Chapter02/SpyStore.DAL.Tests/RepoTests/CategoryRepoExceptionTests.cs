using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SpyStore.DAL.EF;
using SpyStore.DAL.Repos;
using SpyStore.DAL.Tests.Base;
using SpyStore.Models.Entities;
using System;
using Xunit;

namespace SpyStore.DAL.Tests.RepoTests
{
    [Collection("SpyStore.DAL")]
    public class CategoryRepoExceptionTests : TestBase
    {
        private readonly CategoryRepo _repo;

        private bool disposedValue = false;

        public CategoryRepoExceptionTests()
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
        public void ShouldNotDeleteACategoryFromSameContextWithConcurrencyIssue()
        {
            Category category = new Category { CategoryName = "Foo" };
            _repo.Add(category);
            Assert.Equal(1, _repo.Count);
            Assert.Throws<Exception>(() => _repo.Delete(category.Id, null, false));
        }

        [Fact]
        public void ShouldNotDeleteOnConcurrencyIssue()
        {
            Category category = new Category { CategoryName = "Foo" };
            _repo.Add(category);
            _repo.Context.Database.ExecuteSqlCommand("Update Store.Categories set CategoryName = 'Bar'");
            Assert.Throws<DbUpdateConcurrencyException>(() => _repo.Delete(category.Id, category.TimeStamp));
        }

        [Fact]
        public void ShouldThrowRetryExeptionWhenCantConnect()
        {
            DbContextOptionsBuilder<StoreContext> contextOptionsBuilder = new DbContextOptionsBuilder<StoreContext>();
            const string connectionString =
                @"Server=(localdb)\mssqllocaldb;Database=SpyStore;user id=foo;password=bar;MultipleActiveResultSets=true;";
            //contextOptionsBuilder.UseSqlServer(connectionString,
            //    o => o.EnableRetryOnFailure(2,new TimeSpan(0,0,0,0,100),new Collection<int>{ -2146232060 }));
            contextOptionsBuilder.UseSqlServer(connectionString,
                o => o.ExecutionStrategy(c => new MyExecutionStrategy(c, 5, new TimeSpan(0, 0, 0, 0, 30))));
            using (CategoryRepo repo = new CategoryRepo(contextOptionsBuilder.Options))
            {
                Category category = new Category { CategoryName = "Foo" };
                Assert.Throws<RetryLimitExceededException>(() => repo.Add(category));
            }
        }
    }
}