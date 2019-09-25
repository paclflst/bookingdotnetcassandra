using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Repository.Interface
{
    public interface IUnitOfWork
    {
        IHotelRepository Hotel { get; }
        IGuestRepository Guest { get; }
        IRoomRepository Room { get; }
        IRoomBookingRepository RoomBooking { get; }
        void Dispose();
    }
}
