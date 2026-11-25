using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Costing_Formula___KSPA.Models
{
    public class PolyMaterial
    {
        [Key]
        public int PolyMaterialId { get; set; }
        [Required]
        [DisplayName("Poly material code same as in Axpert")]
        public string PolyMatCode { get; set; }
        [Required]
        [DisplayName("Active?")]
        public bool IsActive { get; set; }
    }
}
