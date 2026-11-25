using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Costing_Formula___KSPA.Models
{
    public class ProductFinishingType
    {
        [Key]
        [DisplayName("Product Finishing Type Id")]
        public int ProductFinTyId { get; set; }
        [Required]
        [DisplayName("Product Finishing Type Description")]
        public string ProductFinishingTypeDesc { get; set; }

    }
}
