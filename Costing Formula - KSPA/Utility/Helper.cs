using Microsoft.AspNetCore.Mvc.Rendering;

namespace Costing_Formula___KSPA.Utility
{
    public static class Helper
    {
        public static string Admin = "Admin";
        public static string RepOrCoordinator = "RepOrCoordinator";

        public static List<SelectListItem> GetRolesForDropDown()
        {
            return new List<SelectListItem>
            {
                new SelectListItem{ Value = Helper.Admin, Text = Helper.Admin},
                new SelectListItem{ Value = Helper.RepOrCoordinator, Text = Helper.RepOrCoordinator}
            };
        }
    }
}
