using Invoice.DataAccess.Entities;

namespace InvoiceManagement.Web.Models
{
    public class CustomersViewModel
    {
        public List<Customer>? Customers { get;set; }

        public ICollection<string>? Categories { get; set; }

        public string? ActiveCategory;
    }
}
