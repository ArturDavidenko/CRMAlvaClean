namespace AlvaCleanCRM.Models.ViewModels
{
    public class CustomerViewModel
    {
        public string Id { get; set; }
        public string CustomerType { get; set; }
        public string ClientName { get; set; }
        public string ContactPhone { get; set; }
        public List<Order> Orders { get; set; }
    }
}
