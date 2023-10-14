using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_backend.Models
{
    public class Receipt
    {
        public int Id { get; set; }

        [Column("created_at")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("total_price")]
        [Precision(6, 2)]
        public decimal TotalPrice { get; set; }

        [Precision(5, 2)]
        public decimal Igv { get; set; }

        [Precision(5, 2)]
        public decimal AdditionalAmount { get; set; }

        [Precision(5, 2)]
        public decimal Discount { get; set; }

        [Column("command_id")]
        public int CommandId { get; set; }
        public Command Command { get; set; }

        [Column("customer_id")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [Column("receipt_type_id")]
        public int ReceiptTypeId { get; set; }
        public ReceiptType ReceiptType { get; set; }

        [Column("employee_id")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Column("cash_id")]
        public int CashId { get; set; }
        public Cash Cash { get; set; }

        public List<ReceiptDetails> ReceiptDetailsCollection { get; set; } = new List<ReceiptDetails>();
    }
}
