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
    public IEnumerable<TEntity> saveRange(IEnumerable<TEntity> items);
    void update(TEntity item);
    public void updateRange(IEnumerable<TEntity> items);
    void delete(TEntity item);
    public void deleteRange(IEnumerable<TEntity> items);
}