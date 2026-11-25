using System.ComponentModel.DataAnnotations;

namespace Costing_Formula___KSPA.Models
{
    public class PrintType
    {
        [Key]
        public int PrintTypeId { get; set; }

        [Required]
        public string PrintTypeCode { get; set; }

        [Required]
        public string PrintTypeDesc { get; set; }

        [Required]
        public double ConsumptionLitrePerSqrCm { get; set; }

        [Required]
        public double PrintPercentage { get; set; }

        [Required]
        public bool WithoutCylinderBase { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
