
using Castle.Core.Resource;
using Invoice.DataAccess.Entities;
using InvoiceManagement.DataAccess.Services;
using InvoiceManagement.Web.Controllers;
using InvoiceManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace InvoiceManagement.Tests
{
    public class InvoiceManagementControllerTests
    {
        private readonly Mock<IInvoiceManagementService> _invoiceManagementServiceMock = new Mock<IInvoiceManagementService>();
        [Fact]
        public void Customers_Get_ReturnCustomersViewModel()
        {
            // arrange
            var customers = new List<Customer>()
            {
                new Customer() { CustomerId = 1, Name = "Alfredo", IsDeleted = false },
                new Customer() { CustomerId = 2, Name = "Carlos", IsDeleted = false }
            };
            _invoiceManagementServiceMock.Setup(m => m.GetCustomerByCategory("A-E")).Returns(customers);
            var controller = new InvoiceManagementController(_invoiceManagementServiceMock.Object);

            // act
            var result = controller.Customers("A-E") as ViewResult;
            var customersViewModel = result?.Model as CustomersViewModel;

            //assert
            Assert.Equal(customers, customersViewModel?.Customers);
        }

        [Fact]
        public void EditCustomer_Post_CheckCategory()
        {
            // arrange
            var viewModel = new CustomerViewModel
            {
                Customer = new Customer { CustomerId = 1, Name = "Zezinho" },
                Category = null 
            };

            // mock service
            _invoiceManagementServiceMock.Setup(m => m.GetCustomerCategory("Zezinho")).Returns("S-Z");

            var controller = new InvoiceManagementController(_invoiceManagementServiceMock.Object)
            {
                TempData = new Mock<ITempDataDictionary>().Object // initialize tempdata
            };

            // Act
            controller.EditCustomer(viewModel); // use controller to define category

            // Assert
            Assert.Equal("S-Z", viewModel.Category); // Check if = S-Z
        }

        [Fact]
        public void DeleteCustomer_ReturnsRedirectToActionResult()
        {
            // Arrange
            var customer = new Customer { CustomerId = 1, Name = "Ana Paula" };

            _invoiceManagementServiceMock.Setup(s => s.GetCustomerById(1)).Returns(customer);
            _invoiceManagementServiceMock.Setup(s => s.GetCustomerCategory(customer.Name)).Returns("A-E");

            var controller = new InvoiceManagementController(_invoiceManagementServiceMock.Object)
            {
                TempData = new Mock<ITempDataDictionary>().Object
            };

            // Act
            var result = controller.DeleteCustomer(1);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Customers", ((RedirectToActionResult)result).ActionName);
        }
    }
}