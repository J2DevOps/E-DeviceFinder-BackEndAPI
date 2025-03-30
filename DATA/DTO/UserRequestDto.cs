using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DATA.DTO
{
    public class UserRequestDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        [EmailAddress, Required]
        public string Email { get; set; }
        [Required, PasswordPropertyText]
        public string Password { get; set; }
    }
}
