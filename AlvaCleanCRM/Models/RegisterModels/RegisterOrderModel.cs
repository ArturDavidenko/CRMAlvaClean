namespace AlvaCleanCRM.Models.RegisterModels
{
    public class RegisterOrderModel
    {
        public List<string> CustomerName { get; set; }
        public string OrderType { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public string OrderPriceType { get; set; }
        public string? ClientComments { get; set; }
    }
}
