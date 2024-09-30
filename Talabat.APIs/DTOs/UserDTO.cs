using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class UserDTO
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
