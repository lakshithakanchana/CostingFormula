using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Costing_Formula___KSPA.Models
{
    public class CorruCombItemCatBridge
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CorruCombId { get; set; } // FK1

        [Required]
        public int PlacementId { get; set; } // FK2

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The ItemCatId field is required.")]
        public int ItemCatId { get; set; }  //FK3

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "The Gsm field is required.")]
        public int Gsm { get; set; }

        // Virtual Properties
        [ForeignKey("CorruCombId")]
        public virtual CorruCombination? CorruCombination { get; set; } // This will be null most cases until the software grow. But to add foreign key index. This is must.
        [ForeignKey("PlacementId")]
        public virtual Placement? Placement { get; set; }
        [ForeignKey("ItemCatId")]
        public virtual ItemCategory? ItemCategory { get; set; }
    }
}
