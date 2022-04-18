using Microsoft.EntityFrameworkCore;

namespace GenericApi.Services
{
    public interface IUnitOfWork<DbEntity>  : IDisposable where DbEntity : DbContext 
    {
        IGenericRepository<DbEntity,TEntity> Repo<TEntity>() where TEntity : class;

        void Save();
        Task<int> SaveAsync();
    }
}
