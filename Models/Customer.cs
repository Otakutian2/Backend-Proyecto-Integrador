using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_backend.Models
{
    [Index(nameof(Dni), IsUnique = true)]
    public class Customer
    {
        public int Id { get; set; }

        [Column("first_name")]
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Column("last_name")]
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(8)]
        public string Dni { get; set; }

        public List<Receipt> ReceiptCollection { get; } = new List<Receipt>();
    }
}
