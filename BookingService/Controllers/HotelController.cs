using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using BookingService.Models;
using BookingService.DAO;
using System.ComponentModel.DataAnnotations;

namespace BookingService.Controllers
{
    //[Authorize]
    public class HotelController : ApiController
    {
        private static IHotelDAO dao;

        protected IHotelDAO hotelDAO
        {
            get
            {
                if (dao == null)
                {
                    dao = new HotelDAO();
                }
                return dao;
            }
        }

        [HttpGet]
        public async Task<string> Index()
        {
            IEnumerable<HotelModels> hotels = await hotelDAO.GetHotelByCityOLD("Lviv");
            return "OK";
        }

        [HttpPost]
        [ActionName("Index")]
        public async Task<IEnumerable<HotelModels>> IndexPost([FromBody]CityBindingModel city)
        {
            IEnumerable<HotelModels> hotels = await hotelDAO.GetHotelByCityOLD(city.City);
            return hotels;
        }

        [HttpPost]
        [ActionName("getcity")]
        public async Task<IEnumerable<Hotel>> GetHotelByCity([FromBody]CityBindingModel city)
        {
            IEnumerable<Hotel> hotels = await hotelDAO.GetHotelByCityAsync(city.City);
            return hotels;
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IEnumerable<Hotel>> AddHotel([FromBody][Required]HotelBindingModel hotel)
        {
            HotelInfo hotelInfo = new HotelInfo()
            {
                Name = hotel.Name,
                Phone = hotel.Phone,
                Address = hotel.Address,
                City = hotel.City,
                State = hotel.State,
                Zip = hotel.Zip,
            };
            return await hotelDAO.AddHotelAsync(hotelInfo);
        }

        [HttpPost]
        [ActionName("AddGuest")]
        public async Task AddGuest([FromBody][Required]GuestBindingModel guest)
        {
            GuestInfo guestInfo = new GuestInfo()
            {
                Name = guest.Email,
                Email = guest.Email
            };
            await hotelDAO.AddGuestAsync(guestInfo);
        }

        [HttpPost]
        [ActionName("AddRoom")]
        public async Task AddRoom([FromBody][Required]RoomInfo room)
        {
            await hotelDAO.AddRoomAsync(room);
        }

        [HttpPost]
        [ActionName("GetFreeRooms")]
        public async Task<IEnumerable<Room>> GetFreeRooms([FromBody][Required]BookingByHotelBindingModel bookingQuery)
        {
            return await hotelDAO.GetFreeRoomByHotelIdByBookingPeriodAsync(bookingQuery);
        }

        [HttpPost]
        [ActionName("GetGuestBookings")]
        public async Task<IEnumerable<Room>> GetBookingsByGuest([FromBody][Required]BookingByGuestQuery bookingQuery)
        {
            return await hotelDAO.GetBookingByGuestAsync(bookingQuery);
        }

        [HttpPost]
        [ActionName("AddBooking")]
        public async Task AddBookingAsync([FromBody][Required]BookingByGuestByHotelBindingModel bookingQuery)
        {
            await hotelDAO.AddBookingAsync(bookingQuery);
        }
    }
}