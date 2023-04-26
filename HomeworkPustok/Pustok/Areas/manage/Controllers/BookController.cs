using System.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Helpers;
using Pustok.Models;
using Pustok.ViewModels;

namespace Pustok.Areas.manage.Controllers
{
    [Authorize]
    [Area("manage")]

    public class BookController : Controller
    {
        private readonly PustokDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BookController(PustokDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1, int size = 4)
        {
            var query = _context.Books
            .Include(x => x.BookImages)
            .Include(x => x.Genre)
            .Where(x => !x.IsDeleted);
            return View(PaginatedList<Book>.Create(query, page, size));
        }
        public IActionResult Create()
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            CheckCreate(book);
            if (!ModelState.IsValid)
            {
                ViewBag.Authors = _context.Authors.ToList();
                ViewBag.Genres = _context.Genres.ToList();
                ViewBag.Tags = _context.Tags.ToList();
                return View();
            }

            book.BookTags = book.TagIds.Select(x => new BookTag { TagId = x }).ToList();
            AddBookImages(book);
            book.CreatedAt = DateTime.UtcNow;

            _context.Books.Add(book);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        private void CheckCreate(Book book)
        {
            if (!_context.Authors.Any(x => x.Id == book.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Auhtor not found");
                return;
            }
            if (!_context.Genres.Any(x => x.Id == book.GenreId))
            {
                ModelState.AddModelError("GenreId", "Genre not found");
                return;
            }
            if (book.PosterFile == null)
                ModelState.AddModelError("PosterFile", "PosterFile is required");
            if (book.HoverPosterFile == null)
                ModelState.AddModelError("HoverPosterFile", "HoverPosterFile is required");

            foreach (var tagId in book.TagIds)
            {
                if (!_context.Tags.Any(x => x.Id == tagId))
                {
                    ModelState.AddModelError("TagIds", "Tag not found");
                    break;
                }
            }
        }
        private void AddBookImages(Book book)
        {
            BookImage posterBi = new BookImage
            {
                Image = FileManager.Save(book.PosterFile, _env.WebRootPath + "/uploads/books"),
                PosterStatus = true,
            };

            BookImage hoverPosterBi = new BookImage
            {
                Image = FileManager.Save(book.HoverPosterFile, _env.WebRootPath + "/uploads/books"),
                PosterStatus = false,
            };
            book.BookImages.Add(posterBi);
            book.BookImages.Add(hoverPosterBi);

            foreach (var biFile in book.ImageFiles)
            {
                BookImage bi = new BookImage
                {
                    Image = FileManager.Save(biFile, _env.WebRootPath + "/uploads/books"),
                    PosterStatus = null,
                };
                book.BookImages.Add(bi);
            }
        }


        public IActionResult Edit(int id)
        {
            Book book = _context.Books
                .Include(x => x.BookTags)
                .Include(x => x.BookImages)
                .FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (book == null)
                return View("Error");
            book.TagIds = book.BookTags.Select(x => x.TagId).ToList();

            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(Book book)
        {
            Book existBook = _context.Books
            .Include(x => x.BookImages)
            .Include(x => x.BookTags).FirstOrDefault(x => x.Id == book.Id);
            if (existBook == null)
                return View("Error");

            CheckEdit(book, existBook);
            existBook.SalePrice = book.SalePrice;
            existBook.DiscountPercent = book.DiscountPercent;
            existBook.CostPrice = book.CostPrice;
            existBook.Name = book.Name;
            existBook.GenreId = book.GenreId;
            existBook.AuthorId = book.AuthorId;
            existBook.Desc = book.Desc;
            existBook.IsNew = book.IsNew;
            existBook.StockStatus = book.StockStatus;
            existBook.IsBestSeller = book.IsBestSeller;

            var newBookTags = book.TagIds
            .FindAll(x => !existBook.BookTags.Any(bt => bt.TagId == x))
            .Select(x => new BookTag { TagId = x }).ToList();

            existBook.BookTags.RemoveAll(x => !book.TagIds.Contains(x.TagId));
            existBook.BookTags.AddRange(newBookTags);

            UpdateBookImages(book, existBook);
            existBook.ModifiedAt = DateTime.UtcNow;
            _context.SaveChanges();
            return RedirectToAction("index");

        }
        private void CheckEdit(Book book, Book existBook)
        {
            if (existBook.AuthorId != book.AuthorId && !_context.Authors.Any(x => x.Id == book.AuthorId))
                ModelState.AddModelError("AuthorId", "Author not found");
            if (existBook.GenreId != book.GenreId && !_context.Genres.Any(x => x.Id == book.GenreId))
                ModelState.AddModelError("GenreId", "Genre not found");
            foreach (var tagId in book.TagIds)
            {
                if (!_context.Tags.Any(x => x.Id == tagId))
                {
                    ModelState.AddModelError("TagIds", "Tag not found");
                    break;
                }
            }
        }
        private void UpdateBookImages(Book book, Book existBook)
        {
            if (book.PosterFile != null)
            {
                var poster = existBook.BookImages.FirstOrDefault(x => x.PosterStatus == true);
                string oldImageName = poster.Image;

                poster.Image = FileManager.Save(book.PosterFile, _env.WebRootPath + "/uploads/books");
                FileManager.Delete(_env.WebRootPath + "/uploads/books", oldImageName);
            }
            if (book.HoverPosterFile != null)
            {
                var hoverPoster = existBook.BookImages.FirstOrDefault(x => x.PosterStatus == false);
                string oldImageName = hoverPoster.Image;
                hoverPoster.Image = FileManager.Save(book.HoverPosterFile, _env.WebRootPath + "/uploads/books");
                FileManager.Delete(_env.WebRootPath + "/uploads/books", oldImageName);
            }

            var removedFiles = existBook.BookImages.FindAll(x => x.PosterStatus == null && !book.BookImageIds.Contains(x.Id));
            FileManager.DeleteAll(_env.WebRootPath + "/uploads/books", removedFiles.Select(x => x.Image).ToList());
            existBook.BookImages.RemoveAll(x => removedFiles.Contains(x));

            foreach (var biFile in book.ImageFiles)
            {
                BookImage bi = new BookImage
                {
                    Image = FileManager.Save(biFile, _env.WebRootPath + "/uploads/books"),
                    PosterStatus = null,
                };
                existBook.BookImages.Add(bi);
            }
        }
        public IActionResult Delete(int id)
        {
            Book book = _context.Books.FirstOrDefault(x => x.Id == id);
            if (book == null)
                return NotFound();

            book.IsDeleted = true;
            _context.SaveChanges();

            return Ok();
        }
        private void ValidateBookFiles(Book book)
        {
            if (book.PosterFile == null)
                ModelState.AddModelError("PosterFile", "PosterFile is required");

            if (book.HoverPosterFile == null)
                ModelState.AddModelError("HoverPosterFile", "HoverPosterFile is required");

            if (book.PosterFile != null && !IsImageFile(book.PosterFile))
                ModelState.AddModelError("PosterFile", "PosterFile must be image/png or image/jpeg");

            if (book.HoverPosterFile != null && !IsImageFile(book.HoverPosterFile))
                ModelState.AddModelError("HoverPosterFile", "HoverPosterFile must be image/png or image/jpeg");

            if (book.PosterFile != null && book.PosterFile.Length > 2097152)
                ModelState.AddModelError("PosterFile", "PosterFile must be less or equal than 2MB");

            if (book.HoverPosterFile != null && book.HoverPosterFile.Length > 2097152)
                ModelState.AddModelError("HoverPosterFile", "HoverPosterFile must be less or equal than 2MB");

            foreach (var item in book.ImageFiles)
            {
                if (!IsImageFile(item))
                    ModelState.AddModelError("ImageFiles", "ImageFiles must be image/png or image/jpeg");

                if (item.Length > 2097152)
                    ModelState.AddModelError("ImageFiles", "ImageFiles must be less or equal than 2MB");
            }
        }
        private bool IsImageFile(IFormFile file)
        {
            return file.ContentType == "image/jpeg" || file.ContentType == "image/png";
        }
        private void SetViewBagValues()
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();
        }

        private bool IsImageValid(IFormFile file, string fieldName)
        {
            if (file.ContentType != "image/jpeg" && file.ContentType != "image/png")
            {
                ModelState.AddModelError(fieldName, "File must be image/png or image/jpeg");
                return false;
            }

            if (file.Length > 2097152)
            {
                ModelState.AddModelError(fieldName, "File must be less or equal than 2MB");
                return false;
            }
            return true;
        }

    }
}
