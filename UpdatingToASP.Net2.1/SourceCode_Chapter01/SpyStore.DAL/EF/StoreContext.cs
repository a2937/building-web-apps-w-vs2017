using Microsoft.EntityFrameworkCore;
using SpyStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpyStore.DAL.EF
{
    public class StoreContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }


        public StoreContext()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreContext"/> class.
        ///  The options that can be passed in include settings for the database provider, connection resiliency, and any other options specific to your application.
        ///  The options are typically passed in through a dependency injection (DI) framework,
        ///  such as is used in ASP.NET Core.
        ///  This allows automated tests to inject a different instance for testing purposes (such as the InMemory provider)
        ///  as well as configure EF Core to use development or production servers
        ///  without changing any of the data access code.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public StoreContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer( @"Server=(localdb)\mssqllocaldb;Database=SpyStore;Trusted_Connection=True;MultipleActiveResultSets=true;", options => options.ExecutionStrategy(c=> new MyExecutionStrategy(c)));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
