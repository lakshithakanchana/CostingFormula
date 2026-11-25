using Microsoft.AspNetCore.Mvc.Rendering;

namespace Costing_Formula___KSPA.Models.ViewModels
{
    public class PolyCostingVM
    {
        public PolyCosting? PolyCosting { get; set; }
        public IEnumerable<SelectListItem>? CompanyDropDown { get; set; }
        public IEnumerable<SelectListItem>? CustomerDropDown { get; set; }
        public IEnumerable<SelectListItem>? ColorDropDown { get; set; }
        public IEnumerable<SelectListItem>? PolyCombinationDropDown { get; set; }
        public IEnumerable<SelectListItem>? PolyMaterialDropDown { get; set; }
        public IEnumerable<SelectListItem>? BagTypeDropDown { get; set; }
        public IEnumerable<SelectListItem>? OrderUomDropDown { get; set; }
        public IEnumerable<SelectListItem>? LengthUomDropDown { get; set; }
        public IEnumerable<SelectListItem>? PrintTypeDropDown { get; set; }
        public IEnumerable<SelectListItem>? ShipmentTermDropDown { get; set; }
        public IEnumerable<SelectListItem>? CylinderCodeDropDown { get; set; }
        public IEnumerable<SelectListItem>? BlockNameDropDown { get; set; }
    }
}
