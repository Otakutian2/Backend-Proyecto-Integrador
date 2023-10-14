using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_backend.Models
{
    [Table("table_restaurant")]
    public class TableRestaurant
    {
        public int Id { get; set; }

        [Column("seat_count")]
        public int SeatCount { get; set; }

        [Required]
        [MaxLength(10)]
        public string State { get; set; }

        public List<Command> CommandCollection { get; } = new List<Command>();
    }
}
