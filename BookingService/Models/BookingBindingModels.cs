using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace BookingService.Models
{
    public abstract class Booking
    {
        [Required]
        public DateTime StartReserveTime { get; set; }
        [Required]
        public DateTime EndReserveTime { get; set; }
    }

    public class BookingByHotel : Booking
    {
        [Required]
        [JsonProperty("hotel_id")]
        public Guid? HotelId { get; set; }
        public string Type { get; set; }
    }

    public class BookingByGuestByHotel : BookingByHotel
    {
        [Required]
        [JsonProperty("guest_id")]
        public Guid? GuestId { get; set; }
    }
}