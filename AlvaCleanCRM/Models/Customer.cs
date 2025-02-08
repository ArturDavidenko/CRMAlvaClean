namespace AlvaCleanCRM.Models
{
    public class Customer
    {
        public string Id { get; set; }
        public string CustomerType { get; set; }
        public string ClientName { get; set; }
        public string CompanyName { get; set; }
        public string ContactPhone { get; set; }
        public List<string> Orders { get; set; }

    }
}
