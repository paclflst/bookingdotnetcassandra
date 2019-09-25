using Cassandra;
using Cassandra.Mapping;
using System.Threading.Tasks;

namespace BookingService.DAO
{
    public interface ICassandraDAO
    {
        ISession GetSession();
    }

    public class CassandraDAO : ICassandraDAO
    {
        private static Cluster Cluster;
        private static ISession Session;

        public CassandraDAO()
        {
            SetCluster();
        }

        private void SetCluster()
        {
            if (Cluster == null)
            {
                Cluster = Connect();
            }
        }

        public ISession GetSession()
        {
            if (Cluster == null)
            {
                SetCluster();
                Session = Cluster.Connect();
            }
            else if (Session == null)
            {
                Session = Cluster.Connect();
            }

            return Session;
        }

        private Cluster Connect()
        {
            string user = "";
            string pwd = "";
            string[] nodes = new string[1] { "localhost" };

            QueryOptions queryOptions = new QueryOptions()
                 .SetConsistencyLevel(ConsistencyLevel.LocalQuorum);

            Cluster cluster = Cluster.Builder()
                .AddContactPoints(nodes)
                //.WithCredentials(user, pwd)
                .WithQueryOptions(queryOptions)
                .Build();

            return cluster;
        }

        private async Task Execute<T>(Mapper mapper)
        {
            await mapper.FetchAsync<T>();
        }

        private string GetAppSetting(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }
    }
}