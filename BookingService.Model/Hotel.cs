using Cassandra.Mapping.Attributes;
using System;
using BookingService.Model.Helper_Code.Inherit;
using BookingService.Model.Interface;

namespace BookingService.Model
{
    public class HotelInfo : IEntity
    {
        [Column("name")]
        public string name { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
    }

    [Table("booking.hotel")]
    public class Hotel : HotelInfo
    {
        public Guid id { get; set; } = Guid.NewGuid();

        public Hotel()
        {
        }

        public Hotel(HotelInfo hotelInfo)
        {
            this.FillProperties(hotelInfo);
        }
    }
}
