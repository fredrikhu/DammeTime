﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DammeTime.Core.TimeReporting.Persistence
{
    public static class DbSetExtensions
    {
        public static T SingleOrAdd<T>(this DbSet<T> dbSet, Expression<Func<T, bool>> predicate, Func<T> add)
            where T : class
        {
            var value = dbSet.SingleOrDefault(predicate);

            if (value == null)
            {
                value = add();
                dbSet.Add(value);
            }

            return value;
        }
    }

}
