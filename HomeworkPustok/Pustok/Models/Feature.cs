using System.ComponentModel.DataAnnotations;

namespace Pustok.Models
{
    public class Feature
    {
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Title { get; set; }
        [MaxLength(50)]
        [Required]
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}
