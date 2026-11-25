namespace Costing_Formula___KSPA.Models.JsonModels
{
    public class RouteJM
    {
        public int Id { get; set; }
        public string RouteName { get; set; } = String.Empty;
        public int FixedExcess { get; set; }
        public double HandlingWastePercent { get; set; }
    }
}
