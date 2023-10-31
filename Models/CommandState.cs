using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyecto_backend.Models
{
    [Table("command_state")]
    [Index(nameof(Name), IsUnique = true)]
    public class CommandState
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public List<Command> CommandCollection { get; } = new List<Command>();
    }
}
