using System.ComponentModel.DataAnnotations;

namespace Pustok.ViewModels
{
    public class UserLoginViewModel
    {
        [Required]
        [MaxLength(20)]
        [MinLength(4)]
        public string UserName { get; set;}
        [Required]
        [MaxLength(25)]
        [DataType(DataType.Password)]
        public string Password { get; set;}
        public bool RememberMe { get; set;}
    }
}
