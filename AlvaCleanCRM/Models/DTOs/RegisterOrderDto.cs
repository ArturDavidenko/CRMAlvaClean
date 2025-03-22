namespace AlvaCleanCRM.Models.DTOs
{
    public class RegisterOrderDto
    {
        public string OrderType { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public double Price { get; set; }
        public string OrderPriceType { get; set; }
        public string? ClientComments { get; set; }
        public double Hour { get; set; }
        public double Area { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderStartDate { get; set; }
    }
}
