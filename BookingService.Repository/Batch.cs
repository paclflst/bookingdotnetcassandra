using Cassandra.Data.Linq;
using BookingService.Repository.Interface;
using System.Threading.Tasks;

namespace BookingService.Repository
{
    public class Batch : IBatch
    {
        private Cassandra.Data.Linq.Batch _batch;

        public Batch(Cassandra.Data.Linq.Batch batch)
        {
            _batch = batch;
        }

        public void Append(CqlCommand s)
        {
            _batch.Append(s);
        }

        public void Commit()
        {
            _batch.Execute();
        }

        public async Task CommitAsync()
        {
            await _batch.ExecuteAsync();
        }
    }
}
