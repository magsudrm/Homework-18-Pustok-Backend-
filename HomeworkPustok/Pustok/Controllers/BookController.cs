using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok.DAL;
using Pustok.Models;
using Pustok.ViewModels;

namespace Pustok.Controllers
{
    public class BookController : Controller
    {
        private readonly PustokDbContext _context;

        public BookController(PustokDbContext context)
        {
            _context = context;
        }

        public IActionResult Detail(int id)
        {
            Book book = _context.Books
                .Include(x => x.BookReviews).ThenInclude(x => x.AppUser)
                .Include(x => x.BookImages)
                .Include(x => x.BookTags).ThenInclude(bt => bt.Tag)
                .Include(x => x.Author)
                .Include(x => x.Genre)
                .FirstOrDefault(x => x.Id == id);
            return View(book);
        }

        public IActionResult GetBookModal(int id)
        {
            var book = _context.Books
                .Include(x => x.Genre)
                .Include(x => x.Author)
                .Include(x => x.BookImages)
                .Include(x=>x.BookTags)
                .FirstOrDefault(x => x.Id == id);

            return PartialView("_BookModalPartial", book);
        }

		public IActionResult AddToBasket(int id)
		{
			List<BasketCokkieItemViewModel> basketItems;
			var basket = HttpContext.Request.Cookies["basket"];

			if (basket == null)
				basketItems = new List<BasketCokkieItemViewModel>();
			else
				basketItems = JsonConvert.DeserializeObject<List<BasketCokkieItemViewModel>>(basket);

			var wantedBook = basketItems.FirstOrDefault(x => x.BookId == id);

			if (wantedBook == null)
				basketItems.Add(new BasketCokkieItemViewModel { Count = 1, BookId = id });
			else
				wantedBook.Count++;


			HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketItems));
            BasketViewModel basketVm= new BasketViewModel();
            foreach (var item in basketItems)
            {
                var book = _context.Books.Include(x => x.BookImages.Where(x => x.PosterStatus == true)).FirstOrDefault(x => x.Id == item.BookId);

                basketVm.BasketItems.Add(new BasketItemViewModel
                {
                    Book = book,
                    Count = item.Count
                });
                var price = book.DiscountPercent > 0 ? (book.SalePrice * (100 - book.DiscountPercent) / 100) : book.SalePrice;
                basketVm.TotalPrice += (price * item.Count);
            }
            return PartialView("_BasketCartPartial", basketVm);
		}

		public IActionResult ShowBasket()
		{
			var basket = HttpContext.Request.Cookies["basket"];
			var basketItems = JsonConvert.DeserializeObject<List<BasketCokkieItemViewModel>>(basket);


			return Json(basketItems);
		}

        public IActionResult RemoveBasket(int id)
        {
            var basket = Request.Cookies["basket"];
            if(basket == null)
            {
                return NotFound();
            }
            List<BasketCokkieItemViewModel> basketItems =JsonConvert.DeserializeObject<List<BasketCokkieItemViewModel>>(basket);
            BasketCokkieItemViewModel item = basketItems.Find(x => x.BookId == id);
            if(item == null)
            {
                return NotFound();
            }
            basketItems.Remove(item);
            Response.Cookies.Append("basket",JsonConvert.SerializeObject(basketItems));
            decimal totalPrice = 0;
            foreach (var bi in basketItems)
            {
                var book = _context.Books.Include(x => x.BookImages.Where(x => x.PosterStatus == true)).FirstOrDefault(x => x.Id == bi.BookId);
                var price = book.DiscountPercent > 0 ? (book.SalePrice * (100 - book.DiscountPercent) / 100) : book.SalePrice;
                totalPrice += (price * bi.Count);
            }
            return Ok(new {count=basketItems.Count,totalPrice=totalPrice.ToString("0.00")});
        }
	}
}
