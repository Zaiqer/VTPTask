using ButaGroupTask.DAL;
using ButaGroupTask.Helpers;
using ButaGroupTask.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ButaGroupTask.Controllers
{
    public class MembersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public MembersController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Member> members = await _db.Members.Include(x => x.Degree).Include(x => x.Gender).Where(x => !x.IsDeactive).ToListAsync();
            return View(members);
        }

        #region Create
        // GET: Members/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Gender = await _db.Gender.ToListAsync();
            ViewBag.Degrees = await _db.Degrees.ToListAsync();
            return View();
        }
        // POST: Members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member member, int? genId, int? degId)
        {
            ViewBag.Gender = await _db.Gender.ToListAsync();
            ViewBag.Degrees = await _db.Degrees.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (genId == null)
            {
                ModelState.AddModelError("Age", "Zəhmət olmasa iştirakçı üçün bir cins seçin.");
                return View();
            }
            Gender gender = await _db.Gender.FirstOrDefaultAsync(x => x.Id == genId);
            if (gender == null)
            {
                ModelState.AddModelError("Age", "Gender Error");
                return View();
            }
            if (degId == null)
            {
                ModelState.AddModelError("Age", "Zəhmət olmasa iştirakçı üçün bir təhsil səviyyəsi seçin.");
                return View();
            }
            Degree degree = await _db.Degrees.FirstOrDefaultAsync(x => x.Id == degId);
            if (degree == null)
            {
                ModelState.AddModelError("Age", "Degree Error");
                return View();
            }
            bool isExist = await _db.Members.AnyAsync(x => x.Phone == member.Phone && x.Mail == member.Mail);
            if (isExist)
            {
                ModelState.AddModelError("Phone", "Bu nömrə artıq istifadə olunub!");
                return View();
            }
            if (isExist)
            {
                ModelState.AddModelError("Mail", "Bu mail adresi artıq istifadə olunub!");
                return View();
            }
            if (member.Photo == null)
            {
                ModelState.AddModelError("Photo", "Şəkillər sütunu boş ola bilməz. Zəhmət olmasa bir şəkil faylı seçin!");
                return View();
            }
            if (!member.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil faylı seçin!");
                return View();
            }
            if (member.Photo.OlderFiveMb())
            {
                ModelState.AddModelError("Photo", "Bu şəkil 5MB-dan çoxdur. Zəhmət olmasa daha kiçik ölçülü fayl seçin!");
                return View();
            }
            string path = Path.Combine(_env.WebRootPath, "assets", "images", "members");
            member.Image = await member.Photo.SaveFileAsync(path);
            member.GenderId = (int)genId;
            member.DegreeId = (int)degId;
            await _db.Members.AddAsync(member);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Details
        //GET: Members/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Member dbMember = await _db.Members.Include(x => x.Degree).Include(x => x.Gender).FirstOrDefaultAsync(x => x.Id == id);
            if (dbMember == null)
            {
                return BadRequest();
            }
            return View(dbMember);
        }
        #endregion

        // GET: Members/Edit
        #region Edit
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Gender = await _db.Gender.ToListAsync();
            ViewBag.Degrees = await _db.Degrees.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            Member dbMember = await _db.Members.Include(x => x.Degree).Include(x => x.Gender).FirstOrDefaultAsync(x => x.Id == id);
            if (dbMember == null)
            {
                return BadRequest();
            }
            return View(dbMember);
        }

        // POST: Member/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Member member, int? genId, int? degId)
        {
            ViewBag.Gender = await _db.Gender.ToListAsync();
            ViewBag.Degrees = await _db.Degrees.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            Member dbMember = await _db.Members.Include(x => x.Degree).Include(x => x.Gender).FirstOrDefaultAsync(x => x.Id == id);
            if (dbMember == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(dbMember);
            }
            if (genId == null)
            {
                ModelState.AddModelError("GenderId", "Gender seçimi tələb olunur!");
                return View(dbMember);
            }
            Gender dbGender = await _db.Gender.FirstOrDefaultAsync(x => x.Id == genId);
            if (dbGender == null)
            {
                ModelState.AddModelError("GenderId", "Gender seçimi tələb olunur!");
                return View(dbMember);
            }
            if (degId == null)
            {
                ModelState.AddModelError("DegreeId", "Degree seçimi tələb olunur!");
                return View(dbMember);
            }
            Degree dbDegree = await _db.Degrees.FirstOrDefaultAsync(x => x.Id == degId);
            if (dbDegree == null)
            {
                ModelState.AddModelError("DegreeId", "Degree seçimi tələb olunur!");
                return View(dbMember);
            }
            if (member.Photo != null)
            {
                if (!member.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil faylı seçin!");
                    return View(dbMember);
                }
                if (member.Photo.OlderFiveMb())
                {
                    ModelState.AddModelError("Photo", "Photo more than 5 MB!");
                    return View(dbMember);
                }
                string path = Path.Combine(_env.WebRootPath, "assets", "images", "members");
                if (System.IO.File.Exists(Path.Combine(path, dbMember.Image)))
                {
                    System.IO.File.Delete(Path.Combine(path, dbMember.Image));
                }
                dbMember.Image = await member.Photo.SaveFileAsync(path);
            }
            dbMember.Name = member.Name;
            dbMember.Surname = member.Surname;
            dbMember.Mail = member.Mail;
            dbMember.Phone = member.Phone;
            dbMember.University = member.University;
            dbMember.Specialty = member.Specialty;
            dbMember.DegreeId = (int)degId;
            dbMember.Age = member.Age;
            dbMember.Year = member.Year;
            dbMember.GenderId = (int)genId;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        // GET: Members/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Member dbMember = await _db.Members.FirstOrDefaultAsync(x => x.Id == id);
            if (dbMember == null)
            {
                return BadRequest();
            }
            return View(dbMember);
        }

        // POST: Members/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Member dbMember = await _db.Members.FirstOrDefaultAsync(x => x.Id == id);
            if (dbMember == null)
            {
                return BadRequest();
            }
            dbMember.IsDeactive = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
