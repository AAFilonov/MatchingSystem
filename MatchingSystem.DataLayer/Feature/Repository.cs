using System;
using Common;
using MatchingSystem.DataLayer.Utils;

namespace MatchingSystem.DataLayer.Feature;

public class Repository
{
    protected DiplomaMatchingContext _matchingSystemContext => DbContextFactory.GetContext();

    public void CallInTransaction(Action<DiplomaMatchingContext> myDelegate)
    {
        if (SystemParameters.getInstance().DB_TYPE == DbType.SqliteMemory)
        {
            myDelegate.Invoke(_matchingSystemContext);
            return;
        }
        
        CallInContext(context =>
        {
            using var transaction = context.Database.BeginTransaction();
            try
            {
                myDelegate.Invoke(context);
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw;
            }
        });
    }
    
    protected void CallInContext(Action<DiplomaMatchingContext> myDelegate, DiplomaMatchingContext? extContext = null)
    {
        InvokeDelegateInContext(context =>
        {
            myDelegate.Invoke(context);
            context.SaveChanges();
            return 0;
        },extContext);
    }

    protected T CallInContext<T>(Func<DiplomaMatchingContext, T> myDelegate, DiplomaMatchingContext? extContext = null)
    {
        return InvokeDelegateInContext(context =>
        {
            var result = myDelegate.Invoke(context);
            context.SaveChanges();
            return result;
        }, extContext);
    }

    private T InvokeDelegateInContext<T>(
        Func<DiplomaMatchingContext?, T> callAction,
        DiplomaMatchingContext? extContext = null)
    {
        //inMemory база данных не поддерживает транзакции и при вызове
        //using context вызывает Dispose что нежелательно при ее использовании в тестах
        if (SystemParameters.getInstance().DB_TYPE == DbType.EFInMemory || SystemParameters.getInstance().DB_TYPE == DbType.SqliteMemory)
            extContext = _matchingSystemContext;

        if (extContext != null) 
            return callAction.Invoke(extContext);
        
        using DiplomaMatchingContext context = _matchingSystemContext;
        return callAction.Invoke(context);

    }
}