using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyecto_backend.Models
{
    [Table("receipt_details")]
    public class ReceiptDetails
    {
        public int Id { get; set; }

        [Precision(5, 2)]
        public decimal Amount { get; set; }

        [Column("receipt_id")]
        public int ReceiptId { get; set; }
        public Receipt Receipt { get; set; }

        [Column("payment_method_id")]
        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
