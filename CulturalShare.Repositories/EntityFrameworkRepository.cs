using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monto.Repositories
{
    public class EntityFrameworkRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;

        public EntityFrameworkRepository(DbContext context)
        {
            _context = context;
        }

        public virtual DbContext Context => _context;

        public virtual IQueryable<TEntity> GetAll() => DbSet;

        public virtual DbSet<TEntity> DbSet => Context.Set<TEntity>();

        public virtual TEntity Add(TEntity entity)
        {
            if (Context.Entry(entity).State != EntityState.Detached
                && Context.Entry(entity).State != EntityState.Deleted)
            {
                return entity;
            }

            EntityEntry res = DbSet.Add(entity);
            return (TEntity)res.Entity;
        }

        public virtual void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            DbSet.Remove(entity);
        }

        public virtual void DeleteRange(List<TEntity> entity)
        {
            if (entity == null)
            {
                return;
            }

            DbSet.RemoveRange(entity);
        }

        public virtual TEntity Find(object id) => DbSet.Find(id);

        public ValueTask<TEntity> FindAsync(object id) => DbSet.FindAsync(id);

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public virtual void AddRange(List<TEntity> entity)
        {
            if (entity == null)
            {
                return;
            }

            DbSet.AddRange(entity);
        }
    }
}
