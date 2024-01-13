using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Azure;
using MatchingSystem.DataLayer._Technical;
using MatchingSystem.DataLayer.Utils.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature;

public class Repository
{
    protected DiplomaMatchingContext _matchingSystemContext
    {
        get { return new DiplomaMatchingContext(DbContextUtils.CreateDbContextOptions().Options); }
    }
    //TODO генерализовать вызов контекста
    public Repository(DbContext context)
    {
        
    }
}