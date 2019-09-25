using Cassandra;
using System;

namespace BookingService.Repository
{
    public class CassandraContext : IDisposable
    {
        private readonly string[] _contactPoints;
        private Cluster _cluster;
        private ISession _session;


        private int _currentBatchSize = 0;
        private object _batchLock = new object();
        private BatchStatement _currentBatch = new BatchStatement();

        public CassandraContext(string[] contactPoints)
        {
            _contactPoints = contactPoints;
            SetCluster();
        }

        private void SetCluster()
        {
            if (_cluster == null)
            {
                _cluster = Connect();
            }
        }

        protected ISession GetSession()
        {
            if (_cluster == null)
            {
                SetCluster();
                _session = _cluster.Connect();
            }
            else if (_session == null)
            {
                _session = _cluster.Connect();
            }

            return _session;
        }

        private Cluster Connect()
        {
            string user = "";
            string pwd = "";
            //string[] nodes = new string[1] { "localhost" };

            QueryOptions queryOptions = new QueryOptions()
                 .SetConsistencyLevel(ConsistencyLevel.LocalQuorum);

            Cluster cluster = Cluster.Builder()
                .AddContactPoints(_contactPoints)
                //.WithCredentials(user, pwd)
                .WithQueryOptions(queryOptions)
                .Build();

            return cluster;
        }

        public void Dispose()
        {
            lock (_currentBatch)
            {
                if (_currentBatchSize > 0)
                    _session.Execute(_currentBatch);
            }

            _session.Dispose();
        }
    }
}
