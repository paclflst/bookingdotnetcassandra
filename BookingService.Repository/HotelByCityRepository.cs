using BookingService.Model;
using BookingService.Repository.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingService.Repository
{
    public interface IHotelByCityRepository
    {
        Task<IEnumerable<HotelByCity>> GetByCityAsync(string city);
        void AddInBatch<HotelByCity>(HotelByCity entity, IBatch batch);
    }
    public class HotelByCityRepository : RepositoryBase<HotelByCity>, IHotelByCityRepository
    {
        public HotelByCityRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<HotelByCity>> GetByCityAsync(string city)
        {
            return await Find<HotelByCity>(h => h.city == city);
        }

    }
}
