namespace Costing_Formula___KSPA.Models.JsonModels
{
    public class BoxTypeJM
    {
        public int Id { get; set; }
        public string BoxTypeDesc { get; set; } = String.Empty;
        public int NoOfPlys { get; set; }
        public string ESheetLengthFormula { get; set; } = String.Empty;
        public string ESheetLengthFormulaIfSLExceeds { get; set; } = String.Empty;
        public string ESheetWidthFormula { get; set; } = String.Empty;
        public string ISheetLengthFormula { get; set; } = String.Empty;
        public string ISheetLengthFormulaIfSLExceeds { get; set; } = String.Empty;
        public string ISheetWidthFormula { get; set; } = String.Empty;
        public bool Laminate { get; set; }
        public bool DieCut { get; set; }
        public bool DesignBox { get; set; }
        public bool Board { get; set; }
    }
}
