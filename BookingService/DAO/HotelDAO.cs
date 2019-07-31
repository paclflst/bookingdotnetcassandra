using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Mapping;
using BookingService.Models;
using Cassandra.Data.Linq;

namespace BookingService.DAO
{
    public interface IHotelDAO
    {
        Task<IEnumerable<HotelModels>> GetHotel();
        Task<IEnumerable<HotelModels>> GetHotelByCityOLD(string city);
        Task<IEnumerable<Hotel>> GetHotelByCityAsync(string city);
        Task<IEnumerable<Hotel>> AddHotelAsync(HotelInfo hotel);
        Task AddGuestAsync(GuestInfo guestInfo);
        Task AddRoomAsync(RoomInfo roomInfo);
        Task<IEnumerable<Room>> GetFreeRoomByHotelIdByBookingPeriodAsync(BookingByHotelBindingModel bookingQuery);
        Task<IEnumerable<Room>> GetBookingByGuestAsync(BookingByGuestQuery bookingQuery);
        Task AddBookingAsync(BookingByGuestByHotelBindingModel bookingQuery);
    }

    public class HotelDAO : IHotelDAO
    {
        protected readonly ISession session;
        protected readonly IMapper mapper;

        public HotelDAO()
        {
            ICassandraDAO cassandraDAO = new CassandraDAO();
            session = cassandraDAO.GetSession();
            session.UserDefinedTypes.Define(
                UdtMap.For<HotelInfo>("hotel_info", "booking"),
                UdtMap.For<Hotel>("hotel_info", "booking")
            );
            mapper = new Mapper(session);
            //MappingConfiguration.Global.Define(new Map<RoomBooking>()
            //                                    .Column(b => b.HotelId, cm => cm.WithName("hotel_id"))
            //                                    .Column(b => b.RoomId, cm => cm.WithName("room_id"))
            //                                    .Column(b => b.StartReserveTime, cm => cm.WithName("start_reserverve_time"))
            //                                    .Column(b => b.EndReserveTime, cm => cm.WithName("end_reserverve_time")));
        }

        public async Task<IEnumerable<HotelModels>> GetHotel()
        {
            return await mapper.FetchAsync<HotelModels>();
        }

        public async Task<IEnumerable<HotelModels>> GetHotelByCityOLD(string city)
        {
            return await mapper.FetchAsync<HotelModels>("SELECT * FROM booking.hotel_by_cityOLD WHERE city = ?", city);
        }

