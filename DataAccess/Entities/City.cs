using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.Metrics;

namespace DataAccess.Entities
{
    public class City : RecordBase
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(150, ErrorMessage = "{0} must be at most {1} characters long!")]
        [DisplayName("City Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Country Name")]
        public int CountryId { get; set; }
        public Country Country { get; set; }

        public List<UserDetail> UserDetails { get; set; }
    }
}
