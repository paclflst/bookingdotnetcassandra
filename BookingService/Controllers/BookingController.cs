using BookingService.Models;
using BookingService.Model;
using BookingService.Repository;
using BookingService.Repository.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Hotel = BookingService.Model.Hotel;
using RepositoryContext = BookingService.Repository.RepositoryContext;
using Guest = BookingService.Model.Guest;
using Room = BookingService.Model.Room;
using RoomBookingByHotel = BookingService.Model.RoomBookingByHotel;
using RoomBookingByGuest = BookingService.Model.RoomBookingByGuest;

namespace BookingService.Controllers
{
    public class BookingController : ApiController
    {
        private static RepositoryContext dao;
        private static IUnitOfWork rep;

        protected RepositoryContext HotelDAO
        {
            get
            {
                if (dao == null)
                {
                    dao = new RepositoryContext(new string[1] { "localhost" });
                }
                return dao;
            }
        }

        protected IUnitOfWork UnitOfWork
        {
            get
            {
                if (rep == null)
                {
                    var dd = HotelDAO;
                    rep = new UnitOfWork(dd);
                }
                return rep;
            }
        }

        [HttpPost]
        [ActionName("city")]
        public async Task<IEnumerable<HotelBindingModel>> GetHotelByCityAsync([FromBody][Required]CityBindingModel city)
        {
            IEnumerable <Hotel> hotelsByCity = await UnitOfWork.Hotel.GetByCityAsync(city.City);
            return hotelsByCity.ToList()
                    .Select(hbc => new HotelBindingModel()
                        {
                            HotelId = hbc.id,
                            City = hbc.city,
                            Name = hbc.name,
                            Phone = hbc.phone,
                            Address = hbc.address,
                            Zip = hbc.zip
                        });
        }

        [HttpPost]
        [ActionName("hotel")]
        public async Task<HotelBindingModel> AddHotelAsync([FromBody][Required]HotelBindingModel hotel)
        {
            IEnumerable<Hotel> hotelsByCity = await UnitOfWork.Hotel.GetByCityAsync(hotel.City);
            if (hotelsByCity.Any(h => h.name == hotel.Name))
            {
                throw new Exception("Hotel already exists");
            }
            Hotel hotelInfo = new Hotel()
            {
                //id = Guid.NewGuid(),
                name = hotel.Name,
                phone = hotel.Phone,
                address = hotel.Address,
                city = hotel.City,
                state = hotel.State,
                zip = hotel.Zip,
            };
            await UnitOfWork.Hotel.AddAsync(hotelInfo);
            hotel.HotelId = hotelInfo.id;
            return hotel;
        }

        [HttpPost]
        [ActionName("guest")]
        public async Task<GuestBindingModel> AddGuestAsync([FromBody][Required]GuestBindingModel guest)
        {
            IEnumerable<Guest> result = await UnitOfWork.Guest.GetByEmailAsync(guest.Email);
            List<Guest> guests = result.ToList();
            if (guests.Any())
            {
                throw new Exception("Guest already exists");
            }
            Guest guestInfo = new Guest()
            {
                Name = guest.Email,
                Email = guest.Email
            };
            await UnitOfWork.Guest.AddAsync(guestInfo);
            guest.GuestId = guestInfo.Id;
            return guest;
        }

        [HttpPost]
        [ActionName("room")]
        public async Task<RoomBindingModel> AddRoomAsync([FromBody][Required]RoomBindingModel room)
        {
            var roomInfo = new Room()
            {
                HotelId = room.HotelId,
                Number = room.Number,
                Type = room.Type
            };
            IEnumerable<Hotel> hotelResult = await UnitOfWork.Hotel.GetByIdAsync(roomInfo.HotelId);
            List<Hotel> hotels = hotelResult.ToList();
            if (hotels.Any())
            {
                IEnumerable<Room> roomResult = await UnitOfWork.Room.GetByHotelIdByRoomNumberAsync(roomInfo.HotelId, roomInfo.Number);
                List<Room> rooms = roomResult.ToList();
                if (rooms.Any())
                {
                    throw new Exception("Room with such number already exists");
                }
                await UnitOfWork.Room.AddAsync(roomInfo);
            }
            else
            {
                throw new Exception("Provided hotel does not exist");
            }
            room.RoomId = roomInfo.Id;
            return room;
        }

