using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MatchingSystem.DataLayer.Utils.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature;

public class GenericRepository<TEntity> : Repository, IRepository<TEntity> where TEntity : class
{
    public GenericRepository(DbContext context) : base(context)
    {
    }

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

    public TEntity save(TEntity item)
    {
        using (var context = _matchingSystemContext)
        {
            context.Set<TEntity>().Add(item);
            context.SaveChanges();
        }

        return item;
    }

    public IEnumerable<TEntity> saveRange(IEnumerable<TEntity> items)
    {
        using (var context = _matchingSystemContext)
        {
            context.Set<TEntity>().AddRange(items);
            context.SaveChanges();
            return items;
        }
    }

    public void update(TEntity item)
    {
        using (var context = _matchingSystemContext)
        {
            context.Set<TEntity>().Update(item);
            context  .SaveChanges();
        }
    }

    public void updateRange(IEnumerable<TEntity> items)
    {
        using (var context = _matchingSystemContext)
        {
            context.Set<TEntity>().UpdateRange(items);
            context.SaveChanges();
        }
    }

    public void delete(TEntity item)
    {
        using (var context = _matchingSystemContext)
        {
            context.Set<TEntity>().Remove(item);
            context.SaveChanges();
        }
    }

    public void deleteRange(IEnumerable<TEntity> items)
    {
        using (var context = _matchingSystemContext)
        {
            context.Set<TEntity>().RemoveRange(items);
            context.SaveChanges();
        }
    }
}