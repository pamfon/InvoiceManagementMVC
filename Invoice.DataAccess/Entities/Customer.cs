
using System.ComponentModel.DataAnnotations;

namespace Invoice.DataAccess.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Address 1 is required.")]
        public string? Address1 { get; set; }

        
        public string? Address2 { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string? City { get; set; } = null!;

        [Required(ErrorMessage = "Province/State is required.")]
        [RegularExpression(@"^[A-Za-z]{2}$", ErrorMessage = "Invalid phone number format.")]
        public string? ProvinceOrState { get; set; } = null!;

        [Required(ErrorMessage = "Zip/Postal Code is required.")]
        [RegularExpression(@"^(?:[A-Za-z]\d[A-Za-z] ?\d[A-Za-z]\d|\d{5}(-\d{4})?)$", ErrorMessage = "Invalid Zip/Postal Code. Format: N2C 1L9 (Canada) or 12345 (USA).")]
        public string? ZipOrPostalCode { get; set; } = null!;


        //n2c 1l9
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{3}-?\d{3}-?\d{4}$", ErrorMessage = "Invalid phone number format: XXX-XXX-XXXX")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Contact's last name is required.")]
        public string? ContactLastName { get; set; }

        [Required(ErrorMessage = "Contact's first name is required.")]
        public string? ContactFirstName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? ContactEmail { get; set; }

        public bool IsDeleted { get; set; } = false;

        public List<Invoice>? Invoices { get; set; }

        public List<InvoiceLineItem>? LineItems { get; set; }
    }
}
