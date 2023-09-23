namespace project_backend.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }

        public List<Employee> Employees { get; } = new List<Employee>();
    }
}
