using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEase.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Event Type.")]
        public int EventTypeId { get; set; }

        public EventType EventType { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Venue.")]
        public int VenueId { get; set; }

        public Venue Venue { get; set; }

        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
