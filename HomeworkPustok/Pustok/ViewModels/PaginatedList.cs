using System.Drawing.Printing;
using Pustok.Models;
namespace Pustok.ViewModels
{
    public class PaginatedList<T>
    {
        public PaginatedList(List<T> items, int pageIndex, int pageSize, int pageCount)
        {
            Items = items;
            PageIndex = pageIndex;
            PageSize = pageSize;
            PageCount = pageCount;
        }


        public List<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public bool HasNext => PageIndex < PageCount;
        public bool HasPrev => PageIndex > 1;


        public static PaginatedList<T> Create(IQueryable<T> query, int pageIndex, int pageSize)
        {
            var pageCount = (int)Math.Ceiling(query.Count() / (double)pageSize);
            var items = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedList<T>(items, pageIndex, pageSize, pageCount);
        }
    }

}
