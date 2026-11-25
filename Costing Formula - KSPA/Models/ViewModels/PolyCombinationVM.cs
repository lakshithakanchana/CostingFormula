using Microsoft.AspNetCore.Mvc.Rendering;

namespace Costing_Formula___KSPA.Models.ViewModels
{
    public class PolyCombinationVM
    {

        public PolyCombination? PolyCombination { get; set; }

        public IEnumerable<SelectListItem>? ItemCategoryDropDown { get; set; } //For the percentage table select box

        public IEnumerable<SelectListItem>? ColorDropDown { get; set; }

        public IEnumerable<SelectListItem>? PolyMaterialDropDown { get; set; }

        // JSON virtual arrays
        public virtual string? PolyCombItemCatBridgesJSONArray { get; set; }
    }
}
