
using System.ComponentModel.DataAnnotations;

namespace Invoice.DataAccess.Entities
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        [Required(ErrorMessage = "You need to enter a date to your invoice")]
        public DateTime? InvoiceDate { get; set; }

        public DateTime? InvoiceDueDate
        {
            get
            {
                return InvoiceDate?.AddDays(Convert.ToDouble(PaymentTerm?.DueDays));
            }
        }

        public double? PaymentTotal { get; set; } = 0.0;

        public DateTime? PaymentDate { get; set; }

        // FK:
        [Required(ErrorMessage = "Enter a Payment Term")]
        [Range(1, 5, ErrorMessage = "Enter a valid Payment Term")]
        public int PaymentTermsId { get; set; }
        public PaymentTerms? PaymentTerm { get; set; }

        // FK:
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

    }
}
