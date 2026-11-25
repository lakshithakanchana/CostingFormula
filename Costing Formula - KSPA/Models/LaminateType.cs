using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Costing_Formula___KSPA.Models
{
    [Index(nameof(Desc), IsUnique = true)]
    public class LaminateType
    {
        [Key]
        public int LaminateTypeId { get; set; }
        [Required]
        public string Desc { get; set; } = String.Empty;
    }
}
