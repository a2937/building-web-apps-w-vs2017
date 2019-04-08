using log4net;
using Microsoft.EntityFrameworkCore;

using SpyStore.Models.Entities;
using System;
using System.Data;
using System.Reflection;

namespace SpyStore.DAL.EF
{
    public class StoreContext : DbContext
    {

        public DbSet<Category> Categories { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ShoppingCartRecord> ShoppingCartRecords { get; set; }

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
            try
            {
                Database.Migrate();
            }
            catch (Exception exception)
            {
                //LogHelper._logger.Error("Database migration failure.",exception);
                //Should do something intelligent here
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=SpyStore;Trusted_Connection=True;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.EmailAddress).HasName("IX_Customers").IsUnique();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
                entity.Property(e => e.ShipDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
                entity.Property(e => e.OrderTotal)
                .HasColumnType("money")
                .HasComputedColumnSql("Store.GetOrderTotal([Id])");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.LineItemTotal)
                .HasColumnType("money")
                .HasComputedColumnSql("[Quantity]*[UnitCost]");
                entity.Property(e => e.UnitCost).HasColumnType("money");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.UnitCost).HasColumnType("money");
                entity.Property(e => e.CurrentPrice).HasColumnType("money");
            });

            modelBuilder.Entity<ShoppingCartRecord>(entity =>
            {
                entity.HasIndex(e => new
                {
                    ShoppingCartRecordId = e.Id,
                    e.ProductId,
                    e.CustomerId
                }).HasName("IX_ShoppingCart").IsUnique();
                entity.Property(e => e.DateCreated).HasColumnType("datetime").HasDefaultValueSql("getdate()");
                entity.Property(e => e.Quantity).HasDefaultValue(1);
            });
        }
    }
}