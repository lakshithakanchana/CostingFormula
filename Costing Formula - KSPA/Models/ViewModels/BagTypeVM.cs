using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;

namespace Costing_Formula___KSPA.Models.ViewModels
{
    public class BagTypeVM
    {
        [Required]
        public BagType BagType { get; set; }

        public IEnumerable<SelectListItem>? ProductFinishingTypeDropDown { get; set; }
    }
}
