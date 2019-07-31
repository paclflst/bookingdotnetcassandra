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
        public async Task<IEnumerable<HotelModels>> IndexPost([FromBody]City city)
        {
            IEnumerable<HotelModels> hotels = await hotelDAO.GetHotelByCityOLD(city.Name);
            return hotels;
        }

        [HttpPost]
        [ActionName("City")]
        public async Task<IEnumerable<Hotel>> GetHotelByCity([FromBody]City city)
        {
            IEnumerable<Hotel> hotels = await hotelDAO.GetHotelByCity(city.Name);
            return hotels;
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IEnumerable<Hotel>> AddHotel([FromBody][Required]HotelInfo hotel)
        {
            return await hotelDAO.AddHotel(hotel);
        }

        [HttpPost]
        [ActionName("AddGuest")]
        public async Task AddGuest([FromBody][Required]GuestInfo guest)
        {
            await hotelDAO.AddGuestAsync(guest);
        }

        [HttpPost]
        [ActionName("AddRoom")]
        public async Task AddRoom([FromBody][Required]RoomInfo room)
        {
            await hotelDAO.AddRoomAsync(room);
        }

        [HttpPost]
        [ActionName("GetFreeRooms")]
        public async Task<IEnumerable<Room>> GetFreeRooms([FromBody][Required]BookingByHotel bookingQuery)
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
        public async Task AddBookingAsync([FromBody][Required]BookingByGuestByHotel bookingQuery)
        {
            await hotelDAO.AddBookingAsync(bookingQuery);
        }
    }
}