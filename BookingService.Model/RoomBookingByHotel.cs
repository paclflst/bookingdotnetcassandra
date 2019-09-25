using Cassandra.Mapping.Attributes;
using System;

namespace BookingService.Model
{
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
}
