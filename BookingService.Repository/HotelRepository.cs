using BookingService.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace BookingService.Repository
{
    public interface IHotelRepository
    {
        Task<IEnumerable<Hotel>> GetByIdAsync(Guid hotelId);
        Task<IEnumerable<Hotel>> GetByCityAsync(string city);
        Task AddAsync(Hotel entity);
    }
    public class HotelRepository : RepositoryBase<Hotel>, IHotelRepository
    {
        public HotelRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task AddAsync(Hotel entity)
        {
            Batch batch = RepositoryContext.CreateBatch();
            AddInBatch(entity, batch);
            AddInBatch(new HotelByCity(entity), batch);
            await batch.CommitAsync();
        }

        public async Task<IEnumerable<Hotel>> GetByIdAsync(Guid hotelId)
        {
            return await Find<Hotel>(h => h.id.Equals(hotelId));
        }

        public async Task<IEnumerable<Hotel>> GetByCityAsync(string city)
        {
            return await Find<HotelByCity>(h => h.city == city).ContinueWith((hbc) => 
                {
                    var result = hbc.Result;
                    return result.ToList()
                    .Select(h => new Hotel(h.hotel_info) { id = h.hotel_id });
                });
        }

    }
}
