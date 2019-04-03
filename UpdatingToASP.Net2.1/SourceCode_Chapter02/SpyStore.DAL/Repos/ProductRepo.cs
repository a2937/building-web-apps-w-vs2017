using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.EF;
using SpyStore.DAL.Repos.Base;
using SpyStore.DAL.Repos.Interfaces;
using SpyStore.Models.Entities;
using SpyStore.Models.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;

namespace SpyStore.DAL.Repos
{
    public class ProductRepo : RepoBase<Product>, IProductRepo
    {
        public ProductRepo(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public ProductRepo() : base()
        {
        }

        public override IEnumerable<Product> GetAll()
        {
            return Table.OrderBy(x => x.ModelName);
        }

        public override IEnumerable<Product> GetRange(int skip, int take)
        {
            return GetRange(Table.OrderBy(x => x.ModelName), skip, take);
        }

        internal ProductAndCategoryBase GetRecord(Product p, Category c)
        {
            return new ProductAndCategoryBase
            {
                CategoryName = c.CategoryName,
                CategoryId = p.CategoryId,
                CurrentPrice = p.CurrentPrice,
                Description = p.Description,
                IsFeatured = p.IsFeatured,
                Id = p.Id,
                ModelName = p.ModelName,
                ModelNumber = p.ModelNumber,
                ProductImage = p.ProductImage,
                ProductImageLarge = p.ProductImageLarge,
                ProductImageThumb = p.ProductImageThumb,
                TimeStamp = p.TimeStamp,
                UnitCost = p.UnitCost,
                UnitsInStock = p.UnitsInStock
            };
        }

        public IEnumerable<ProductAndCategoryBase> GetProductsForCategory(int id)
        {
            return Table
                           .Where(p => p.CategoryId == id)
                           .Include(p => p.Category)
                           .Select(item => GetRecord(item, item.Category))
                           .OrderBy(x => x.ModelName);
        }

        public IEnumerable<ProductAndCategoryBase> GetAllWithCategoryName()
        {
            return Table
                           .Include(p => p.Category)
                           .Select(item => GetRecord(item, item.Category))
                           .OrderBy(x => x.ModelName);
        }

        public IEnumerable<ProductAndCategoryBase> GetFeaturedWithCategoryName()
        {
            return Table
                           .Where(p => p.IsFeatured)
                           .Include(p => p.Category)
                           .Select(item => GetRecord(item, item.Category))
                           .OrderBy(x => x.ModelName);
        }

        public ProductAndCategoryBase GetOneWithCategoryName(int id)
        {
            return Table
                           .Where(p => p.Id == id)
                           .Include(p => p.Category)
                           .Select(item => GetRecord(item, item.Category))
                           .SingleOrDefault();
        }

        public IEnumerable<ProductAndCategoryBase> Search(string searchString)
        {
            return Table
                           .Where(p =>
                               p.Description.IndexOf(searchString, System.StringComparison.OrdinalIgnoreCase) >= 0 || p.ModelName.IndexOf(searchString, System.StringComparison.OrdinalIgnoreCase) >= 0)
                           .Include(p => p.Category)
                           .Select(item => GetRecord(item, item.Category))
                           .OrderBy(x => x.ModelName);
        }
    }
}