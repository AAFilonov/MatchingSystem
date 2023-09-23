using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MatchingSystem.DataLayer.Feature;

public interface IRepository<TEntity> where TEntity : class
{
    TEntity findById(int id);
    IEnumerable<TEntity> find(Expression<Func<TEntity, bool>> predicate);
    IEnumerable<TEntity> findAll();

    TEntity save(TEntity item);
    public IEnumerable<TEntity> saveAll(IEnumerable<TEntity> items);
    void update(TEntity item);
    public void updateAll(IEnumerable<TEntity> items);
    void delete(TEntity item);
    public void deleteAll(IEnumerable<TEntity> items);
}