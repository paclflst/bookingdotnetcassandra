using BookingService.Repository.Interface;
using System;

namespace BookingService.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private RepositoryContext _repositoryContext;
        private IHotelRepository _hotel;
        private IGuestRepository _guest;
        private IRoomRepository _room;
        private IRoomBookingRepository _roomBooking;

        public UnitOfWork(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IHotelRepository Hotel
        {
            get
            {
                if (_hotel == null)
                {
                    _hotel = new HotelRepository(_repositoryContext);
                }
                return _hotel;
            }
        }

        public IGuestRepository Guest
        {
            get
            {
                if (_guest == null)
                {
                    _guest = new GuestRepository(_repositoryContext);
                }
                return _guest;
            }
        }

        public IRoomRepository Room
        {
            get
            {
                if (_room == null)
                {
                    _room = new RoomRepository(_repositoryContext);
                }
                return _room;
            }
        }

        public IRoomBookingRepository RoomBooking
        {
            get
            {
                if (_roomBooking == null)
                {
                    _roomBooking = new RoomBookingRepository(_repositoryContext);
                }
                return _roomBooking;
            }
        }

        public void Dispose()
        {
            _repositoryContext.Dispose();
        }
    }
}
