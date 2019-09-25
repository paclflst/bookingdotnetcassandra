using BookingService.Model.Interface;
using Cassandra.Mapping.Attributes;
using System;

namespace BookingService.Model
{

    [Table("booking.hotel_by_city")]
    public class HotelByCity : IEntity
    {
        public Guid hotel_id { get; set; }
        public string city { get; set; }
        public HotelInfo hotel_info { get; set; }

        public HotelByCity()
        {
        }

        public HotelByCity(Hotel hotel)
        {
            city = hotel.city;
            hotel_id = hotel.id;
            hotel_info = hotel;
        }
    }
}
