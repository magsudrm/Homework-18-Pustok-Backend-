using System.ComponentModel.DataAnnotations;

namespace Pustok.Areas.manage.ViewModels
{
    public class AdminLoginViewModel
    {
        [MaxLength(30)]
        [MinLength(5)]
        [Required]
        public string UserName { get; set; }
        [Required]
        [MinLength(6)]
        [MaxLength(30)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
