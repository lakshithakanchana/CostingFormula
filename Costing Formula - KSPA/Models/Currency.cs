using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Costing_Formula___KSPA.Models
{
    [Index(nameof(CurrencyCode), IsUnique = true), Index(nameof(CurrencyDesc), IsUnique = true)]
    public class Currency
    {
        [Key]
        public int CurrencyId { get; set; }

        [Required, StringLength(25)]
        public string CurrencyCode { get; set; }

        [Required, StringLength(100)]
        public string CurrencyDesc { get; set; }

        [Required]
        public bool IsActive { get; set; }

        //public virtual ExchangeRate? ChoosenExchangeRate { get; set; }
    }
}
