using Microsoft.AspNetCore.Mvc.Rendering;

namespace Costing_Formula___KSPA.Models.ViewModels
{
    public class CorruCostingVM
    {
        public CorruCosting? CorruCosting { get; set; }
        public IEnumerable<SelectListItem>? CompanyDropDown { get; set; }
        public IEnumerable<SelectListItem>? CustomerDropDown { get; set; }
        public IEnumerable<SelectListItem>? ColorDropDown { get; set; }
        public IEnumerable<SelectListItem>? PlysDropDown { get; set; }
        public IEnumerable<SelectListItem>? BoxTypeDropDown { get; set; }
        public IEnumerable<SelectListItem>? CorruCombinationDropDown { get; set; }
        public IEnumerable<SelectListItem>? OrderUomDropDown { get; set; }
        public IEnumerable<SelectListItem>? PrintTypeDropDown { get; set; }
        public IEnumerable<SelectListItem>? LaminateTypeDropDown { get; set; }
        public IEnumerable<SelectListItem>? DieCutTypeDropDown { get; set; }
        public IEnumerable<SelectListItem>? FinishingTypeDropDown { get; set; }
        public IEnumerable<SelectListItem>? RouteDropDown { get; set; }
        public IEnumerable<SelectListItem>? ShipmentTermDropDown { get; set; }
        public IEnumerable<SelectListItem>? MeasurementUomDropDown { get; set; }
    }
}
