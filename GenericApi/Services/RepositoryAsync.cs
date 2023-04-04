using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GenericApi.Services
{
    // Generic Repository implementing generic interface takes two generic paremeters DbContext and model class type
    public class RepositoryAsync<DbEntity, TEntity> : IRepositoryAsync<DbEntity, TEntity> where DbEntity : DbContext where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> dbSet;

        public RepositoryAsync(DbEntity context)
        {
            _context = context;
            dbSet = context.Set<TEntity>();
            Query = dbSet.AsQueryable();
        }

        internal IQueryable<TEntity> Query { get; set; }

        public virtual async Task<TEntity> FindByIdAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }
        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Query.FirstOrDefaultAsync(predicate);
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var eentity = await dbSet.AddAsync(entity);
            eentity.State = EntityState.Added;
            //await context.SaveChangesAsync();
            return eentity.Entity;
        }


        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }
        public virtual async Task Delete(object id)
        {
            TEntity entityToDelete = await FindByIdAsync(id)!;
            if (entityToDelete != null)
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
        public virtual async Task<int> TotalAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Query.CountAsync(predicate);
        }

        public virtual async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Query.AnyAsync(predicate);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // =============================== Async version
        public virtual async Task<IEnumerable<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "", int PageNo = 0, int PageSize = 0)
        {
            //return await QueryEntity(filter, orderBy, includeProperties).ToListAsync();
            var query = QueryEntity(filter, orderBy, includeProperties);
            if (PageSize + PageNo > 0)
            {
                return await query.Skip(PageNo * PageSize).Take(PageSize).ToListAsync();
            }
            return await query.ToListAsync();

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
