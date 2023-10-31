using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyecto_backend.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public int Code { get; set; }

        [Column("employee_id")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public static string GeneratePassword(Employee employee)
        {
            var rand = new Random();

            int charactersToExtract = 2;
            int randomNumber = rand.Next(1, employee.LastName.Length - charactersToExtract + 1);
            string extractedCharacters = employee.LastName.Substring(randomNumber, charactersToExtract);
            string capitalizedExtractedCharacters = string.Concat(extractedCharacters[..1].ToUpper(), extractedCharacters.AsSpan(1));

            return capitalizedExtractedCharacters + "$" + rand.Next(1000, 5001);
        }
    }
}
