using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Costing_Formula___KSPA.Models
{
    public class Uom
    {
        [Key]
        public int UomId { get; set; }

        [Required]
        public int MeasurementId { get; set; }

        [Required]
        public string UomCode { get; set; }

        [Required]
        public string UomDesc { get; set; }

        [Required]
        public double ConversionMultiplierToSystemUom { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [ForeignKey("MeasurementId")]
        public virtual Measurement Measurement { get; set; }
    }
}
