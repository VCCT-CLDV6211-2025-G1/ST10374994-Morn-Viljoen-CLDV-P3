using EventEase.Models;
public class Booking
{
    public int Id { get; set; }

    // Foreign Keys
    public int EventId { get; set; }
    public int VenueId { get; set; }

    // Other Properties
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public DateTime BookingDate { get; set; }

    // Navigation Properties
    public Event Event { get; set; }
    public Venue Venue { get; set; } 
}
