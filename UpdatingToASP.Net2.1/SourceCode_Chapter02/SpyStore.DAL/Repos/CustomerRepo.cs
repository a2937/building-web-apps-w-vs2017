using Microsoft.EntityFrameworkCore;
using SpyStore.DAL.EF;
using SpyStore.DAL.Repos.Base;
using SpyStore.DAL.Repos.Interfaces;
using SpyStore.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SpyStore.DAL.Repos
{
    public class CustomerRepo : RepoBase<Customer>, ICustomerRepo
    {
        public CustomerRepo(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public CustomerRepo() : base()
        {
        }

        public override IEnumerable<Customer> GetAll()
        {
            return Table.OrderBy(x => x.FirstName).ThenBy(x => x.LastName);
        }

        public override IEnumerable<Customer> GetRange(int skip, int take)
        {
            return GetRange(Table.OrderBy(x => x.FirstName).ThenBy(x => x.LastName), skip, take);
        }
    }
}