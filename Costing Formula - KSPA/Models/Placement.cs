using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Costing_Formula___KSPA.Models
{
    public class Placement
    {
        [Key]
        [DisplayName("Placement Id")]
        public int PlacementId { get; set; }

        [Required]
        [DisplayName("Placement Code")]
        [StringLength(5)]
        public string PlacementCode { get; set; } = String.Empty;

        [Required]
        [DisplayName("Placement Factor")]
        public double PlacementFactor { get; set; }

        [Required]
        [DisplayName("Active?")]
        public bool IsActive { get; set; }
    }
}
