using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Models;

namespace Pustok.Areas.manage.Controllers
{
    [Authorize]
    [Area("manage")]
    public class GenreController : Controller
    {
        private readonly PustokDbContext _context;

        public GenreController(PustokDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var model = _context.Genres
                .Include(x => x.Books)
                .Skip((page - 1) * 10)
                .Take(10).ToList();
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Genre genre)
        {
            if (!ModelState.IsValid)
                return View();


            if (_context.Genres.Any(x => x.Name == genre.Name))
            {
                ModelState.AddModelError("Name", "The Name already taken");
                return View();
            }


            _context.Genres.Add(genre);
            _context.SaveChanges();


            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Genre genre = _context.Genres.Find(id);

            if (genre == null)
                return View("Error");


            return View(genre);
        }
        [HttpPost]
        public IActionResult Edit(Genre genre)
        {
            if (!ModelState.IsValid)
                return View();


            Genre existgenre = _context.Genres.Find(genre.Id);

            if (existgenre == null)
                return View("Error");


            if (genre.Name != existgenre.Name && _context.Genres.Any(x => x.Id != genre.Id && x.Name == genre.Name))
            {
                ModelState.AddModelError("Name", "The Name already taken");
                return View();
            }


            existgenre.Name = genre.Name;

            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Genre genre = _context.Genres.Find(id);


            if (genre == null)
                return View("Error");


            return View(genre);
        }

        [HttpPost]
        public IActionResult Delete(Genre genre)
        {

            Genre existgenre = _context.Genres.Find(genre.Id);

            if (existgenre == null)
                return View("Error");


            _context.Genres.Remove(existgenre);
            _context.SaveChanges();


            return RedirectToAction("index");
        }
    }
}
