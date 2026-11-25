using System.ComponentModel.DataAnnotations;

namespace Costing_Formula___KSPA.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; } // Polythene, Corrugation
    }
}
