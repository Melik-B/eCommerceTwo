using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Business.Models
{
    public class RoleModel : RecordBase
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(20, ErrorMessage = "{0} must be at most {1} characters long!")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Users")]
        public List<string> UsersDisplay { get; set; }
    }
}
