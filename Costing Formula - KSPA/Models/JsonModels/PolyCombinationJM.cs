namespace Costing_Formula___KSPA.Models.JsonModels
{
    public class PolyCombinationJM
    {
        public int Id { get; set; }
        public int ColorId { get; set; } // New
        public int PolyMaterialId { get; set; } // New
        public string PolyCombCode { get; set; } = String.Empty; // New
        public string PolyCombDesc { get; set; } = String.Empty;
        public List<PolyCombItemCatBridgeJM> PolyCombItemCatBridges { get; set; } = new();
    }
}