        public async Task<IEnumerable<Hotel>> GetHotelByCityAsync(string city)
        {
            return await mapper.FetchAsync<Hotel>(@"SELECT 
                    hotel_id as id, 
                    hotel_info.name as name, 
                    hotel_info.phone as phone, 
                    hotel_info.address as address, 
                    city, 
                    hotel_info.state as state, 
                    hotel_info.zip as zip 
                FROM booking.hotel_by_city WHERE city = ?;", city);
        }

        public async Task<IEnumerable<Hotel>> AddHotelAsync(HotelInfo hotelInfo)
        {
            IEnumerable<Hotel> result = await GetHotelByCityAsync(hotelInfo.City);
            List<Hotel> hotels = result.ToList();
            if (hotels.Any(h => h.Name == hotelInfo.Name))
            {
                return hotels.Where(h => h.Name == hotelInfo.Name);
            }

            Hotel newHotel = new Hotel(hotelInfo);
            Task.WaitAll(mapper.InsertAsync(newHotel), mapper.InsertAsync(new HotelByCity(newHotel)));
            return hotels;
        }

        public async Task<IEnumerable<Guest>> GetGuestByEmailAsync(string email)
        {
            return await mapper.FetchAsync<Guest>("WHERE email = ?;", email);
        }

        public async Task<IEnumerable<Guest>> GetGuestByIdAsync(Guid? id)
        {
            return await mapper.FetchAsync<Guest>("SELECT * FROM booking.guest WHERE id = ?;", id.Value);
        }

        public async Task AddGuestAsync(GuestInfo guestInfo)
        {
            IEnumerable<Guest> result = await GetGuestByEmailAsync(guestInfo.Email);
            List<Guest> users = result.ToList();
            if (users.Any())
            {
                return;
            }
            var newGuest = new Guest(guestInfo);
            await mapper.InsertAsync(newGuest);
        }

        public async Task<IEnumerable<Hotel>> GetHotelAsync(Guid hotelId)
        {
            return await mapper.FetchAsync<Hotel>("WHERE id = ?;", hotelId);
        }

        public async Task<IEnumerable<Room>> GetRoomByHotelIdAsync(Guid? hotelId)
        {
            return await mapper.FetchAsync<Room>("WHERE hotel_id = ?", hotelId);
        }

        public async Task<IEnumerable<Room>> GetRoomByHotelIdByRoomNumberAsync(Guid hotelId, string roomNumber)
        {
            return await mapper.FetchAsync<Room>(@"SELECT hotel_id,
                id, 
                number
                FROM booking.room_by_hotelid
                WHERE hotel_id = ? AND number = ?;", hotelId, roomNumber);
        }

        public async Task<IEnumerable<RoomBookingByHotel>> GetRoomBookingByHotelIdByRoomIdAsync(Guid hotelId, IEnumerable<Guid> roomIds)
        {
            return await mapper.FetchAsync<RoomBookingByHotel>(@"
                SELECT hotel_id, 
                    room_id, 
                    id, 
                    number,
                    type,
                    start_reserverve_time, 
                    end_reserverve_time
                FROM booking.roombooking_by_hotelid_roomid
                WHERE hotel_id = ?
                    AND room_id IN ?", hotelId, roomIds);
        }

        public async Task<IEnumerable<RoomBookingByHotel>> GetRoomBookingByHotelIdAsync(Guid? hotelId)
        {
            return await mapper.FetchAsync<RoomBookingByHotel>("WHERE hotel_id = ?", hotelId);
        }

        public async Task<IEnumerable<RoomBookingByHotel>> GetRoomBookingByHotelIdByRoomIdAsync(Guid hotelId, Guid roomId)
        {
            return await GetRoomBookingByHotelIdByRoomIdAsync(hotelId, new List<Guid>() { roomId });
        }

        public async Task AddRoomAsync(RoomInfo roomInfo)
        {
            IEnumerable<Hotel> hotelResult = await GetHotelAsync(roomInfo.HotelId);
            List<Hotel> hotels = hotelResult.ToList();
            if (hotels.Any())
            {
                IEnumerable<Room> roomResult = await GetRoomByHotelIdByRoomNumberAsync(roomInfo.HotelId, roomInfo.Number);
                List<Room> rooms = roomResult.ToList();
                if (rooms.Any())
                {
                    return;
                }
                await mapper.InsertAsync(new Room(roomInfo));
            }
            else
            {
                return;
            }
        }

        public async Task<IEnumerable<Room>> GetFreeRoomByHotelIdByBookingPeriodAsync(BookingByHotelBindingModel bookingQuery)
        {
            Task<IEnumerable<Room>> getRooms = GetRoomByHotelIdAsync(bookingQuery.HotelId);
            Task<IEnumerable<RoomBookingByHotel>> getBookings = GetRoomBookingByHotelIdAsync(bookingQuery.HotelId);
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
                bookings = bookings.Where(b => b.Type == bookingQuery.Type).ToList();
            }
            IEnumerable<Guid> bookedRoomsIds = bookings.Select(b => b.RoomId);
            return rooms.Where(room => !bookedRoomsIds.Contains(room.Id));
        }

        public async Task<IEnumerable<Room>> GetBookingByGuestAsync(BookingByGuestQuery bookingQuery)
        {
            IEnumerable<RoomBookingByGuest> result = await mapper.FetchAsync<RoomBookingByGuest>("WHERE guest_id = ?", bookingQuery.GuestId);
            return result.ToList().Where(b =>
                                            (b.StartReserveTime >= bookingQuery.StartReserveTime
                                            && b.StartReserveTime <= bookingQuery.EndReserveTime)
                                            || (b.EndReserveTime >= bookingQuery.StartReserveTime
                                            && b.EndReserveTime <= b.EndReserveTime))
                                            .Select(r => new Room(r))
                                            .ToList();
        }

        public async Task AddBookingAsync(BookingByGuestByHotelBindingModel bookingQuery)
        {
            var result = await GetGuestByIdAsync(bookingQuery.GuestId);
            if (result.Count() < 1)
            {
                throw new Exception("Guest does not exist");
            }
            IEnumerable<Room> freeRooms = await GetFreeRoomByHotelIdByBookingPeriodAsync(bookingQuery);
            RoomBookingByGuest roomToBook = new RoomBookingByGuest(bookingQuery.GuestId, freeRooms.First(), bookingQuery.StartReserveTime, bookingQuery.EndReserveTime);
            Task.WaitAll(mapper.InsertAsync((RoomBookingByHotel)roomToBook), mapper.InsertAsync(roomToBook));
        }

    }
}