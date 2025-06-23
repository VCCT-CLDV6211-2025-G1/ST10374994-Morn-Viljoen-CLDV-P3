using EventEase.Data;
using EventEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EventEase.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Booking
        // Advanced filtering on EventType, date range, and venue availability
        public async Task<IActionResult> Index(int? eventTypeId, DateTime? startDate, DateTime? endDate, bool? availability)
        {
            var bookingsQuery = _context.Bookings
                .Include(b => b.Event).ThenInclude(e => e.EventType)
                .Include(b => b.Event.Venue)
                .AsQueryable();

            // Filter by EventType
            if (eventTypeId.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.Event.EventTypeId == eventTypeId.Value);
            }

            // Filter by Booking Date Range
            if (startDate.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.BookingDate.Date >= startDate.Value.Date);
            }
            if (endDate.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.BookingDate.Date <= endDate.Value.Date);
            }

            // Filter by Venue Availability
            if (availability.HasValue && availability.Value)
            {
                bookingsQuery = bookingsQuery.Where(b => b.Event.Venue.Availability == true);
            }

            var bookings = await bookingsQuery.ToListAsync();

            ViewBag.EventTypes = new SelectList(await _context.EventType.ToListAsync(), "Id", "Name");

            return View(bookings);
        }

        // GET: Booking/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Events = new SelectList(await _context.Events.Include(e => e.Venue).ToListAsync(), "Id", "Name");
            return View();
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerName,CustomerEmail,BookingDate,EventId")] Booking booking)
        {
            // Block past dates
            if (booking.BookingDate.Date < DateTime.Today)
            {
                ModelState.AddModelError("BookingDate", "Booking date cannot be in the past.");
            }

            // Get the selected event + venue
            var selectedEvent = await _context.Events
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(e => e.Id == booking.EventId);

            if (selectedEvent == null)
            {
                ModelState.AddModelError("", "The selected event does not exist.");
            }
            else
            {
                int venueId = selectedEvent.VenueId;
                DateTime date = booking.BookingDate.Date;

                // Check if venue is already booked on this date
                bool alreadyBooked = await _context.Bookings
                    .Include(b => b.Event)
                    .AnyAsync(b => b.Event.VenueId == venueId && b.BookingDate.Date == date);

                if (alreadyBooked)
                {
                    ModelState.AddModelError("", $"The venue '{selectedEvent.Venue.Name}' is already booked on {date:yyyy-MM-dd}.");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Booking successfully created.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Events = new SelectList(await _context.Events.ToListAsync(), "Id", "Name", booking.EventId);
            return View(booking);
        }

        // GET: Booking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            ViewBag.Events = new SelectList(await _context.Events.ToListAsync(), "Id", "Name", booking.EventId);
            return View(booking);
        }

        // POST: Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerName,CustomerEmail,BookingDate,EventId")] Booking booking)
        {
            if (id != booking.Id) return NotFound();

            // Block past dates during edit as well
            if (booking.BookingDate.Date < DateTime.Today)
            {
                ModelState.AddModelError("BookingDate", "Booking date cannot be in the past.");
            }

            // Check venue booking conflicts on edit (excluding current booking)
            var selectedEvent = await _context.Events.Include(e => e.Venue).FirstOrDefaultAsync(e => e.Id == booking.EventId);
            if (selectedEvent == null)
            {
                ModelState.AddModelError("", "The selected event does not exist.");
            }
            else
            {
                int venueId = selectedEvent.VenueId;
                DateTime date = booking.BookingDate.Date;

                bool alreadyBooked = await _context.Bookings
                    .Include(b => b.Event)
                    .AnyAsync(b => b.Event.VenueId == venueId && b.BookingDate.Date == date && b.Id != booking.Id);

                if (alreadyBooked)
                {
                    ModelState.AddModelError("", $"The venue '{selectedEvent.Venue.Name}' is already booked on {date:yyyy-MM-dd}.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Booking updated.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Events = new SelectList(await _context.Events.ToListAsync(), "Id", "Name", booking.EventId);
            return View(booking);
        }

        // GET: Booking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                    .ThenInclude(e => e.Venue)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Booking deleted.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}
