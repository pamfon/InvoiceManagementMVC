
using System.ComponentModel.DataAnnotations;

namespace Invoice.DataAccess.Entities
{
    public class InvoiceLineItem
    {
        public int InvoiceLineItemId { get; set; }

        [Required(ErrorMessage = "Enter an amount")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public double? Amount { get; set; }

        [Required(ErrorMessage = "Enter a description")]
        public string? Description { get; set; }

        // FK:
        public int? InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }
    }
}
