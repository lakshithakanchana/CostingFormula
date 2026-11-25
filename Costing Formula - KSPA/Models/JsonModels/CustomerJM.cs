namespace Costing_Formula___KSPA.Models.JsonModels
{
    public class CustomerJM
    {
        public int Id { get; set; } //CustomerId
        public string CustomerTypeDesc { get; set; }
        public string CurrencyCode { get; set; }
        public double? RateInLKR { get; set; }
        public string CustomerAddress { get; set; }
    }
}
