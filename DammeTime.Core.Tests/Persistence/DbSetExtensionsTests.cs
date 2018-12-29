using System.Collections.Generic;
using System.Linq;
using Xunit;
using DammeTime.Core.TimeReporting.Persistence;

namespace DammeTime.Core.Tests.Persistence
{
    public class DbSetExtensionsTests
    {
        protected List<string> _data;

        public class A_db_set_with_no_match : DbSetExtensionsTests
        {
            [Fact, UnitTest]
            public void adds_entity_to_db_set()
            {
                _data = new List<string>();
                var dbSet = DbSetMock.MockDbSet(_data);

                dbSet.Object.SingleOrAdd(x => x == "test", () => "test");

                Assert.Equal("test", _data.Single());
            }
        }

        public class A_db_set_with_a_match : DbSetExtensionsTests
        {
            [Fact, UnitTest]
            public void does_not_add_entity_to_db_set()
            {
                _data = new List<string> { "test" };
                var dbSet = DbSetMock.MockDbSet(_data);

                dbSet.Object.SingleOrAdd(x => x == "test", () => "test");

                Assert.Equal("test", _data.Single());
            }
        }
    }
}
