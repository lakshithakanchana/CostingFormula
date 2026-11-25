using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Costing_Formula___KSPA.Models
{
    [Index(nameof(ColorName), IsUnique = true)]
    public class Color
    {
        [Key]
        public int ColorId { get; set; }
        [Required]
        public string ColorName { get; set; } = String.Empty;
        [Required]
        public bool IsActive { get; set; }
    }
}
