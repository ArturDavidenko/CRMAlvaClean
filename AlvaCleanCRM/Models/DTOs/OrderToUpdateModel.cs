namespace AlvaCleanCRM.Models.DTOs
{
    public class OrderToUpdateModel
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string CurrentCustomerName { get; set; }
        public string? OrderType { get; set; }
        public string? Status { get; set; }
        public string? Address { get; set; }
        public string? OrderPriceType { get; set; }
        public string? ClientComments { get; set; }
        public double Price { get; set; }
        public double Hour { get; set; }
        public double Area { get; set; }
        public DateTime OrderStartDate { get; set; }
        public List<string> CustomersList { get; set; }
    }
}
