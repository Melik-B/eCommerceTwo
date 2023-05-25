using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Country : RecordBase
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public List<City> Cities { get; set; }

        public List<UserDetail> UserDetails { get; set; }

    }
}
