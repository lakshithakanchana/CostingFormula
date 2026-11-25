using System.ComponentModel.DataAnnotations;

namespace Costing_Formula___KSPA.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
