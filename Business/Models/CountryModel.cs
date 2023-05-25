using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Business.Models
{
    public class CountryModel : RecordBase
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(100, ErrorMessage = "{0} must be at most {1} characters long!")]
        [DisplayName("Country Name")]
        public string Name { get; set; }
    }
}
