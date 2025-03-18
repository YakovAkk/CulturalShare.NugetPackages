using CulturalShare.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CulturalShare.Repositories.Interfaces;

public interface IRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
{
    TEntity Add(TEntity entity);
    EntityAddingType AddOrReturnExisting(TEntity entity, Expression<Func<TEntity, bool>> predicate, out TEntity entityOut);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    void AddRange(IEnumerable<TEntity> entity);
    void DeleteRange(IEnumerable<TEntity> entity);
    void UpdateRange(IEnumerable<TEntity> entity);
    int SaveChanges();
    Task<int> SaveChangesAsync();

    /// <summary>
    /// There is no need to call SaveChanges after BulkInsertAsync
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    void BulkInsert(IEnumerable<TEntity> entity);

    /// <summary>
    /// There is no need to call SaveChanges after BulkInsert
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    Task BulkInsertAsync(IEnumerable<TEntity> entity);
}
