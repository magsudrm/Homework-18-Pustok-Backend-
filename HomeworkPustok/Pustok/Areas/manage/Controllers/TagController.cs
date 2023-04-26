using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Models;

namespace Pustok.Areas.manage.Controllers
{
    [Authorize]
    [Area("manage")]
    public class TagController : Controller
    {
        private readonly PustokDbContext _context;

        public TagController(PustokDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var model = _context.Tags
                .Include(x => x.BookTags)
                .Skip((page - 1) * 10)
                .Take(10).ToList();
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Tag tag)
        {
            if (!ModelState.IsValid)
                return View();


            if (_context.Tags.Any(x => x.Name == tag.Name))
            {
                ModelState.AddModelError("TagName", "The TagName already taken");
                return View();
            }


            _context.Tags.Add(tag);
            _context.SaveChanges();


            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Tag tag = _context.Tags.Find(id);

            if (tag == null)
                return View("Error");


            return View(tag);
        }
        [HttpPost]
        public IActionResult Edit(Tag tag)
        {
            if (!ModelState.IsValid)
                return View();


            Tag existTag = _context.Tags.Find(tag.Id);

            if (existTag == null)
                return View("Error");


            if (tag.Name != existTag.Name && _context.Tags.Any(x => x.Id != tag.Id && x.Name == tag.Name))
            {
                ModelState.AddModelError("TagName", "The TagName already taken");
                return View();
            }


            existTag.Name = tag.Name;
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Tag tag = _context.Tags.Find(id);


            if (tag == null)
                return View("Error");


            return View(tag);
        }

        [HttpPost]
        public IActionResult Delete(Tag tag)
        {

            Tag existTag = _context.Tags.Find(tag.Id);

            if (existTag == null)
                return View("Error");


            _context.Tags.Remove(existTag);
            _context.SaveChanges();


            return RedirectToAction("index");
        }
    }
}
