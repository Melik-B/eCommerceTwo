using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Business.Models
{
    public class UserRegistrationModel
    {
        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be at least {1} characters long!")]
        [MaxLength(20, ErrorMessage = "{0} must be at most {1} characters long!")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be at least {1} characters long!")]
        [MaxLength(20, ErrorMessage = "{0} must be at most {1} characters long!")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be at least {1} characters long!")]
        [MaxLength(20, ErrorMessage = "{0} must be at most {1} characters long!")]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(15, ErrorMessage = "{0} must be at most {1} characters long!")]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("Password Confirmation")]
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(15, ErrorMessage = "{0} must be at most {1} characters long!")]
        [Compare("Password", ErrorMessage = "{0} must match {1}!")]
        public string PasswordConfirmation { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(200, ErrorMessage = "{0} must be at most {1} characters!")]
        [DisplayName("E-Mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        public string Address { get; set; }

        public UserDetailModel UserDetails { get; set; }

    }
}
