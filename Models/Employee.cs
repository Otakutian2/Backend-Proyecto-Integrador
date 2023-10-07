using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_backend.Models
{
    [Index(nameof(Phone), IsUnique = true)]
    [Index(nameof(Dni), IsUnique = true)]
    public class Employee
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

        [Required]
        [MaxLength(12)]
        public string Phone { get; set; }

        [Column("created_at")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("role_id")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public User User { get; set; }

        public List<Command> CommandCollection { get; } = new List<Command>();

        public List<Receipt> ReceiptCollection { get; } = new List<Receipt>();
    }
}
