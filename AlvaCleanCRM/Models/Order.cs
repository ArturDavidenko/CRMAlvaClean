namespace AlvaCleanCRM.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string OrderType { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public string OrderPriceType { get; set; }
        public string ClientComments { get; set; }
    }
}
