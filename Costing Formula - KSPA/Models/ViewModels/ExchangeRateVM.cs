using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Costing_Formula___KSPA.Models.ViewModels
{
    public class ExchangeRateVM
    {
        [Required]
        public ExchangeRate? ExchangeRate { get; set; }

        // Already available as a virtual field inside ExchangeRate
        //[Required]
        //public Currency Currency { get; set; }

        public IEnumerable<ExchangeRate>? ExchangeRateList { get; set; }
    }
}
