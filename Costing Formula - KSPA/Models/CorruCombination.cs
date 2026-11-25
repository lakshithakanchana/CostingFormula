using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Costing_Formula___KSPA.Models
{
    [Index(nameof(CorruCombCode), IsUnique = true)]
    public class CorruCombination
    {
        [Key]
        [DisplayName("Combination Id")]
        public int CorruCombId { get; set; }

        [Required]
        [DisplayName("Combination Code")]
        public string CorruCombCode { get; set; } = String.Empty;

        [Required]
        [DisplayName("Color Id")]
        public int ColorId { get; set; } //FK1

        [Required]
        [DisplayName("No Of Plys")]
        [Range(1, 7)]
        public int NoOfPlys { get; set; }

        [Required]
        [DisplayName("Active?")]
        public bool IsActive { get; set; }

        // Virtual properties
        public virtual List<CorruCombItemCatBridge> CorruCombItemCatBridges { get; set; } = new();

        [ForeignKey("ColorId")]
        public virtual Color? Color { get; set; }

    }
}
