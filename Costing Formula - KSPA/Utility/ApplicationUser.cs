using Microsoft.AspNetCore.Identity;
namespace Costing_Formula___KSPA.Utility
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; }
    }
}
