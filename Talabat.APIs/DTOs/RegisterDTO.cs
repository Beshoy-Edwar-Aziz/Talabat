using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage ="Name is Required")]
        public string DisplayName { get; set; }
        [EmailAddress(ErrorMessage ="Email is Incorrect")]
        [Required(ErrorMessage ="Email is Required")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Password is Required")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Minimum eight characters, at least one letter and one number")]
        public string Password { get; set; }
        [Required(ErrorMessage ="Phone is Required")]
        [Phone(ErrorMessage ="Phone Number You Entered is Incorrect")]
        public string PhoneNumber { get; set; }
        
    }
}
