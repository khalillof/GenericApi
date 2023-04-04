using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GenericApi.Services
{
    // Generic Repository interface takes two generic paremeters DbContext and model class type
    public interface IRepository<DbEntity,Tentity> : IDisposable where DbEntity : DbContext where Tentity : class
    {
        Tentity FindById(object id);
        Tentity Find(Expression<Func<Tentity, bool>> predicate);
        void Add(Tentity entity);
        void Update(Tentity entityToUpdate);
        void Delete(object id);
        void Delete(Tentity entityToDelete);
        void SaveChanges();
        int Total(Expression<Func<Tentity, bool>> predicate);
        bool Exist(Expression<Func<Tentity, bool>> predicate);

        IEnumerable<Tentity> GetList(
           Expression<Func<Tentity, bool>>? filter = null,
           Func<IQueryable<Tentity>, IOrderedQueryable<Tentity>>? orderBy = null,
           string includeProperties = "", int PageNo = 0, int PageSize = 0);


    }
}
