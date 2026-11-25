using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;

namespace Costing_Formula___KSPA.Models.ViewModels
{
    public class CustomerVM
    {
        [Required]
        public Customer Customer { get; set; }

        public IEnumerable<SelectListItem>? CustomerTypeDropDown { get; set; }

        public IEnumerable<SelectListItem>? SRepDropDown { get; set; }

        public IEnumerable<SelectListItem>? SCoordinatorDropDown { get; set; }

        public IEnumerable<SelectListItem>? CurrencyDropDown { get; set; } //May change to Currency View Model
        public IEnumerable<SelectListItem>? CompanyDropDown { get; set; } //May change to Currency View Model
    }
}
