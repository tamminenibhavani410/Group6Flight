using Group6Flight.Models.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace Group6Flight.Models
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected FlightDbContext context { get; set; }
        protected DbSet<T> dbset;

        public Repository(FlightDbContext ctx) {
            context = ctx;
            dbset = context.Set<T>();
        }

        public int Count => dbset.Count();

        // retrieve a list of entities
        public virtual IEnumerable<T> List(QueryOptions<T> options) =>
            BuildQuery(options).ToList();

        // retrieve a single entity (3 overloads)
        public virtual T? Get(int id) => dbset.Find(id);
        public virtual T? Get(string id) => dbset.Find(id);
        public virtual T? Get(QueryOptions<T> options) =>
            BuildQuery(options).FirstOrDefault();

        // insert, update, delete, save
        public virtual void Insert(T entity) => dbset.Add(entity);
        public virtual void Update(T entity) => dbset.Update(entity);
        public virtual void Delete(T entity) => dbset.Remove(entity);
        public virtual void Save() => context.SaveChanges();

        // private helper method to build query expression
        private IQueryable<T> BuildQuery(QueryOptions<T> options)
        {
            IQueryable<T> query = dbset;
            foreach (string include in options.GetIncludes()) {
                query = query.Include(include);
            }
            if (options.HasWhere) { 
                query = query.Where(options.Where);
            }

            return query;
        }
    }
}
