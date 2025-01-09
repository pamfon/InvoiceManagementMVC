using InvoiceManagement.DataAccess.Services;
using InvoiceManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Invoice.DataAccess.Entities;
using System.ComponentModel;

namespace InvoiceManagement.Web.Controllers
{
    public class InvoiceManagementController : Controller
    {
        private readonly IInvoiceManagementService _invoiceManagementService;
        public InvoiceManagementController(IInvoiceManagementService invoiceManagementService)
        {
            _invoiceManagementService = invoiceManagementService;
        }

        [HttpGet("/customers/{category?}")]
        public IActionResult Customers(string category)
        {
            List<Customer> customers;
            if (string.IsNullOrEmpty(category))
            {
                category = "A-E";
                customers = _invoiceManagementService.GetCustomerByCategory(category);
            }
            else
            {
                customers = _invoiceManagementService.GetCustomerByCategory(category);
            }
            var viewModel = new CustomersViewModel()
            {
                Categories = _invoiceManagementService.GetAllCategories(),
                ActiveCategory = category,
                Customers = customers
            };
            return View("Customer", viewModel);
        }

        [HttpGet("/customer/add")]
        public IActionResult AddCustomer()
        {
            var viewModel = new CustomerViewModel()
            {
                Customer = new Customer()
            };
            return View("Add", viewModel);
        }

        [HttpPost("/customer/add")]
        public IActionResult AddCustomer(CustomerViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Add", viewModel);
            }
            else
            {
                _invoiceManagementService.AddCustomer(viewModel.Customer);
                viewModel.Category = _invoiceManagementService.GetCustomerCategory(viewModel.Customer.Name);
                TempData["message"] = $"New Customer added: {viewModel.Customer.Name}";
                TempData["className"] = "success";
                //return RedirectToAction("Customers", new { customerId = viewModel.Customer.CustomerId });
                return RedirectToAction("ManageInvoices", new { customerId = viewModel.Customer.CustomerId });
            }
        }

        [HttpGet("/customer/{customerId}/edit")]
        public IActionResult EditCustomer(int customerId)
        {
            var activeCustomer = _invoiceManagementService.GetCustomerById(customerId);
            if (activeCustomer == null) { return NotFound(); }

            var viewModel = new CustomerViewModel()
            {
                Customer = activeCustomer,
                Category = _invoiceManagementService.GetCustomerCategory(activeCustomer.Name)
            };
            return View("Edit", viewModel);
        }

