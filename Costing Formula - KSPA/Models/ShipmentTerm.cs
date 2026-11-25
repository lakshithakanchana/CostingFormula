using System.ComponentModel.DataAnnotations;

namespace Costing_Formula___KSPA.Models
{
    public class ShipmentTerm
    {
        [Key]
        public int ShipmentTermId { get; set; }

        [Required]
        public string ShipmentTermCode { get; set; }

        [Required]
        public string ShipmentTermName { get; set; }

        [Required]
        public bool FreightBySupplierOrCarried { get; set; } //1 - Sup, 0 - Carrier

        [Required]
        public bool InsuranceBySupplierOrAgency { get; set; } //1 - Sup, 0 - Agency

        [Required]
        public bool IsActive { get; set; }
    }
}
