using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEase.Data;
using EventEase.Models;
using EventEase.Services;
using System.Threading.Tasks;
using System.Linq;

namespace EventEase.Controllers
{
    public class VenueController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BlobService _blobService;

        public VenueController(ApplicationDbContext context, BlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        // GET: Venue
        public async Task<IActionResult> Index()
        {
            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }

            return View(await _context.Venues.ToListAsync());
        }

        // GET: Venue/Create
        public IActionResult Create()
        {
            var venue = new Venue
            {
                Availability = true // default to available
            };
            return View(venue);
        }

        // POST: Venue/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venue venue)
        {
            if (ModelState.IsValid)
            {
                if (venue.ImageFile != null)
                {
                    venue.ImageUrl = await _blobService.UploadFileAsync(venue.ImageFile);
                }

                _context.Venues.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(venue);
        }

        // GET: Venue/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venues.FindAsync(id);
            if (venue == null) return NotFound();

            return View(venue);
        }

        // POST: Venue/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Venue venue)
        {
            if (id != venue.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (venue.ImageFile != null)
                    {
                        if (!string.IsNullOrEmpty(venue.ImageUrl))
                        {
                            await _blobService.DeleteFileAsync(venue.ImageUrl);
                        }

                        venue.ImageUrl = await _blobService.UploadFileAsync(venue.ImageFile);
                    }

                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.Id)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(venue);
        }

        // GET: Venue/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venues.FirstOrDefaultAsync(m => m.Id == id);
            if (venue == null) return NotFound();

            return View(venue);
        }

        // POST: Venue/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null) return NotFound();

            bool hasBookings = await _context.Bookings.AnyAsync(b => b.VenueId == id);
            if (hasBookings)
            {
                TempData["Error"] = "This venue is linked to one or more bookings and cannot be deleted.";
                return RedirectToAction(nameof(Index));
            }

            if (!string.IsNullOrEmpty(venue.ImageUrl))
                await _blobService.DeleteFileAsync(venue.ImageUrl);

            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VenueExists(int id)
        {
            return _context.Venues.Any(e => e.Id == id);
        }
    }
}
