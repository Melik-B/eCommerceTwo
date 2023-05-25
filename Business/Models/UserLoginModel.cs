using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Business.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be at least {1} characters long!")]
        [MaxLength(20, ErrorMessage = "{0} must be at most {1} characters long!")]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(15, ErrorMessage = "{0} must be at most {1} characters long!")]
        [DisplayName("Password")]
        public string Password { get; set; }
    }
}
