using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MatchingSystem.DataLayer.Utils.Exceptions;

namespace MatchingSystem.DataLayer.Feature;

public class GenericRepository<TEntity> : Repository, IRepository<TEntity> where TEntity : class
{
    public TEntity findById(int id)
    {
        var item = _matchingSystemContext.Set<TEntity>().Find(id);
        if (item == null)
            throw new RecordNotFoundException();
        return item;
    }

    public IEnumerable<TEntity> find(Expression<Func<TEntity, bool>> predicate)
    {
        return _matchingSystemContext.Set<TEntity>().Where(predicate);
    }

    public IEnumerable<TEntity> findAll()
    {
        return _matchingSystemContext.Set<TEntity>().ToList();
    }

    public TEntity save(TEntity item, DiplomaMatchingContext? extContext = null)
    {
        return СallInContext<TEntity>(context =>
        {
            context.Set<TEntity>().Add(item);
            return item;
        }, extContext);
    }

    public IEnumerable<TEntity> saveRange(IEnumerable<TEntity> items, DiplomaMatchingContext? extContext = null)
    {
        return СallInContext<IEnumerable<TEntity>>(context =>
        {
            context.Set<TEntity>().AddRange(items);
            return items;
        }, extContext);
    }

    public void update(TEntity item, DiplomaMatchingContext? extContext = null)
    {
        СallInContext(context => { context.Set<TEntity>().Update(item); }, extContext);
    }

    public void updateRange(IEnumerable<TEntity> items, DiplomaMatchingContext? extContext = null)
    {
        СallInContext(context => { context.Set<TEntity>().UpdateRange(items); }, extContext);
    }

    public void remove(TEntity item, DiplomaMatchingContext? extContext = null)
    {
        СallInContext(context => { context.Set<TEntity>().Remove(item); }, extContext);
    }

    public void removeRange(IEnumerable<TEntity> items, DiplomaMatchingContext? extContext = null)
    {
        СallInContext(context => { context.Set<TEntity>().RemoveRange(items); }, extContext);
    }
}