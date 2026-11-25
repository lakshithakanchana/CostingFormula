using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Costing_Formula___KSPA.Models
{
    public class ItemCategoryPrice
    {
        [Key]
        public int IcpId { get; set; }

        [Required]
        public int ItemCatId { get; set; } //FK

        [Required, Range(0.0001, double.MaxValue)]
        public double UnitPrice { get; set; }

        [Required]
        public double UnitDuty { get; set; }

        [Required]
        public DateTime DateUpdated { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [ForeignKey("ItemCatId")]
        public virtual ItemCategory? ItemCategory { get; set; }
    }
}
