using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Business.Models
{
    public class CategoryModel : RecordBase
    {
        [Required]
        [StringLength(100)]
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }


        [DisplayName("Number of Products")]
        public int NumberOfProducts { get; set; }
    }
}