using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Costing_Formula___KSPA.Models
{
    [Index(nameof(CurrencyId), nameof(EffectiveFrom), IsUnique = true)]
    [Index(nameof(EffectiveFrom))]
    public class ExchangeRate
    {
        [Key]
        public int ExchangeRateId { get; set; }

        [Required]
        public int CurrencyId { get; set; } //FK

        [Required]
        [Range(0.001, 10000)]
        public double? RateInLKR { get; set; }

        [Required]
        public DateTime EffectiveFrom { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [ForeignKey("CurrencyId")]
        public virtual Currency? Currency { get; set; }
    }
}
