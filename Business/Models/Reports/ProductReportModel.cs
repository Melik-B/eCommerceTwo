using System.ComponentModel;

namespace Business.Models.Reports
{
    public class ProductReportModel
    {
        public int? CategoryId { get; set; }

        [DisplayName("Category")]
        public string CategoryName { get; set; }


        [DisplayName("Product")]
        public string ProductName { get; set; }

        public string Product { get; set; }

    }
}
