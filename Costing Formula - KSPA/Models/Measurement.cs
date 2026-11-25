using System.ComponentModel.DataAnnotations;

namespace Costing_Formula___KSPA.Models
{
    public class Measurement
    {
        [Key]
        public int MeasurementId { get; set; }

        [Required]
        public string MeasurementDesc { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
