using BookingService.Model.Helper_Code.Inherit;
using BookingService.Model.Interface;
using Cassandra.Mapping.Attributes;
using System;

namespace BookingService.Model
{

    [Table("booking.guest")]
    public class Guest : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
