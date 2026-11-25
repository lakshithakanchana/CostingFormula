namespace Costing_Formula___KSPA.Models.JsonModels
{
    public class BagTypeJM
    {
        public int Id { get; set; }
        public string BagTypeDesc { get; set; }
        public string ProductFinishingTypeDesc { get; set; }
        public bool PrintByWidthOrLength { get; set; }
        public string FormulaToCalcSArea { get; set; }
        public string SheetSizeFormula { get; set; }
    }
}
