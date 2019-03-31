using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.EF;
using SpyStore.DAL.Repos.Base;
using SpyStore.DAL.Repos.Interfaces;
using SpyStore.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SpyStore.DAL.Repos
{
    public class CategoryRepo : RepoBase<Category>, ICategoryRepo
    {
        public CategoryRepo(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public CategoryRepo()
        {
        }

        public override IEnumerable<Category> GetAll()
        {
            return Table.OrderBy(x => x.CategoryName);
        }

        public override IEnumerable<Category> GetRange(int skip, int take)
        {
            return GetRange(Table.OrderBy(x => x.CategoryName), skip, take);
        }

        public Category GetOneWithProducts(int? id)
        {
            return Table.Include(x => x.Products).FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Category> GetAllWithProducts()
        {
            return Table.Include(x => x.Products);
        }
    }
}