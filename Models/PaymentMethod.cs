
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyecto_backend.Models
{
    [Table("payment_method")]
    [Index(nameof(Name), IsUnique = true)]
    public class PaymentMethod
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public List<ReceiptDetails> ReceiptDetailsCollection { get; } = new List<ReceiptDetails>();
    }
}
