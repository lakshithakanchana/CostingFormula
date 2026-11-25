using System.ComponentModel.DataAnnotations;

namespace Costing_Formula___KSPA.Models
{
    public class ReelSize
    {
        [Key]
        public int ReelSizeId { get; set; }
        [Required]
        [Range(80, double.MaxValue, ErrorMessage = "Size of the reel must be greater than or equal to 80.")] 
        public double Size { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
