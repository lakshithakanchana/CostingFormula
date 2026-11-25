namespace Costing_Formula___KSPA.Models.JsonModels
{
    public class CorruCombinationJM
    {
        public int Id { get; set; }
        public int ColorId { get; set; }
        public int NoOfPlys { get; set; }
        public string CorruCombCode { get; set; } = String.Empty;
        public List<CorruCombItemCatBridgeJM> CorruCombItemCatBridges { get; set; } = new();
    }
}
