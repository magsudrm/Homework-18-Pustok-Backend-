using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Pustok.Attributes.ValidationAttributes;

namespace Pustok.Models
{
	public class Book
	{
		public int Id { get; set; }
		[Required]
		[MaxLength(45)]
		public string Name { get; set; }
		[MaxLength(700)]
		public string Desc { get; set; }
		public int GenreId { get; set; }
		public int AuthorId { get; set; }
		[Column(TypeName = "money")]
		public decimal SalePrice { get; set; }
		[Column(TypeName = "money")]
		public decimal CostPrice { get; set; }
		[Column(TypeName = "money")]
		public decimal DiscountPercent { get; set; }
		[Required]
		public bool StockStatus { get; set; }

		public bool IsBestSeller { get; set; }
		public bool IsNew { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        public Genre Genre { get; set; }
		public Author Author { get; set; }

		public List<BookTag> BookTags { get; set; }
		public List<BookImage> BookImages { get; set; } = new List<BookImage>();
        [NotMapped]
        [FileMaxLength(2097152)]
        [FileAllowedTypes("image/png", "image/jpeg")]
        public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();
        [NotMapped]
        [FileMaxLength(2097152)]
        [FileAllowedTypes("image/png", "image/jpeg")]
        public IFormFile PosterFile { get; set; }
        [NotMapped]
        [FileMaxLength(2097152)]
        [FileAllowedTypes("image/png", "image/jpeg")]
        public IFormFile HoverPosterFile { get; set; }
        [NotMapped]
        public List<int> TagIds { get; set; } = new List<int>();
        [NotMapped]
        public List<int> BookImageIds { get; set; } = new List<int>();
        public List<BookReview> BookReviews { get; set; }

    }
}
