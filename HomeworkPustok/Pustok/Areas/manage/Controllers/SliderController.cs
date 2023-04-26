using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pustok.DAL;
using Pustok.Helpers;
using Pustok.Models;

namespace Pustok.Areas.manage.Controllers
{
    [Authorize]
    [Area ("manage")]
    public class SliderController : Controller
    {
        private readonly PustokDbContext _context;
        private readonly IWebHostEnvironment _env;

		public SliderController(PustokDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

		}
        public IActionResult Index()
        {
            var data = _context.Sliders.OrderBy(x => x.Order).ToList();
            return View(data);
        }

        public IActionResult Create()
        {
            var order = _context.Sliders.Max(x => x.Order);
            ViewBag.Order = order + 1;
            return View();
        }
        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (slider.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "ImageFile is required");
                return View();
            }

            if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
                ModelState.AddModelError("ImageFile", "ImageFile must be image/png or image/jpeg");

            if (slider.ImageFile.Length > 2097152)
                ModelState.AddModelError("ImageFile", "ImageFile must be less or equal than 2MB");

            slider.Image = FileManager.Save(slider.ImageFile, _env.WebRootPath + "/Uploads/Sliders");

            if (!ModelState.IsValid) return View();

            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Edit( int id)
        {
            Slider slider = _context.Sliders.Find(id);

            if (slider == null)
                return View("Error");

            return View(slider);
        }
        [HttpPost]
        public IActionResult Edit(Slider slider)
        {
            if (!ModelState.IsValid) return View();

            Slider existSlider = _context.Sliders
                .Find(slider.Id);

            if (existSlider == null)
                return View("Error");

            existSlider.Title1 = slider.Title1;
            existSlider.Title2 = slider.Title2;
            existSlider.Order = slider.Order;
            existSlider.ButtonText = slider.ButtonText;
            existSlider.ButtonUrl = slider.ButtonUrl;
            existSlider.Description = slider.Description;

            if (slider.ImageFile != null)
            {
                string oldFileName = existSlider.Image;
                existSlider.Image = FileManager.Save(slider.ImageFile, _env.WebRootPath + "/Uploads/Sliders");
                FileManager.Delete(_env.WebRootPath + "/Uploads/Sliders", oldFileName);
            }

            _context.SaveChanges();

            return RedirectToAction("index");
        }


		public IActionResult Delete(int id)
		{
            Slider existSlider = _context.Sliders.Find(id);

            if (existSlider == null)
                return NotFound();

            FileManager.Delete(_env.WebRootPath + "/Uploads/Sliders", existSlider.Image);

            _context.Sliders.Remove(existSlider);
            _context.SaveChanges();

            return Ok();
        }

	}
}
