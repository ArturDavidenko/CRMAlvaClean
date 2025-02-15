namespace AlvaCleanCRM.Models.DTOs
{
    public class RegisterOrderDto
    {
        public string OrderType { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public string OrderPriceType { get; set; }
        public string ClientComments { get; set; }
    }
}
