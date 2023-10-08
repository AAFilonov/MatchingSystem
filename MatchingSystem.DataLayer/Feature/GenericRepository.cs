using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MatchingSystem.DataLayer.Context;
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
       var item =  _matchingSystemContext.Set<TEntity>().Find(id);
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
        _matchingSystemContext.Set<TEntity>().Add(item);
        _matchingSystemContext.SaveChanges();
        return item;
    }
    public IEnumerable<TEntity> saveAll(IEnumerable<TEntity> items)
    {
        foreach (var tEntity in items)
        {
            _matchingSystemContext.Set<TEntity>().Add(tEntity);
        }
        _matchingSystemContext.SaveChanges();
        return items;
    }


    public void update(TEntity item)
    {
        _matchingSystemContext.Set<TEntity>().Update(item);
        _matchingSystemContext.SaveChanges();
    }
    public void updateAll(IEnumerable<TEntity> items)
    {
        foreach (var tEntity in items)
        {
            _matchingSystemContext.Set<TEntity>().Update(tEntity);
        }
        _matchingSystemContext.SaveChanges();
    }

    public void delete(TEntity item)
    {
        _matchingSystemContext.Set<TEntity>().Remove(item);
        _matchingSystemContext.SaveChanges();
    }
    
    public void deleteAll(IEnumerable<TEntity> items)
    {
        foreach (var tEntity in items)
        {
            _matchingSystemContext.Set<TEntity>().Update(tEntity);
        }
        _matchingSystemContext.SaveChanges();
    }
}