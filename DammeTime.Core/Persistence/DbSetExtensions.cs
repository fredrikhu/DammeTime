using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DammeTime.Core.TimeReporting.Persistence
{
    public static class DbSetExtensions
    {
        public static T FirstOrAdd<T>(this DbSet<T> dbSet, Expression<Func<T, bool>> predicate, Func<T> add)
            where T : class
        {
            var value = dbSet.FirstOrDefault(predicate);

            if (value == null)
            {
                value = add();
                dbSet.Add(value);
            }

            return value;
        }
        public static T FirstOrAddDescending<T, TKey>(this DbSet<T> dbSet, Expression<Func<T, bool>> predicate, Func<T> add, Expression<Func<T, TKey>> keySelector)
            where T : class
        {
            var value = dbSet
                .OrderByDescending(keySelector)
                .FirstOrDefault(predicate);

            if (value == null)
            {
                value = add();
                dbSet.Add(value);
            }

            return value;
        }
    }

}
