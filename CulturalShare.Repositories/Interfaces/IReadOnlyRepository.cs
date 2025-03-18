using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Threading.Tasks;

namespace CulturalShare.Repositories.Interfaces;

public interface IReadOnlyRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetAll();
    TEntity Find(object id);
    ValueTask<TEntity> FindAsync(object id);
    ChangeTracker GetChangeTracker();
}
