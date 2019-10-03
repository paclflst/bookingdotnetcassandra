using BookingService.Model;
using BookingService.Repository.Helper_Code.Mapping;
using BookingService.Model.Helper_Code.Mapping;
using Cassandra;
using Cassandra.Data.Linq;
using Cassandra.Mapping;

namespace BookingService.Repository
{
    public class RepositoryContext : CassandraContext
    {
        public RepositoryContext(string connectionString) : base(connectionString)
        {
            GetSession().UserDefinedTypes
                .Define(
                    UdtMap.For<HotelInfo>("hotel_info", "booking"),
                    UdtMap.For<Hotel>("hotel_info", "booking")
                );

            //MappingConfiguration.Global.Define(new Map<Hotel>()
            //    .TableName("booking.hotel")
            //                                    .Column(b => b.id, cm => cm.WithName("id"))
            //                                    .Column(b => b.name, cm => cm.WithName("name"))
            //                                    );
            MapTablesToModels.MapTypes(AssemblyHelper.GetTypes());
        }

        public Table<T> GetTable<T>()
        {
            return GetSession().GetTable<T>();
        }

        public Table<T> GetTable<T>(string tableName)
        {
            return GetSession().GetTable<T>(tableName);
        }

        public Batch CreateBatch()
        {
            var batch = GetSession().CreateBatch();
            return new Batch(batch);
        }

    }
}
