using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public int Order { get; set; }
        [MaxLength(30)]
        public string Title1 { get; set; }
        [MaxLength(30)]
        public string Title2 { get; set; }
        [MaxLength(150)]
        public string Image { get; set; }
        [MaxLength(60)]
        [Required]
        public string ButtonText { get; set; }
        [MaxLength(220)]
        public string ButtonUrl { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }

    }
}
