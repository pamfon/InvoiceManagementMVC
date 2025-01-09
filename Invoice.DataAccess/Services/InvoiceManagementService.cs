

using Invoice.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Invoice.DataAccess.Entities;
using Microsoft.IdentityModel.Tokens;

namespace InvoiceManagement.DataAccess.Services
{
    public class InvoiceManagementService : IInvoiceManagementService
    {
        private readonly InvoiceDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public InvoiceManagementService(InvoiceDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        private readonly string[] _categories = ["A-E", "F-K", "L-R", "S-Z"];

        public ICollection<string> GetAllCategories()
        {
            return _categories.AsReadOnly();
        }

        public List<Customer> GetCustomerByCategory(string category)
        {
            List<Customer> customers = new List<Customer>();

            if (category == "A-E" || string.IsNullOrEmpty(category))
            {   
                customers = _dbContext.Customers.Where(c => !c.IsDeleted && (c.Name.StartsWith("A") || c.Name.StartsWith("B")
                || c.Name.StartsWith("C") || c.Name.StartsWith("D")
                || c.Name.StartsWith("E"))).OrderBy(c => c.Name).ToList();
            }
            else if (category == "F-K")
            {
                customers = _dbContext.Customers.Where(c => !c.IsDeleted && (c.Name.StartsWith("F") || c.Name.StartsWith("G")
                || c.Name.StartsWith("H") || c.Name.StartsWith("I") || c.Name.StartsWith("J")
                || c.Name.StartsWith("K"))).OrderBy(c => c.Name).ToList();
            }
            else if (category == "L-R")
            {
                customers = _dbContext.Customers.Where(c => !c.IsDeleted && (c.Name.StartsWith("L") || c.Name.StartsWith("M")
                || c.Name.StartsWith("N") || c.Name.StartsWith("O") || c.Name.StartsWith("P") || c.Name.StartsWith("Q")
                || c.Name.StartsWith("R"))).OrderBy(c => c.Name).ToList();
            }
            else if (category == "S-Z")
            {
                customers = _dbContext.Customers.Where(c => !c.IsDeleted && (c.Name.StartsWith("S") || c.Name.StartsWith("T")
                || c.Name.StartsWith("U") || c.Name.StartsWith("V") || c.Name.StartsWith("X") || c.Name.StartsWith("Z") || c.Name.StartsWith("Y")
                || c.Name.StartsWith("W"))).OrderBy(c => c.Name).ToList();
            }

            return customers;
        }



        public Customer GetCustomerById(int customerId)
        {
            var customer = _dbContext.Customers.Include(c => c.Invoices).FirstOrDefault(c => c.CustomerId == customerId);
            return customer;
        }

        public Invoice.DataAccess.Entities.Invoice GetInvoiceById(int customerId, int invoiceId)
        {
            var customer = _dbContext.Customers
                .Include(c => c.Invoices)
                .FirstOrDefault(c => c.CustomerId == customerId);

            return customer?.Invoices.FirstOrDefault(s => s.InvoiceId == invoiceId);
        }

        public int AddCustomer(Customer newCustomer)
        {
            _dbContext.Customers.Add(newCustomer);
            _dbContext.SaveChanges();
            return newCustomer.CustomerId;
        }

        public int UpdateCustomer(Customer activeCustomer)
        {
            _dbContext.Customers.Update(activeCustomer);
            _dbContext.SaveChanges();
            return activeCustomer.CustomerId;
        }

        public void AddInvoiceToCustomer(int customerId, Invoice.DataAccess.Entities.Invoice newInvoice)
        {
            var activeCustomer = GetCustomerById(customerId);
            if (activeCustomer == null) { return; }
            activeCustomer.Invoices.Add(newInvoice);
            _dbContext.SaveChanges();
        }

        public void SoftDeleteCustomer(Customer customerToDelete)
        {
            customerToDelete.IsDeleted = true;
            _dbContext.Customers.Update(customerToDelete);
            _dbContext.SaveChanges();
        }

        public List<PaymentTerms> GetPaymentTerms()
        {
            return _dbContext.PaymentTerms.ToList();  
        }

        public List<InvoiceLineItem> GetInvoiceLineItemsByInvoiceId(int invoiceId){ 
            return _dbContext.InvoiceLineItems.Where(i => i.InvoiceId == invoiceId).ToList();
        }

        public List<InvoiceLineItem> GetInvoiceLineItemsByInvoice(Invoice.DataAccess.Entities.Invoice invoice)
        {
            return null;
        }

        public void AddInvoiceLineItemToInvoice(int invoiceId, InvoiceLineItem newInvoiceLineItem)
        {
            newInvoiceLineItem.InvoiceId = invoiceId;
            _dbContext.InvoiceLineItems.Add(newInvoiceLineItem);
            _dbContext.SaveChanges();
        }

        public string GetCustomerCategory(string customerName)
        {
            char firstLetter = char.ToUpper(customerName[0]);
            if (firstLetter >= 'A' && firstLetter <= 'E') return "A-E";
            if (firstLetter >= 'F' && firstLetter <= 'K') return "F-K";
            if (firstLetter >= 'L' && firstLetter <= 'R') return "L-R";
            if (firstLetter >= 'S' && firstLetter <= 'Z') return "S-Z";
            return "A-E";
        }

        public void RestoreCustomer(Customer customerToRestore)
        {
            customerToRestore.IsDeleted = false;
            _dbContext.SaveChanges();
        }

    }
}
