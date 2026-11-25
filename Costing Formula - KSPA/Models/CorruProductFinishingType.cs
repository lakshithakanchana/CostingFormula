using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Costing_Formula___KSPA.Models
{
    [Index(nameof(Desc), IsUnique = true)]
    public class CorruProductFinishingType
    {
        [Key]
        public int CorruProductFinishingTypeId { get; set; }
        [Required]
        public string Desc { get; set; } = String.Empty;
    }
}
