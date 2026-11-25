using System.ComponentModel.DataAnnotations;

namespace Costing_Formula___KSPA.Models.ViewModels
{
    public class CreateUserVM
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        public bool IsActive { get; set; }
    }
}
