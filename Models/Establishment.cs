using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace project_backend.Models
{
    [Index(nameof(Ruc), IsUnique = true)]
    public class Establishment
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(12)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(150)]
        public string Address { get; set; }

        [Required]
        [MaxLength(11)]
        public string Ruc { get; set; }

        public List<Cash> CashCollection { get; } = new List<Cash>();
    }
}
