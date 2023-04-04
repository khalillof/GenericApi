using Microsoft.EntityFrameworkCore;

namespace GenericApi.Services
{
    public class UnitOfWork<DbEntity> : IUnitOfWork<DbEntity> where DbEntity : DbContext
    {
        private readonly DbEntity _context;
        private readonly IDictionary<Type, object> _store;
        public UnitOfWork(DbEntity contex)
        {
            _context = contex;
            _store = new Dictionary<Type, object>();
        }

        public IRepositoryAsync<DbEntity, TEntity> RepoAsync<TEntity>() where TEntity : class
        {

            if (_store.ContainsKey(typeof(TEntity)))
            {
                var _repo = _store[typeof(TEntity)] as IRepositoryAsync<DbEntity, TEntity>;
                return _repo!;
            }
            else
            {
                var newRepo = new RepositoryAsync<DbEntity, TEntity>(_context);
                _store.Add(typeof(TEntity), newRepo);
                return newRepo;
            }
        }

        public IRepository<DbEntity, TEntity> Repo<TEntity>() where TEntity : class
        {

            if (_store.ContainsKey(typeof(TEntity)))
            {
                var _repo = _store[typeof(TEntity)] as IRepository<DbEntity, TEntity>;
                return _repo!;
            }
            else
            {
                var newRepo = new Repository<DbEntity, TEntity>(_context);
                _store.Add(typeof(TEntity), newRepo);
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
            if (!disposed)
            {
                if (disposing)
                {
                    _store.Clear();
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
