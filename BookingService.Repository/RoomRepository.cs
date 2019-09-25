using BookingService.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingService.Repository
{
    public interface IRoomRepository
    {
        Task AddAsync(Room entity);
        Task<IEnumerable<Room>> GetByHotelIdAsync(Guid hotelId);
        Task<IEnumerable<Room>> GetByHotelIdByRoomNumberAsync(Guid hotelId, string roomNumber);
    }
    public class RoomRepository : RepositoryBase<Room>, IRoomRepository
    {
        public RoomRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task AddAsync(Room entity)
        {
            await Add(entity);
        }

        public async Task<IEnumerable<Room>> GetByHotelIdAsync(Guid hotelId)
        {
            return await Find<Room>(h => h.HotelId.Equals(hotelId));
        }

        public async Task<IEnumerable<Room>> GetByHotelIdByRoomNumberAsync(Guid hotelId, string roomNumber)
        {
            return await Find<Room>(h => (h.HotelId.Equals(hotelId) && h.Number.Equals(roomNumber)));
        }
    }
}
