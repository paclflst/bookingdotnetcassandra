using BookingService.Model.Interface;
using Cassandra.Mapping.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Model
{
    [Table("booking.room_by_hotelid")]
    public class Room : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Column("hotel_id")]
        public Guid HotelId { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
    }
}
