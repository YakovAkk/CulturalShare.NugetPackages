using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monto.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        TEntity Find(object id);
        ValueTask<TEntity> FindAsync(object id);
        TEntity Add(TEntity entity);
        void AddRange(List<TEntity> entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void DeleteRange(List<TEntity> entity);
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
