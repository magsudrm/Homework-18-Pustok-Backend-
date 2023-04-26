namespace Pustok.Models
{
	public class BookImage
	{
		public int Id { get; set; }
		public int BookId { get; set; }
		public string Image { get; set; }
		public bool? PosterStatus { get; set; }

		public Book Book { get; set; }
	}
}
