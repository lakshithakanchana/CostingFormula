using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Costing_Formula___KSPA.Models
{
    public class Cylinder
    {
        [Key]
        [DisplayName("Cylinder ID")]
        public int CylinderId { get; set; }

        [Required]
        [DisplayName("Cylinder Code")]
        public string CylinderCode { get; set; }

        [Required]
        [DisplayName("Cylinder Description")]
        public string CylinderDesc { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "No Of Colors must be positive.")]
        [DisplayName("No Of Colors")]
        public int NoOfColors { get; set; }

        [Required]
        [Range(0.00000001, double.MaxValue, ErrorMessage = "Cylinder Size must be positive.")]
        [DisplayName("Cylinder Size")]
        public double CylinderSizeCm { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "No Of Pieces must be positive.")]
        [DisplayName("No Of Pieces")]
        public int NoOfPieces { get; set; }

        [DisplayName("Active?")]
        public bool IsActive { get; set; }
    }
}
