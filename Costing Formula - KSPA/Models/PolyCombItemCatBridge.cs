using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Costing_Formula___KSPA.Models
{
    public class PolyCombItemCatBridge
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PolyCombId { get; set; } // FK1

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The ItemCatId field is required.")]
        public int ItemCatId { get; set; }  //FK2

        [Required]
        [Range(0.00, 100.00, ErrorMessage = "Percentage is between 0% and 100%")]
        public double Percentage { get; set; }

        [ForeignKey("PolyCombId")]
        public virtual PolyCombination? PolyCombination { get; set; } // This will be null most cases until the software grow. But to add foreign key index. This is must.

        [ForeignKey("ItemCatId")]
        public virtual ItemCategory? ItemCategory { get; set; }
    }
}
