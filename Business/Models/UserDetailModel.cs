using AppCore.Records.Bases;
using DataAccess.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Business.Models
{
    public class UserDetailModel : RecordBase
    {
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(200, ErrorMessage = "{0} must be at most {1} characters long!")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Country")]
        [Required(ErrorMessage = "{0} is required!")]
        public int? CountryId { get; set; }

        [DisplayName("City")]
        [Required(ErrorMessage = "{0} is required!")]
        public int? CityId { get; set; }

        public string CountryNameDisplay { get; set; }
        public string CityNameDisplay { get; set; }
    }
}
