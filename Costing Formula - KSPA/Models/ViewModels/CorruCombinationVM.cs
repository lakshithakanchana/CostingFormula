using Microsoft.AspNetCore.Mvc.Rendering;

namespace Costing_Formula___KSPA.Models.ViewModels
{
    public class CorruCombinationVM
    {

        public CorruCombination CorruCombination { get; set; } = new();

        public IEnumerable<SelectListItem>? ColorDropDown { get; set; }
        public IEnumerable<SelectListItem>? PlacementDropDown { get; set; }
        public IEnumerable<SelectListItem>? ItemCategoryDropDown { get; set; }

        // JSON virtual arrays
        public virtual string? CorruCombItemCatBridgesJSONArray { get; set; }
    }
}
