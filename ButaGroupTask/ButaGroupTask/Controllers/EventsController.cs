using ButaGroupTask.DAL;
using ButaGroupTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ButaGroupTask.Controllers
{
    public class EventsController : Controller
    {
        private readonly AppDbContext _db;
        public EventsController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Event> events = await _db.Events.Where(x => !x.IsDeactive).ToListAsync();
            return View(events);
        }

        #region Create
        // GET: Events/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }
        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event events)
        {
            
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _db.Events.AddAsync(events);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        // GET: Events/Edit
        #region Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Event dbEvent = await _db.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (dbEvent == null)
            {
                return BadRequest();
            }
            return View(dbEvent);
        }

        // POST: Events/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Event events)
        {
            if (id == null)
            {
                return NotFound();
            }
            Event dbEvent = await _db.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (dbEvent == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(dbEvent);
            }
            dbEvent.Name = events.Name;
            dbEvent.Place = events.Place;
            dbEvent.Date = events.Date;
            dbEvent.Duration = events.Duration;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Details
        //GET: Events/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Event dbEvent = await _db.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (dbEvent == null)
            {
                return BadRequest();
            }
            return View(dbEvent);
        }
        #endregion

        #region Delete
        // GET: Events/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Event dbEvent = await _db.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (dbEvent == null)
            {
                return BadRequest();
            }
            return View(dbEvent);
        }

        // POST: Events/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Event dbEvent = await _db.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (dbEvent == null)
            {
                return BadRequest();
            }
            dbEvent.IsDeactive = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
