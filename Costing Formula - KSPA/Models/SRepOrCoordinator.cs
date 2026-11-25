using System.ComponentModel.DataAnnotations;

namespace Costing_Formula___KSPA.Models
{
    public class SRepOrCoordinator
    {
        [Key]
        public int SrocId { get; set; }

        [Required]
        public string SrocName { get; set; }

        [Required, Phone, DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered contact number format is not valid.")]
        public string SrocContact { get; set; }

        [Required, EmailAddress]
        public string SrocEmail { get; set; }

        [Required]
        public bool IsReprecentative { get; set; }

        [Required]
        public bool IsCoordinator { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
