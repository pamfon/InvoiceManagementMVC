using Invoice.DataAccess.Entities;

namespace InvoiceManagement.Web.Models
{
    public class CustomerViewModel
    {
        public Customer? Customer { get; set; }
        public Invoice.DataAccess.Entities.Invoice? Invoice {  get; set; }

        public List<PaymentTerms>? PaymentTerms { get; set; }

        public List<InvoiceLineItem>? InvoiceLineItems { get; set; }

        public InvoiceLineItem? InvoiceLineItem { get; set; }

        public string? Category { get; set; }
    }
}
