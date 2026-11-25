using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Costing_Formula___KSPA.Models
{
    [Index(nameof(BagTypeCode), IsUnique = true)]
    [Index(nameof(BagTypeDesc), IsUnique = true)]
    public class BagType
    {
        [Key]
        [DisplayName("Bag Type Id")]
        public int BagTypeId { get; set; }

        [Required]
        [DisplayName("Bag Type Code")]
        public string BagTypeCode { get; set; } = String.Empty;

        [Required]
        [DisplayName("Bag Type Desc")]
        public string BagTypeDesc { get; set; } = String.Empty;

        [Required]
        [DisplayName("Formula To Calculate Sheet Area")]
        public string FormulaToCalcSArea { get; set; } = String.Empty;

        [Required]
        [DisplayName("Printed by?")]
        public bool PrintByWidthOrLength { get; set; } // 1-pByW 0-pByL

        [Required]
        [DisplayName("Sheet Size Formula")]
        public string SheetSizeFormula { get; set; } = String.Empty;

        [Required]
        [DisplayName("Printed?")]
        public bool Printed { get; set; }
        [Required]
        [DisplayName("Side Seal?")]
        public bool SideSeal { get; set; }
        [Required]
        [DisplayName("Side Gusset?")]
        public bool SideGusset { get; set; }
        [Required]
        [DisplayName("Hanger Attached?")]
        public bool HangerAttached { get; set; }
        [Required]
        [DisplayName("Flap?")]
        public bool Flap { get; set; }
        [Required]
        [DisplayName("Hanger Bag?")]
        public bool HangerBag { get; set; }
        [Required]
        [DisplayName("Bottom Gusset?")]
        public bool BottomGusset { get; set; }
        [Required]
        [DisplayName("Handle Cut?")]
        public bool HandleCut { get; set; }
        [Required]
        [DisplayName("Bottom Seal?")]
        public bool BottomSeal { get; set; }
        [Required]
        [DisplayName("Punch Hole?")]
        public bool PunchHole { get; set; }
        [Required]
        [DisplayName("Top Seal")]
        public bool TopSeal { get; set; }
        [Required]
        [DisplayName("Zipper?")]
        public bool Zipper { get; set; }
        [Required]
        [DisplayName("Active?")]
        public bool Isactive { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select the Product Finishing Type")]
        [DisplayName("Product Finishing Type")]
        public int ProductFinishingTypeId { get; set; } //Forign Key

        [ForeignKey("ProductFinishingTypeId")]
        public virtual ProductFinishingType? ProductFinishingType { get; set; }

    }
}
