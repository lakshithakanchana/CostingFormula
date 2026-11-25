using Costing_Formula___KSPA.Models;
using Costing_Formula___KSPA.Utility;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Costing_Formula___KSPA.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Cylinder> Cylinders { get; set; }
        public DbSet<ProductFinishingType> ProductFinishingTypes { get; set; }
        public DbSet<BagType> BagTypes { get; set; }
        public DbSet<PolyMaterial> PolyMaterials { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<PolyCombItemCatBridge> PolyCombItemCatBridges { get; set; }
        public DbSet<PolyCombination> PolyCombinations { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; } // No view, No controller needed. Manage by db manager.
        public DbSet<SRepOrCoordinator> SRepsOrCoordinators { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<PrintType> PrintTypes { get; set; }
        public DbSet<ShipmentTerm> ShipmentTerms { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Uom> Uoms { get; set; }
        public DbSet<PolyCosting> PolyCostings { get; set; }
        public DbSet<ItemCategoryPrice> ItemCategoryPrices { get; set; }
        public DbSet<BoxType> BoxTypes { get; set; }
        public DbSet<Placement> Placements { get; set; }
        public DbSet<CorruCombination> CorruCombinations { get; set; }
        public DbSet<CorruCombItemCatBridge> CorruCombItemCatBridges { get; set; }
        public DbSet<CorruCosting> CorruCostings { get; set; }
        public DbSet<Models.Route> Routes { get; set; }
        public DbSet<ReelSize> ReelSizes { get; set; }
        public DbSet<LaminateType> LaminateTypes { get; set; }
        public DbSet<DieCutType> DieCutTypes { get; set; }
        public DbSet<CorruProductFinishingType> CorruProductFinishingType { get; set; }
    }
}
