using System.ComponentModel.DataAnnotations.Schema;

namespace project_backend.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dni { get; set; }
        public string Phone { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime IssueDate { get; set; } = DateTime.Now;

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public User User { get; set; }

        public List<Commands> Commands { get; } = new List<Commands>();

        public List<Voucher> Vouchers { get; } = new List<Voucher>();
    }
}
