using BookingService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Repository
{
    public interface IRoomBookingRepository
    {
        Task AddAsync(RoomBookingByGuest entity);
        Task<IEnumerable<RoomBookingByHotel>> GetByHotelIdAsync(Guid hotelId);
        Task<IEnumerable<RoomBookingByGuest>> GetByGuestIdAsync(Guid guestId);
    }
    public class RoomBookingRepository : RepositoryBase<RoomBookingByHotel>, IRoomBookingRepository
    {
        public RoomBookingRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task AddAsync(RoomBookingByGuest entity)
        {
            Batch batch = RepositoryContext.CreateBatch();
            AddInBatch(entity, batch);
            AddInBatch((RoomBookingByHotel)entity, batch);
            await batch.CommitAsync();
        }

        public async Task<IEnumerable<RoomBookingByHotel>> GetByHotelIdAsync(Guid hotelId)
        {
            return await Find<RoomBookingByHotel>(h => h.HotelId.Equals(hotelId));
        }

        public async Task<IEnumerable<RoomBookingByGuest>> GetByGuestIdAsync(Guid guestId)
        {
            return await Find<RoomBookingByGuest>(h => h.GuestId.Equals(guestId));
        }
    }
}
