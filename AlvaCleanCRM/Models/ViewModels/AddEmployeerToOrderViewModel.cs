namespace AlvaCleanCRM.Models.ViewModels
{
    public class AddEmployeerToOrderViewModel
    {
        public List<Employeer> Employeers { get; set; }

        public string SelectedEmployeerId { get; set; }
        public Order Order { get; set; }

    }
}
