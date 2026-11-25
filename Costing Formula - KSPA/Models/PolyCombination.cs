using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Costing_Formula___KSPA.Models
{
    public class PolyCombination
    {
        [Key]
        public int PolyCombId { get; set; }

        [Required]
        public string PolyCombCode { get; set; } = String.Empty;

        [Required]
        public int ColorId { get; set; } //FK1

        [Required]
        public int PolyMaterialId { get; set; } //FK2

        [Required]
        public string PolyCombDesc { get; set; } = String.Empty;

        public string? SpecialRef { get; set; }

        [Required]
        public bool IsActive { get; set; }

        // Virtual properties
        public virtual List<PolyCombItemCatBridge>? PolyCombItemCatBridges { get; set; }

        [ForeignKey("ColorId")]
        public virtual Color? Color { get; set; }

        [ForeignKey("PolyMaterialId")]
        public virtual PolyMaterial? PolyMaterial { get; set; }
    }
}
