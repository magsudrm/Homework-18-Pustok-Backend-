using System.ComponentModel.DataAnnotations;

namespace Pustok.Models
{
	public class Author
	{
		public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }
        public int BirthYear { get; set; }
        public ICollection<Book> Books { get; set; }
	}
}
