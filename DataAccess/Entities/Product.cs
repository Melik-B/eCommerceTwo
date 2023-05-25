using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Product : RecordBase
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public double UnitPrice { get; set; }

        public int StockQuantity { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}