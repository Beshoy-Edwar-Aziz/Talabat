using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage ="Email is Required")]
        [EmailAddress(ErrorMessage ="Email is Incorrect")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Password is Required")]
        public string Password { get; set; }
    }
}
