using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GenericApi.Services
{
    // Generic Repository implementing generic interface takes two generic paremeters DbContext and model class type
    public class Repository<DbEntity, TEntity> : IRepository<DbEntity, TEntity> where DbEntity : DbContext where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> dbSet;

        public Repository(DbEntity context)
        {
            _context = context;
            dbSet = context.Set<TEntity>();
            Query = dbSet.AsQueryable();
        }

        internal IQueryable<TEntity> Query { get; set; }
        public virtual TEntity FindById(object id)
        {
            return dbSet.Find(id)!;
        }
        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Query.FirstOrDefault(predicate);
        }
        public virtual void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }
        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = FindById(id)!;
            if(entityToDelete != null)
            {
            Delete(entityToDelete!);
            }
            
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public virtual int Total(Expression<Func<TEntity, bool>> predicate)
        {
            return Query.Count(predicate);
        }
        public virtual bool Exist(Expression<Func<TEntity, bool>> predicate)
        {
            return Query.Any(predicate);
        }

        public virtual IEnumerable<TEntity> GetList(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "",
            int PageNo = 0, int PageSize = 0)
        {
            var query = QueryEntity(filter, orderBy, includeProperties);
            if (PageSize + PageNo > 0)
            {
                return query.Skip(PageNo * PageSize).Take(PageSize).ToList();
            }
            return query.ToList();
        }

        

        private IQueryable<TEntity> QueryEntity(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "")
        {
            //IQueryable<Tentity> query = dbSet;

            if (filter != null)
            {
                Query = Query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                Query = Query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(Query);
            }
            else
            {
                return Query;
            }
        } 


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
