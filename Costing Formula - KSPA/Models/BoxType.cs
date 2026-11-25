using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Costing_Formula___KSPA.Models
{
    [Index(nameof(BoxTypeCode), IsUnique = true)]
    [Index(nameof(BoxTypeDesc), IsUnique = true)]
    public class BoxType
    {
        [Key]
        [DisplayName("Box Type Id")]
        public int BoxTypeId { get; set; }

        [Required]
        [DisplayName("Box Type Code")]
        public string BoxTypeCode { get; set; } = String.Empty;

        [Required]
        [DisplayName("Box Type Desc")]
        public string BoxTypeDesc { get; set; } = String.Empty;

        [Required]
        [DisplayName("No Of Plys")]
        [Range(1, 7)]
        public int NoOfPlys { get; set; }

        [DisplayName("Formula For External Sheet Length")]
        public string? ESheetLengthFormula { get; set; } = String.Empty;

        [DisplayName("Formula For External Sheet Length If Sheet Length Exceeds The Max Sheet Length")]
        public string? ESheetLengthFormulaIfSLExceeds { get; set; } = String.Empty;

        [DisplayName("Formula For External Sheet Width")]
        public string? ESheetWidthFormula { get; set; } = String.Empty;

        [DisplayName("Formula For Internal Sheet Length")]
        public string? ISheetLengthFormula { get; set; } = String.Empty;

        [DisplayName("Formula For Internal Sheet Length If Sheet Length Exceeds The Max Sheet Length")]
        public string? ISheetLengthFormulaIfSLExceeds { get; set; } = String.Empty;

        [DisplayName("Formula For Internal Sheet Width")]
        public string? ISheetWidthFormula { get; set; } = String.Empty;

        [Required]
        [DisplayName("Laminate?")]
        public bool Laminate { get; set; }

        [Required]
        [DisplayName("DieCut?")]
        public bool DieCut { get; set; }

        [Required]
        [DisplayName("DesignBox?")]
        public bool DesignBox { get; set; }

        [Required]
        [DisplayName("Board?")]
        public bool Board { get; set; }

        [Required]
        [DisplayName("Active?")]
        public bool IsActive { get; set; }
    }
}
