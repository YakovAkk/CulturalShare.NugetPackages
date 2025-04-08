using CulturalShare.Repositories.Enums;
using CulturalShare.Repositories.Interfaces;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CulturalShare.Repositories;

public class EntityFrameworkRepository<TEntity> : EntityFrameworkReadOnlyRepository<TEntity>, IRepository<TEntity> where TEntity : class
{
    public EntityFrameworkRepository(DbContext context) : base(context) { }

    public virtual TEntity Add(TEntity entity)
    {
        if (_context.Entry(entity).State != EntityState.Detached
            && _context.Entry(entity).State != EntityState.Deleted)
        {
            return entity;
        }

        EntityEntry res = DbSet.Add(entity);
        return (TEntity)res.Entity;
    }

    public virtual EntityAddingType AddOrReturnExisting(TEntity entity, Expression<Func<TEntity, bool>> predicate, out TEntity entityOut)
    {
        var existingEntity = DbSet.FirstOrDefault(predicate);
        if (existingEntity != null)
        {
            entityOut = existingEntity;
            return EntityAddingType.Existing;
        }

        entityOut = Add(entity);
        return EntityAddingType.New;
    }

    public virtual void Update(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
    }

    public virtual void Delete(TEntity entity)
    {
        if (entity == null)
        {
            return;
        }

        DbSet.Remove(entity);
    }

    public virtual void AddRange(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any())
        {
            return;
        }

        DbSet.AddRange(entities);
    }

    /// <summary>
    /// There is no need to call SaveChanges after BulkInsertAsync
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public virtual async Task BulkInsertAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null)
        {
            return;
        }

        var entitiesList = entities.ToList();

        if (!entitiesList.Any())
        {
            return;
        }

        await _context.BulkInsertAsync(entities.ToList()).ConfigureAwait(false);
    }

    /// <summary>
    /// There is no need to call SaveChanges after BulkInsert
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public virtual void BulkInsert(IEnumerable<TEntity> entities)
    {
        if (entities == null)
        {
            return;
        }

        var entitiesList = entities.ToList();

        if (!entitiesList.Any())
        {
            return;
        }

        _context.BulkInsert(entitiesList);
    }

    public virtual void UpdateRange(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any())
        {
            return;
        }

        DbSet.UpdateRange(entities);
    }

    public virtual void DeleteRange(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any())
        {
            return;
        }

        DbSet.RemoveRange(entities);
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
