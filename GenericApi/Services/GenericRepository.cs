using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GenericApi.Services
{
    // Generic Repository implementing generic interface takes two generic paremeters DbContext and model class type
    public class GenericRepository<DbEntity,TEntity> : IGenericRepository<DbEntity, TEntity> where DbEntity : DbContext  where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> dbSet;

        public GenericRepository(DbEntity context)
        {
            this._context = context;
            this.dbSet = context.Set<TEntity>();
            this.Query = dbSet.AsQueryable();
        }

        internal IQueryable<TEntity> Query { get; set; }
        public virtual TEntity GetById(object id)
        {
            return dbSet.Find(id)!;
        }
        public virtual void Insert(TEntity entity)
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
            TEntity entityToDelete = dbSet.Find(id)!;
            Delete(entityToDelete!);
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
            return this.Query.Count(predicate);
        }
        public virtual bool Exist(Expression<Func<TEntity, bool>> predicate)
        {
            return this.Query.Any(predicate);
        }

        public virtual IEnumerable<TEntity> GetList(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "")
        {
            return QueryEntity(filter, orderBy, includeProperties).ToList();
        }

        // =============================== Async version
        public virtual async Task<IEnumerable<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "")
        {
           return await QueryEntity(filter, orderBy, includeProperties).ToListAsync();      
        }

        public virtual async Task<IEnumerable<TEntity>> PagerAsync(
            Expression<Func<TEntity, bool>>? filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, 
            string includeProperties = "", 
            int PageNo = 0, int PageSize = 0)
        {
            var query = QueryEntity(filter, orderBy, includeProperties);
            if (PageSize + PageNo > 0){
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
                this.Query = this.Query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                this.Query = this.Query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(this.Query);
            }
            else
            {
                return this.Query;
            }
        }

       public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await dbSet.FindAsync(id);    
        }
       
        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            var eentity = await dbSet.AddAsync(entity);
            eentity.State = EntityState.Added;
            //await context.SaveChangesAsync();
            return eentity.Entity;
        }

        public virtual async Task<int> TotalAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await this.Query.CountAsync(predicate);
        }

        public virtual async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await this.Query.AnyAsync(predicate);
        }

       public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
