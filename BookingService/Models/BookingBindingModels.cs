using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace BookingService.Models
{
    public abstract class BookingBindingModel
    {
        [Required]
        public DateTime StartReserveTime { get; set; }
        [Required]
        public DateTime EndReserveTime { get; set; }
    }

    public class BookingByHotelBindingModel : BookingBindingModel
    {
        [Required]
        [JsonProperty("hotel_id")]
        public Guid? HotelId { get; set; }
        public string Type { get; set; }
    }

    public class BookingByGuestByHotelBindingModel : BookingByHotelBindingModel
    {
        [JsonProperty("booking_id")]
        public Guid? BookingId { get; set; }
        [Required]
        [JsonProperty("guest_id")]
        public Guid? GuestId { get; set; }
    }

    public class CityBindingModel
    {
        [Required]
        public string City { get; set; }
    }

    public class HotelBindingModel
    {
        [JsonProperty("hotel_id")]
        public Guid? HotelId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }

    public class GuestBindingModel
    {
        [JsonProperty("guest_id")]
        public Guid? GuestId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class RoomBindingModel
    {
        [JsonProperty("room_id")]
        public Guid? RoomId { get; set; }
        [Required]
        [JsonProperty("hotel_id")]
        public Guid HotelId { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string Type { get; set; }
    }
}