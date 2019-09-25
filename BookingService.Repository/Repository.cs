using Cassandra;
using Cassandra.Data.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CassandraDAL
{
    /*public class Repository : IRepository
    {
        private ISession _session { get; set; }

        public Repository(ISession session)
        {
            _session = session;
        }

        public T Get<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : IEntity
        {
            return Find<T>(predicate).FirstOrDefault();
        }

        public IEnumerable<T> Find<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        where T : IEntity
        {
            return new Table<T>(_session).Where(predicate).Execute();
        }

        public void Create<T>(T entity) where T : IEntity
        {
            new Table<T>(_session).Insert(entity).Execute();
        }
    }*/
}
