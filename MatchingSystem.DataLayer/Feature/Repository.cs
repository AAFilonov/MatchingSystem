using System;
using MatchingSystem.DataLayer.Utils;

namespace MatchingSystem.DataLayer.Feature;

public class Repository
{
    protected DiplomaMatchingContext _matchingSystemContext => DbContextFactory.GetContext();

    public void CallInTransaction(Action<DiplomaMatchingContext> myDelegate)
    {
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
        if (extContext == null)
        {
            using var context =  _matchingSystemContext;
            myDelegate.Invoke(context);
            context.SaveChanges();
        }
        else
        {
            myDelegate.Invoke(extContext);
            extContext.SaveChanges();
        }
    }

    protected T CallInContext<T>(Func<DiplomaMatchingContext, T> myDelegate, DiplomaMatchingContext? extContext = null)
    {
        if (extContext == null)
        {
            using var context = _matchingSystemContext;
            var result = myDelegate.Invoke(context);
            context.SaveChanges();
            return result;
        }
        else
        {
            T result = myDelegate.Invoke(extContext);
            extContext.SaveChanges();
            return result;
        }
    }
}