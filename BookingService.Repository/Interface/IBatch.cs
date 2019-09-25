using Cassandra.Data.Linq;
using System.Threading.Tasks;

namespace BookingService.Repository.Interface
{
    public interface IBatch
    {
        void Append(CqlCommand c);
        void Commit();
        Task CommitAsync();
    }
}
