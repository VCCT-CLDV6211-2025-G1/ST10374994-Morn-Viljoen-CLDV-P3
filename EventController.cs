using EventEase.Data;
using EventEase.Models;
using EventEase.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class EventController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly BlobService _blobService;

    public EventController(ApplicationDbContext context, BlobService blobService)
    {
        _context = context;
        _blobService = blobService;
    }

    // GET: Event
    public async Task<IActionResult> Index()
    {
        var events = await _context.Events.Include(e => e.Venue).Include(e => e.EventType).ToListAsync();
        return View(events);
    }

    // GET: Event/Create
    public IActionResult Create()
    {
        ViewBag.EventTypes = new SelectList(_context.EventType.ToList(), "Id", "Name");
        ViewBag.VenueId = new SelectList(_context.Venues.ToList(), "Id", "Name");
        return View();
    }

    // POST: Event/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Event eventItem)
    {
        if (eventItem.Date < DateTime.Today)
        {
            ModelState.AddModelError("Date", "Event date cannot be in the past.");
        }

        if (ModelState.IsValid)
        {
            if (eventItem.ImageFile != null)
            {
                eventItem.ImageUrl = await _blobService.UploadFileAsync(eventItem.ImageFile);
            }

            _context.Add(eventItem);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Event created successfully.";
            return RedirectToAction(nameof(Index));
        }

        
        ViewBag.EventTypes = new SelectList(_context.EventType.ToList(), "Id", "Name", eventItem.EventTypeId);
        ViewBag.VenueId = new SelectList(_context.Venues.ToList(), "Id", "Name", eventItem.VenueId);
        return View(eventItem);
    }
}
