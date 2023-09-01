using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MatchingSystem.DataLayer.Context;
using MatchingSystem.DataLayer.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly DbContext _context;

    public DiplomaMatchingContext _matchingSystemContext
    {
        get
        {
            return _context as DiplomaMatchingContext ??
                   throw new InvalidOperationException("Matching system db context is null");
        }
    }

    public Repository(DbContext context)
    {
        _context = context;
    }

    public TEntity findById(int id)
    {
       var item =  _context.Set<TEntity>().Find(id);
       if (item == null)
           throw new RecordNotFoundException();
        return item;
    }

    public IEnumerable<TEntity> find(Expression<Func<TEntity, bool>> predicate)
    {
        return _context.Set<TEntity>().Where(predicate);
    }

    public IEnumerable<TEntity> findAll()
    {
        return _context.Set<TEntity>().ToList();
    }

    public TEntity save(TEntity item)
    {
        _context.Set<TEntity>().Add(item);
        _context.SaveChanges();
        return item;
    }

    public void update(TEntity item)
    {
        _context.Set<TEntity>().Update(item);
        _context.SaveChanges();
    }

    public void delete(TEntity item)
    {
        _context.Set<TEntity>().Remove(item);
        _context.SaveChanges();
    }
}