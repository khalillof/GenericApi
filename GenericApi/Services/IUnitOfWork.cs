using Microsoft.EntityFrameworkCore;

namespace GenericApi.Services
{
    public interface IUnitOfWork<DbEntity>  : IDisposable where DbEntity : DbContext 
    {
        IRepositoryAsync<DbEntity,TEntity> RepoAsync<TEntity>() where TEntity : class;
        IRepository<DbEntity, TEntity> Repo<TEntity>() where TEntity : class;

        void Save();
        Task<int> SaveAsync();
    }
}
