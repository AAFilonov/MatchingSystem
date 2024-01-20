using System;
using MatchingSystem.DataLayer._Technical;

namespace MatchingSystem.DataLayer.Feature;

public class Repository
{
    protected DiplomaMatchingContext _matchingSystemContext => new(DbContextUtils.CreateDbContextOptions().Options);
    
    protected void СallInContext(Action<DiplomaMatchingContext> myDelegate, DiplomaMatchingContext? extContext = null)
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

    protected T СallInContext<T>(Func<DiplomaMatchingContext, T> myDelegate, DiplomaMatchingContext? extContext = null)
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