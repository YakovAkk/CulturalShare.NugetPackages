using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using CulturalShare.Repositories.Interfaces;

namespace CulturalShare.Repositories;

public class EntityFrameworkReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
{
    protected readonly DbContext _context;

    public EntityFrameworkReadOnlyRepository(DbContext context)
    {
        _context = context;
    }

    protected virtual DbSet<TEntity> DbSet => _context.Set<TEntity>();

    public virtual IQueryable<TEntity> GetAll() => DbSet;
    public virtual TEntity Find(object id) => DbSet.Find(id);
    public ValueTask<TEntity> FindAsync(object id) => DbSet.FindAsync(id);
    public virtual ChangeTracker GetChangeTracker() => _context.ChangeTracker;
}
