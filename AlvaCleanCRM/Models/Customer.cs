namespace AlvaCleanCRM.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string ClientName { get; set; }
        public string CompanyName { get; set; }
        public string ContactPhone { get; set; }
        public List<Guid> Orders { get; set; }

    }
}