        [HttpPost("/customer/{customerId}/edit")]
        public IActionResult EditCustomer(CustomerViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", viewModel);
            }
            else
            {
                _invoiceManagementService.UpdateCustomer(viewModel.Customer);
                viewModel.Category = _invoiceManagementService.GetCustomerCategory(viewModel.Customer.Name);
                TempData["message"] = $"Customer updated: {viewModel.Customer.Name}";
                TempData["className"] = "info";
                //return RedirectToAction("Customers", new { customerId = viewModel.Customer.CustomerId });
                return RedirectToAction("ManageInvoices", new { customerId = viewModel.Customer.CustomerId, category = viewModel.Category});
            }
        }


        //delete customer
        [HttpGet("/customer/del/{customerId}")]
        public IActionResult DeleteCustomer(int customerId)
        {
            var customerToDelete = _invoiceManagementService.GetCustomerById(customerId);
            string Category = _invoiceManagementService.GetCustomerCategory(customerToDelete.Name);
            if (customerToDelete == null)
            {
                TempData["message"] = "Customer not found!";
                TempData["className"] = "danger";
                
                return RedirectToAction("Customers", new { category = Category });
            }

            _invoiceManagementService.SoftDeleteCustomer(customerToDelete);

            TempData["message"] = $"Customer deleted: {customerToDelete.Name}";
            TempData["className"] = "danger";
            TempData["undoCustomerId"] = customerToDelete.CustomerId;
            TempData["undoCustomerName"] = customerToDelete.Name;
            //TempData["category"] = _invoiceManagementService.GetCustomerCategory(customerToDelete.)
            return RedirectToAction("Customers", new { category = Category });
        }

        //undo delete costumer
        [HttpGet("/customer/undo-delete")]
        public IActionResult UndoDeleteCustomer()
        {
            string Category = null;
            if (TempData.ContainsKey("undoCustomerId"))
            {
                int customerId = (int)TempData["undoCustomerId"];
                string customerName = (string)TempData["undoCustomerName"];

                var customerToRestore = _invoiceManagementService.GetCustomerById(customerId);
                Category = _invoiceManagementService.GetCustomerCategory(customerToRestore.Name);
                if (customerToRestore != null)
                {
                    _invoiceManagementService.RestoreCustomer(customerToRestore);

                    TempData["message"] = $"Undo successful: {customerName} has been restored.";
                    TempData["className"] = "success";
                }
                else
                {
                    TempData["message"] = "Customer to undo not found!";
                    TempData["className"] = "danger";
                }

                // Clear undo information
                TempData.Remove("undoCustomerId");
                TempData.Remove("undoCustomerName");
            }
            else
            {
                TempData["message"] = "No recent deletions to undo.";
                TempData["className"] = "warning";
            }

            return RedirectToAction("Customers", new { category = Category });
        }



        //manage
        [HttpGet("/customer/{customerId}/manage-invoices")]
        public IActionResult ManageInvoices(int customerId)
        {
            var activeCustomer = _invoiceManagementService.GetCustomerById(customerId);

            if (activeCustomer == null) { return NotFound(); }

            var viewModel = new CustomerViewModel()
            {
                Customer = activeCustomer,
                Invoice = new Invoice.DataAccess.Entities.Invoice(),
                PaymentTerms = _invoiceManagementService.GetPaymentTerms(),
                InvoiceLineItems = null,
                Category = _invoiceManagementService.GetCustomerCategory(activeCustomer.Name)
            };
            return View("Manage", viewModel);
        }

        //add invoice
        [HttpPost("/customer/{customerId}/manage-invoices/add-invoice")]
        public IActionResult AddInvoice(int customerId, CustomerViewModel viewModel)
        {

            if (!ModelState.IsValid)
            {
                var activeCustomer = _invoiceManagementService.GetCustomerById(customerId);

                if (activeCustomer == null) { return NotFound(); }
                viewModel.Customer = activeCustomer;
                viewModel.PaymentTerms = _invoiceManagementService.GetPaymentTerms();
                return View("Manage", viewModel);
            }
            else
            {
                _invoiceManagementService.AddInvoiceToCustomer(customerId, viewModel.Invoice);
                return RedirectToAction("ManageInvoices", new { customerId });
            }
        }


        //manage invoices
        [HttpGet("/customer/{customerId}/manage-invoices/{invoiceId}/line-items")]
        public IActionResult ManageInvoice(int customerId, int invoiceId)
        {
            var activeCustomer = _invoiceManagementService.GetCustomerById(customerId);

            if (activeCustomer == null) { return NotFound(); }

            var viewModel = new CustomerViewModel()
            {
                Customer = activeCustomer,
                Invoice = _invoiceManagementService.GetInvoiceById(customerId, invoiceId),
                PaymentTerms = _invoiceManagementService.GetPaymentTerms(),
                InvoiceLineItems = _invoiceManagementService.GetInvoiceLineItemsByInvoiceId(invoiceId),
                Category = _invoiceManagementService.GetCustomerCategory(activeCustomer.Name)
            };
            return View("Manage", viewModel);
        }

        [HttpPost("/customer/{customerId}/manage-invoices/{invoiceId}/line-items")]
        public IActionResult AddInvoiceLineItem(int customerId, int invoiceId, CustomerViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var activeCustomer = _invoiceManagementService.GetCustomerById(customerId);
                if (activeCustomer == null) { return NotFound(); }
                var activeInvoice = _invoiceManagementService.GetInvoiceById(customerId, invoiceId);
                if (activeInvoice == null) { return NotFound(); }
                viewModel.Customer = activeCustomer;
                viewModel.Invoice = activeInvoice;
                viewModel.PaymentTerms = _invoiceManagementService.GetPaymentTerms();
                viewModel.InvoiceLineItems = _invoiceManagementService.GetInvoiceLineItemsByInvoiceId(invoiceId);
                viewModel.Category = _invoiceManagementService.GetCustomerCategory(activeCustomer.Name);
                return View("Manage", viewModel);
            }
            else
            {
                viewModel.Customer = _invoiceManagementService.GetCustomerById(customerId);
                viewModel.Invoice = _invoiceManagementService.GetInvoiceById(customerId, invoiceId);
                viewModel.PaymentTerms = _invoiceManagementService.GetPaymentTerms();
                viewModel.InvoiceLineItems = _invoiceManagementService.GetInvoiceLineItemsByInvoiceId(invoiceId);
                viewModel.Category = _invoiceManagementService.GetCustomerCategory(viewModel.Customer.Name);
                var newInvoiceLineItem = viewModel.InvoiceLineItem;
                _invoiceManagementService.AddInvoiceLineItemToInvoice(invoiceId, newInvoiceLineItem);
                return RedirectToAction("ManageInvoice", new { customerId, invoiceId });
            }
        }





    }
}
