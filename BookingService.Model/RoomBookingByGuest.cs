using Cassandra.Mapping.Attributes;
using System;

namespace BookingService.Model
{
    [Table("booking.roombooking_by_guestid")]
    public class RoomBookingByGuest : RoomBookingByHotel
    {
        [Column("guest_id")]
        public Guid GuestId { get; set; }

        public RoomBookingByGuest()
        {
        }
    }
}