        [HttpPost]
        [ActionName("freerooms")]
        public async Task<IEnumerable<Room>> GetFreeRoomsAsync([FromBody][Required]BookingByHotelBindingModel bookingQuery)
        {
            Task<IEnumerable<Room>> getRooms = UnitOfWork.Room.GetByHotelIdAsync(bookingQuery.HotelId.Value);
            Task<IEnumerable<RoomBookingByHotel>> getBookings = UnitOfWork.RoomBooking.GetByHotelIdAsync(bookingQuery.HotelId.Value);
            await Task.WhenAll(getRooms, getBookings);
            IEnumerable<Room> roomResult = getRooms.Result;
            List<Room> rooms = roomResult.ToList();

            IEnumerable<RoomBookingByHotel> bookingResult = getBookings.Result;
            List<RoomBookingByHotel> bookings = bookingResult.ToList().Where(b => 
                (b.StartReserveTime >= bookingQuery.StartReserveTime
                && b.StartReserveTime <= bookingQuery.EndReserveTime)
                || (b.EndReserveTime >= bookingQuery.StartReserveTime
                && b.EndReserveTime <= bookingQuery.EndReserveTime))
                .ToList();
            if (bookingQuery.Type != null)
            {
                rooms = rooms.Where(b => b.Type == bookingQuery.Type).ToList();
            }
            IEnumerable<Guid> bookedRoomsIds = bookings.Select(b => b.RoomId);
            return rooms.Where(room => !bookedRoomsIds.Contains(room.Id));
        }

        [HttpPost]
        [ActionName("roombyguest")]
        public async Task<IEnumerable<Room>> GetBookingsByGuestAsync([FromBody][Required]BookingByGuestQuery bookingQuery)
        {
            IEnumerable<Guest> resultGuests = await UnitOfWork.Guest.GetByIdAsync(bookingQuery.GuestId.Value);
            List<Guest> guests = resultGuests.ToList();
            if (!guests.Any())
            {
                throw new Exception("Provided guest does not exist");
            }
            IEnumerable<RoomBookingByGuest> result = await UnitOfWork.RoomBooking.GetByGuestIdAsync(bookingQuery.GuestId.Value);
            return result.ToList().Where(b =>
                                            (b.StartReserveTime >= bookingQuery.StartReserveTime
                                            && b.StartReserveTime <= bookingQuery.EndReserveTime)
                                            || (b.EndReserveTime >= bookingQuery.StartReserveTime
                                            && b.EndReserveTime <= b.EndReserveTime))
                                            .Select(r => (Room)r)
                                            .ToList();
        }

        [HttpPost]
        [ActionName("booking")]
        public async Task<BookingByGuestByHotelBindingModel> AddBookingAsync([FromBody][Required]BookingByGuestByHotelBindingModel bookingQuery)
        {
            IEnumerable<Guest> resultGuests = await UnitOfWork.Guest.GetByIdAsync(bookingQuery.GuestId.Value);
            List<Guest> guests = resultGuests.ToList();
            if (!guests.Any())
            {
                throw new Exception("Provided guest does not exist");
            }
            IEnumerable<Room> freeRooms = await GetFreeRoomsAsync(bookingQuery);
            var roomToBook = freeRooms.First();
            RoomBookingByGuest booking = new RoomBookingByGuest()
            {
                HotelId = roomToBook.HotelId,
                Type = roomToBook.Type,
                Number = roomToBook.Number,
                GuestId = bookingQuery.GuestId.Value,
                StartReserveTime = bookingQuery.StartReserveTime,
                EndReserveTime = bookingQuery.EndReserveTime
            };
            await UnitOfWork.RoomBooking.AddAsync(booking);
            bookingQuery.BookingId = roomToBook.Id;
            return bookingQuery;

        }

    }
}
