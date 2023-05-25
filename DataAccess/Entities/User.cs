using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class User : RecordBase
    {
        [Required]
        [MinLength(3)]
        [MaxLength(18)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(18)]
        public string LastName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [StringLength(20)]
        public string Password { get; set; }

        public bool IsActive { get; set; }

        public UserDetail UserDetail { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

    }
}
