using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GenericApi.Services
{
    // Generic Repository interface takes two generic paremeters DbContext and model class type
    public interface IGenericRepository<DbEntity,Tentity> : IDisposable where DbEntity : DbContext where Tentity : class
    {
        Tentity GetById(object id);
        void Insert(Tentity entity);
        void Update(Tentity entityToUpdate);
        void Delete(object id);
        void Delete(Tentity entityToDelete);
        void SaveChanges();
        int Total(Expression<Func<Tentity, bool>> predicate);
        bool Exist(Expression<Func<Tentity, bool>> predicate);

        IEnumerable<Tentity> GetList(
           Expression<Func<Tentity, bool>>? filter = null,
           Func<IQueryable<Tentity>, IOrderedQueryable<Tentity>>? orderBy = null,
           string includeProperties = "");

        // async versions
       Task<IEnumerable<Tentity>> GetListAsync(
           Expression<Func<Tentity, bool>>? filter = null,
           Func<IQueryable<Tentity>, IOrderedQueryable<Tentity>>? orderBy = null,
           string includeProperties = "");
        Task<IEnumerable<Tentity>> PagerAsync(Expression<Func<Tentity, bool>>? filter = null, Func<IQueryable<Tentity>, IOrderedQueryable<Tentity>>? orderBy = null, string includeProperties = "", int PageNo = 0, int PageSize = 0);
        
        Task<Tentity> GetByIdAsync(object id);
        
        Task<Tentity> InsertAsync(Tentity entity);
        Task<int> TotalAsync(Expression<Func<Tentity, bool>> predicate);
        Task<bool> ExistAsync(Expression<Func<Tentity, bool>> predicate);
        Task<int> SaveChangesAsync();
    }
}
