using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Costing_Formula___KSPA.Models
{
    [Index(nameof(CustomerShortCode), IsUnique = true)]
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        public int CustomerTypeId { get; set; } //FK

        [Required]
        public int CurrencyId { get; set; } //FK

        [Required]
        public int CompanyId { get; set; } //FK, new

        [Required]
        public string CustomerShortCode { get; set; } = String.Empty;

        [Required]
        public string CustomerName { get; set; } = String.Empty;

        public string? CustomerAddress { get; set; }

        public string? Email { get; set; } // new

        [Required]
        [DisplayName("Representative Id")]
        public int SRepId { get; set; } //FK

        [Required]
        [DisplayName("Coordinator Id")]
        public int SCoordinatorId { get; set; } //FK

        [Required]
        public bool IsActive { get; set; }

        // Virtual attributes.................................................................................................................
        [ForeignKey("CustomerTypeId")]
        public virtual CustomerType? CustomerType { get; set; }

        [ForeignKey("SRepId")]
        public virtual SRepOrCoordinator? SRep { get; set; }

        [ForeignKey("SCoordinatorId")]
        public virtual SRepOrCoordinator? SCoordinator { get; set; }

        [ForeignKey("CurrencyId")]
        public virtual Currency? Currency { get; set; } //May change to Currency View Model

        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; } // new
    }
}
