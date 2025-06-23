using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEase.Models
{
    public class Venue
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Venue Name is required.")]
        public string Name { get; set; }
        public bool Availability { get; set; }
        public string? VenueDescription { get; set; }

        public string? Location { get; set; }

        public int? VenueCapacity { get; set; }

        [Required(ErrorMessage = "Opening Date is required.")]
        public DateTime OpeningDate { get; set; } = DateTime.Now;
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
