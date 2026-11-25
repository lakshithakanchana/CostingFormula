using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Costing_Formula___KSPA.Models
{
    public class Route
    {
        [Key]
        [DisplayName("Route Id")]
        public int RouteId { get; set; }
        [Required]
        [DisplayName("Route Name")]
        public string RouteName { get; set; } = String.Empty;
        [Required]
        [DisplayName("Fixed Excess")]
        public int FixedExcess { get; set; }
        [Required]
        [DisplayName("Handling Waste %")]
        public double HandlingWastePercent { get; set; }
    }
}
