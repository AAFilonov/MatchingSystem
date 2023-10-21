using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MatchingSystem.DataLayer._Technical;
using MatchingSystem.DataLayer.Utils.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature;

public class Repository
{
    private readonly DbContext _context;

    protected DiplomaMatchingContext _matchingSystemContext
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
}