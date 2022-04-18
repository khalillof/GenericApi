using Microsoft.EntityFrameworkCore;

namespace GenericApi.Services
{
    public class UnitOfWork<DbEntity> : IUnitOfWork<DbEntity> where DbEntity : DbContext
    {
        private readonly DbEntity _context;
        private readonly IDictionary<Type, object> _store;
        public UnitOfWork(DbEntity contex)
        {
            this._context = contex;
            _store = new Dictionary<Type, object>();
        }

        public IGenericRepository<DbEntity, TEntity> Repo<TEntity>() where TEntity : class
        {

            if (this._store.ContainsKey(typeof(TEntity)))
            {
                var _repo = this._store[typeof(TEntity)] as IGenericRepository<DbEntity, TEntity>;
                return _repo!;
            }
            else
            {
                var newRepo = new GenericRepository<DbEntity, TEntity>(_context);
                this._store.Add(typeof(TEntity), newRepo);
                return newRepo;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        public async Task<int> SaveAsync()
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
                    this._store.Clear();
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
