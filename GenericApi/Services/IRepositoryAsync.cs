using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GenericApi.Services
{
    // Generic Repository interface takes two generic paremeters DbContext and model class type
    public interface IRepositoryAsync<DbEntity,Tentity> : IDisposable where DbEntity : DbContext where Tentity : class
    {
        Task<Tentity> FindByIdAsync(object id);
        Task<Tentity> FindAsync(Expression<Func<Tentity, bool>> predicate);
        Task<Tentity> AddAsync(Tentity entity);
        void Update(Tentity entityToUpdate);
        Task Delete(object id);
        void Delete(Tentity entityToDelete);
        Task<int> SaveChangesAsync();
       Task<int> TotalAsync(Expression<Func<Tentity, bool>> predicate);
       Task<bool> ExistAsync(Expression<Func<Tentity, bool>> predicate);
       Task<IEnumerable<Tentity>> GetListAsync(
           Expression<Func<Tentity, bool>>? filter = null,
           Func<IQueryable<Tentity>, IOrderedQueryable<Tentity>>? orderBy = null,
           string includeProperties = "", int PageNo = 0, int PageSize = 0);
     
        
        
    }
}
