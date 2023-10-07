using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace project_backend.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Role
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public List<Employee> EmployeeCollection { get; } = new List<Employee>();
    }
}
