using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MatchingSystem.DataLayer.Feature;

public interface IRepository<TEntity> where TEntity : class
{
    TEntity? findById(int id);
    IEnumerable<TEntity> find(Expression<Func<TEntity, bool>> predicate);
    IEnumerable<TEntity> findAll();

    TEntity save(TEntity item, DiplomaMatchingContext? extContext = null);
    public IEnumerable<TEntity> saveRange(IEnumerable<TEntity> items, DiplomaMatchingContext? extContext = null);
    void update(TEntity item, DiplomaMatchingContext? extContext = null);
    public void updateRange(IEnumerable<TEntity> items, DiplomaMatchingContext? extContext = null);
    void remove(TEntity item, DiplomaMatchingContext? extContext = null);
    public void removeRange(IEnumerable<TEntity> items, DiplomaMatchingContext? extContext = null);
}