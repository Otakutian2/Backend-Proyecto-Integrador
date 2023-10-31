using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyecto_backend.Models
{
    [Table("receipt_type")]
    [Index(nameof(Name), IsUnique = true)]
    public class ReceiptType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public List<Receipt> ReceiptCollection { get; } = new List<Receipt>();
    }
}
