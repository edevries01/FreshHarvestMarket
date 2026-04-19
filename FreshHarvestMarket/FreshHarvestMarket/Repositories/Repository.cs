using FreshHarvestMarket.Data;
using Microsoft.EntityFrameworkCore;

namespace FreshHarvestMarket.Repositories
{
    /// <summary>
    /// Generic repository service for every data entit from the FreshHarvest database
    /// Can be used when a moer specific Repository service is not desired
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FreshHarvestRepository<T> : IRepository<T> where T : class
    {
        protected FreshHarvestContext context { get; set; }
        private DbSet<T> dbset { get; set; }

        public FreshHarvestRepository(FreshHarvestContext ctx)
        {
            context = ctx;
            dbset = context.Set<T>();
        }

        public virtual IQueryable<T> GetAll() => dbset;
        public virtual T? Get(int id) => dbset.Find(id);
        public virtual void Insert(T entity) => dbset.Add(entity);
        public virtual void Update(T entity) => dbset.Update(entity);
        public virtual void Delete(T entity) => dbset.Remove(entity);
        public virtual void Save() => context.SaveChanges();
    }
}
