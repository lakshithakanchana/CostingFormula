using System.ComponentModel.DataAnnotations;

namespace Costing_Formula___KSPA.Models
{
    public class CustomerType
    {
        [Key]
        public int CustomerTypeId { get; set; }

        [Required]
        public string CustomerTypeDesc { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
