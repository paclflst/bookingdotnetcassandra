using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Cassandra.Mapping.Attributes;
using Newtonsoft.Json;
using BookingService.Helper_Code.Inherit;

namespace BookingService.Models
{
    [Table("booking.hotel_by_cityOLD")]
    public class HotelModels
    {
        [Column("city")]
        public string City { get; set; }
        [Column("hotel")]
        public string Hotel { get; set; }
    }

    public class HotelInfo
    {
        [Required]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }

    [Table("booking.hotel")]
    public class Hotel : HotelInfo
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Hotel()
        {
        }

        public Hotel(HotelInfo hotelInfo)
        {
            this.FillProperties(hotelInfo);
        }
    }

    [Table("booking.hotel_by_city")]
    public class HotelByCity
    {
        public string City { get; set; }
        [Column("hotel_id")]
        public Guid HotelId { get; set; }
        [Column("hotel_info")]
        public HotelInfo HotelInfo { get; set; }

        public HotelByCity(Hotel hotel)
        {
            City = hotel.City;
            HotelId = hotel.Id;
            HotelInfo = hotel;
        }
    }

    public class GuestInfo
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    [Table("booking.guest_by_email")]
    public class Guest : GuestInfo
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guest()
        {
        }

        public Guest(GuestInfo guestInfo)
        {
            this.FillProperties(guestInfo);
        }
    }

    public class RoomInfo
    {
        //[Required]
        [Column("hotel_id")]
        [JsonProperty("hotel_id")]
        public Guid HotelId { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string Type { get; set; }
    }

    [Table("booking.room_by_hotelid")]
    public class Room : RoomInfo
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Room()
        {
        }

        public Room(RoomInfo roomInfo)
        {
            this.FillProperties(roomInfo);
        }
    }

    [Table("booking.roombooking_by_hotelid_roomid")]
    public class RoomBookingByHotel : Room
    {
        public new Guid Id { get; set; } = Guid.NewGuid();
        [Column("room_id")]
        public Guid RoomId { get { return base.Id; } set { base.Id = value; } }
        [Column("start_reserverve_time")]
        public DateTime StartReserveTime { get; set; }
        [Column("end_reserverve_time")]
        public DateTime EndReserveTime { get; set; }
    }

    [Table("booking.roombooking_by_guestid")]
    public class RoomBookingByGuest : RoomBookingByHotel
    {
        [Column("guest_id")]
        public Guid GuestId { get; set; }

        public RoomBookingByGuest()
        {
        }

        public RoomBookingByGuest(Guid? guestId, Room room, DateTime startReserveTime, DateTime endReserveTime)
        {
            GuestId = guestId.Value;
            RoomId = room.Id;
            HotelId = room.HotelId;
            StartReserveTime = startReserveTime;
            EndReserveTime = endReserveTime;
        }
    }

    public class City
    {
        [Required]
        [JsonProperty("city")]
        public string Name { get; set; }
    }

    public class BookingByHotelQuery
    {
        [Required]
        [JsonProperty("hotel_id")]
        public Guid? HotelId { get; set; }
        [Required]
        public DateTime StartReserveTime { get; set; }
        [Required]
        public DateTime EndReserveTime { get; set; }
        public string Type { get; set; }
    }

    public class BookingByGuestQuery
    {
        [Required]
        [JsonProperty("guest_id")]
        public Guid? GuestId { get; set; }
        [Required]
        public DateTime StartReserveTime { get; set; }
        [Required]
        public DateTime EndReserveTime { get; set; }
    }
}