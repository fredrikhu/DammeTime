using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace DammeTime.Core.Tests.Persistence
{
    public static class DbSetMock
    {
        public static Mock<DbSet<T>> MockDbSet<T>(List<T> data)
            where T : class
        {
            var q = data.AsQueryable();
            var result = new Mock<DbSet<T>>();
            result.As<IQueryable<T>>()
                .Setup(m => m.Provider).Returns(q.Provider);
            result.As<IQueryable<T>>()
                .Setup(m => m.Expression).Returns(q.Expression);
            result.As<IQueryable<T>>()
                .Setup(m => m.ElementType).Returns(q.ElementType);
            result.As<IQueryable<T>>()
                .Setup(m => m.GetEnumerator()).Returns(q.GetEnumerator());

            result.Setup(m => m.Add(It.IsAny<T>()))
                .Callback<T>(x => data.Add(x));

            return result;
        }
    }
}
