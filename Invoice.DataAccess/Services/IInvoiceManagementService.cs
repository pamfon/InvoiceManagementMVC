using Invoice.DataAccess.Entities;

namespace InvoiceManagement.DataAccess.Services
{
    public interface IInvoiceManagementService
    {
        public List<Customer> GetCustomerByCategory(string category);

        public ICollection<string> GetAllCategories();
        public Customer GetCustomerById(int customerId);
        public Invoice.DataAccess.Entities.Invoice GetInvoiceById(int customerId, int invoiceId);
        public int AddCustomer(Customer newCustomer);
        public int UpdateCustomer(Customer activeCostumer);
        public void AddInvoiceToCustomer(int customerId, Invoice.DataAccess.Entities.Invoice newInvoice);
        public void SoftDeleteCustomer(Customer customer);
        public List<PaymentTerms> GetPaymentTerms();
        public List<InvoiceLineItem> GetInvoiceLineItemsByInvoiceId(int invoiceId);
        public List<InvoiceLineItem> GetInvoiceLineItemsByInvoice(Invoice.DataAccess.Entities.Invoice invoice);
        public void AddInvoiceLineItemToInvoice(int invoiceId, InvoiceLineItem newInvoiceLineItem);

        public string GetCustomerCategory(string customerName);

        public void RestoreCustomer(Customer customerToRestore);
    }
}
