using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Business.Models.Filters
{
    public class ProductReportFilterModel
    {
        [DisplayName("Category")]
        public int? CategoryId { get; set; }

        [StringLength(40, ErrorMessage = "{0} must be at most {1} characters long!")]
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
    }
}
