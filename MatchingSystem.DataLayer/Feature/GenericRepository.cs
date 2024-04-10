using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MatchingSystem.DataLayer.Feature;

public class GenericRepository<TEntity> : Repository, IRepository<TEntity> where TEntity : class
{
    public TEntity findById(int id)
    {
        return _matchingSystemContext.Set<TEntity>().Find(id) ??
               throw new Exception(typeof(TEntity).Name + " with id =" + id + " not found.");
    }

    public IEnumerable<TEntity> find(Expression<Func<TEntity, bool>> predicate)
    {
        return _matchingSystemContext.Set<TEntity>().Where(predicate);
    }

    public DbSet<TEntity> findAll()
    {
        return _matchingSystemContext.Set<TEntity>();
    }

    public TEntity save(TEntity item, DiplomaMatchingContext? extContext = null)
    {
        return CallInContext<TEntity>(context =>
        {
            if (SystemParameters.getInstance().DB_TYPE == DbType.SqliteMemory)
            {
                HandleSqliteIdAutoincrement(item, context);
            }
            context.Set<TEntity>().Add(item);
            return item;
        }, extContext);
    }
    
   
    public IEnumerable<TEntity> saveRange(IEnumerable<TEntity> items, DiplomaMatchingContext? extContext = null)
    {
        return CallInContext<IEnumerable<TEntity>>(context =>
        {
            context.Set<TEntity>().AddRange(items);
            return items;
        }, extContext);
    }

    public void update(TEntity item, DiplomaMatchingContext? extContext = null)
    {
        CallInContext(context => { context.Set<TEntity>().Update(item); }, extContext);
    }

    public void updateRange(IEnumerable<TEntity> items, DiplomaMatchingContext? extContext = null)
    {
        CallInContext(context => { context.Set<TEntity>().UpdateRange(items); }, extContext);
    }

    public void remove(TEntity item, DiplomaMatchingContext? extContext = null)
    {
        CallInContext(context => { context.Set<TEntity>().Remove(item); }, extContext);
    }

    public void removeRange(IEnumerable<TEntity> items, DiplomaMatchingContext? extContext = null)
    {
        CallInContext(context => { context.Set<TEntity>().RemoveRange(items); }, extContext);
    }

    private void HandleSqliteIdAutoincrement(TEntity item, DiplomaMatchingContext context)
    {
        //у Sqlite базы не роаботает автоинкримент для сущеностей EF Core. Находим и сетим первичный ключ с помощью рефлексии
        //база на Sqlite используется исключительно в тестах,и на работу приложения это не повлияет
        IEntityType entityType = context.Model.FindEntityType(typeof(TEntity))!;
        //Делаем допущения что id не явялется составным и имеет тип int
        IKey? primaryKey = entityType.FindPrimaryKey();
        int? currentId = (int?)context.Entry(item).Property(primaryKey.Properties.Single()).CurrentValue;
        if (currentId is null or 0) // если id  не заполнен  или равен 0
        {
            //находим запись с самым большим id  в таблице
            TEntity lastItemInTable =
                context.Set<TEntity>().AsEnumerable().OrderBy(entity => GetKey(entity, context)).Last();
            int maxId = (int)context.Entry(lastItemInTable).Property(primaryKey.Properties.Single()).CurrentValue!;
            //вручную сетим id как миксимальный + 1
            context.Entry(item).Property(primaryKey.Properties.Single()).CurrentValue = maxId + 1;
        }
    }

    public int GetKey<T>(T entity, DiplomaMatchingContext context)
    {
        var keyName = context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
            .Select(x => x.Name).Single();

        return (int)entity.GetType().GetProperty(keyName).GetValue(entity, null);
    }
}