using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok.DAL;
using Pustok.Models;
using Pustok.ViewModels;

namespace Pustok.Services
{
	public class LayoutService
	{
		private readonly PustokDbContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public LayoutService(PustokDbContext context,IHttpContextAccessor httpContextAccesor)
		{
			_context = context;
			_httpContextAccessor = httpContextAccesor;
		}

		public Dictionary<string, string> GetSettings()
		{
			return _context.Settings.ToDictionary(x => x.Key, x => x.Value);
		}

		public List<Genre> GetGenres()
		{
			return _context.Genres.ToList();
		}

		public BasketViewModel GetBasket()
		{
			BasketViewModel basketVm = new BasketViewModel();
			List<BasketCokkieItemViewModel> basketItems = new List<BasketCokkieItemViewModel>();
			var basketJson = _httpContextAccessor.HttpContext.Request.Cookies["basket"];
			if (basketJson != null)
			{
				basketItems = JsonConvert.DeserializeObject<List<BasketCokkieItemViewModel>>(basketJson);
			}

			foreach (var item in basketItems)
			{
				var book= _context.Books.Include(x=> x.BookImages.Where(y=> y.PosterStatus==true)).FirstOrDefault(x => x.Id == item.BookId);

				basketVm.BasketItems.Add(new BasketItemViewModel
				{
					Book = book,
					Count = item.Count
				});
				var price = book.DiscountPercent > 0 ? (book.SalePrice * (100 - book.DiscountPercent) / 100): book.SalePrice;
				basketVm.TotalPrice += (price * item.Count);
			}
			return basketVm;

        }
    }
}
