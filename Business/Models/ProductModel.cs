using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Business.Models
{
    public class ProductModel : RecordBase
    {
        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(2, ErrorMessage = "{0} must be at least {1} characters!")]
        [MaxLength(100, ErrorMessage = "{0} must be at most {1} characters!")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "{0} must be at most {1} characters!")]
        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Unit Price")]
        [Required(ErrorMessage = "{0} is required!")]
        [Range(0, double.MaxValue, ErrorMessage = "{0} must be between {1} and {2}!")]
        public double? UnitPrice { get; set; }

        [DisplayName("Stock Quantity")]
        [Required(ErrorMessage = "{0} is required!")]
        [Range(0, 1000000, ErrorMessage = "{0} must be between {1} and {2}!")]
        public int? StockQuantity { get; set; }

        [DisplayName("Expiration Date")]
        public DateTime? ExpirationDate { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "{0} is required!")]
        public int? CategoryId { get; set; }



        [DisplayName("Unit Price")]
        public string UnitPriceDisplay { get; set; }

        [DisplayName("Expiration Date")]
        public string ExpirationDateDisplay { get; set; }

        [DisplayName("Category")]
        public string CategoryNameDisplay { get; set; }
    }
}
