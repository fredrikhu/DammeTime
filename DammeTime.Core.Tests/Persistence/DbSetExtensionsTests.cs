using System.Collections.Generic;
using System.Linq;
using Xunit;
using DammeTime.Core.TimeReporting.Persistence;

namespace DammeTime.Core.Tests.Persistence
{
    public class DbSetExtensionsTests
    {
        protected List<Domain> _data;

        public class FirstOrDefault_on_a_db_set_with_no_items : DbSetExtensionsTests
        {
            [Fact, UnitTest]
            public void adds_entity_to_db_set()
            {
                _data = new List<Domain>();
                var dbSet = DbSetMock.MockDbSet(_data);

                dbSet.Object.FirstOrAdd(x => x.Key == "test", () => new Domain("test", 1));

                Assert.Single(_data);
            }
        }

        public class FirstOrDefault_on_a_db_set_with_no_match : DbSetExtensionsTests
        {
            [Fact, UnitTest]
            public void adds_entity_to_db_set()
            {
                _data = new List<Domain> { new Domain("other", 1) };
                var dbSet = DbSetMock.MockDbSet(_data);

                dbSet.Object.FirstOrAdd(x => x.Key == "test", () => new Domain("test", 2));

                Assert.Single(_data, x => x.Key == "test");
            }
        }

        public class FirstOrDefault_on_a_db_set_with_one_match : DbSetExtensionsTests
        {
            [Fact, UnitTest]
            public void does_not_add_entity_to_db_set()
            {
                _data = new List<Domain> { new Domain("test", 1) };
                var dbSet = DbSetMock.MockDbSet(_data);

                dbSet.Object.FirstOrAdd(x => x.Key == "test", () => new Domain("test", 1));

                Assert.Single(_data);
            }

            [Fact, UnitTest]
            public void returns_the_first_with_match()
            {
                _data = new List<Domain> { new Domain("test", 1), new Domain("test", 2) };
                var dbSet = DbSetMock.MockDbSet(_data);

                var actual = dbSet.Object.FirstOrAdd(x => x.Key == "test", () => new Domain("test", 1));

                Assert.Equal(1, actual.Order);
            }
        }

        public class FirstOrDefaultDescending_on_a_db_set_with_no_items : DbSetExtensionsTests
        {
            [Fact, UnitTest]
            public void adds_entity_to_db_set()
            {
                _data = new List<Domain>();
                var dbSet = DbSetMock.MockDbSet(_data);

                dbSet.Object.FirstOrAddDescending(x => x.Key == "test", () => new Domain("test", 1), x => x.Order);

                Assert.Single(_data);
            }
        }

        public class FirstOrDefaultDescending_on_a_db_set_with_no_match : DbSetExtensionsTests
        {
            [Fact, UnitTest]
            public void adds_entity_to_db_set()
            {
                _data = new List<Domain> { new Domain("other", 1) };
                var dbSet = DbSetMock.MockDbSet(_data);

                dbSet.Object.FirstOrAddDescending(x => x.Key == "test", () => new Domain("test", 2), x => x.Order);

                Assert.Single(_data, x => x.Key == "test");
            }
        }

        public class FirstOrDefaultDescending_on_a_db_set_with_one_match : DbSetExtensionsTests
        {
            [Fact, UnitTest]
            public void does_not_add_entity_to_db_set()
            {
                _data = new List<Domain> { new Domain("test", 1) };
                var dbSet = DbSetMock.MockDbSet(_data);

                dbSet.Object.FirstOrAddDescending(x => x.Key == "test", () => new Domain("test", 1), x => x.Order);

                Assert.Single(_data);
            }

            [Theory, UnitTest]
            [InlineData(2, 1, 2)]
            [InlineData(1, 3, 3)]
            public void returns_the_first_with_match_ordered(int first, int second, int expected)
            {
                _data = new List<Domain> { new Domain("test", first), new Domain("test", second) };
                var dbSet = DbSetMock.MockDbSet(_data);

                var actual = dbSet.Object.FirstOrAddDescending(x => x.Key == "test", () => new Domain("test", 100), x => x.Order);

                Assert.Equal(expected, actual.Order);
            }
        }

        public class Domain
        {
            public string Key { get; set; }
            public int Order { get; set; }

            public Domain(string key, int order)
            {
                Key = key;
                Order = order;
            }
        }
    }
}
