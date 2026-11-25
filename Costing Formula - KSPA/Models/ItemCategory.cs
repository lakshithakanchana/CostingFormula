using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Costing_Formula___KSPA.Models
{
    [Index(nameof(ItemCategoryCode), IsUnique = true)]
    [Index(nameof(ItemCategoryDesc), IsUnique = true)]
    public class ItemCategory
    {
        [Key]
        public int ItemCatId { get; set; }
        [Required]
        public string ItemCategoryCode { get; set; } = String.Empty;
        [Required]
        public string ItemCategoryDesc { get; set; } = String.Empty;
        [Required]
        public bool IsPolyComb { get; set; }
        [Required]
        public bool IsCorruComb { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
