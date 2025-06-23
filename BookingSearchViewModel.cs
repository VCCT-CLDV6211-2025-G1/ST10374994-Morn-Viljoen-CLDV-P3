using System;
using System.Collections.Generic;

namespace EventEase.Models
{
    public class BookingSearchViewModel
    {
        public List<Booking> Bookings { get; set; } = new List<Booking>();

        public int? EventTypeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Availability { get; set; }

        public List<EventType> EventType { get; set; } = new List<EventType>();
    }
}
