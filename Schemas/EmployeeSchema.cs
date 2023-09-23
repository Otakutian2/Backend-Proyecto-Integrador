using System.ComponentModel.DataAnnotations;

namespace project_backend.Schemas
{
    public class EmployeePrincipal
    {
        [Required(ErrorMessage = "El campo 'Nombres' es requerido")]
        [MinLength(3, ErrorMessage = "El campo 'Nombres' debe tener una longitud mínima de 3 caracteres")]
        [MaxLength(50, ErrorMessage = "El campo 'Nombres' debe tener una longitud máxima de 50 caracteres")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El campo 'Apellidos' es requerido")]
        [MinLength(3, ErrorMessage = "El campo 'Apellidos' debe tener una longitud mínima de 3 caracteres")]
        [MaxLength(50, ErrorMessage = "El campo 'Apellidos' debe tener una longitud máxima de 50 caracteres")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El campo 'Teléfono' es requerido")]
        [Phone(ErrorMessage = "El campo 'Teléfono' debe almacenar un número válido")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "El campo 'Dni' es requerido")]
        public string Dni { get; set; }
    }

    public class EmployeeCreate : EmployeePrincipal
    {
        [Required(ErrorMessage = "El campo 'Rol' es requerido")]
        public int RoleId { get; set; }

        public UserPrincipal User { get; set; }
    }

    public class EmployeeUpdate : EmployeePrincipal
    {
        [Required(ErrorMessage = "El campo 'Rol' es requerido")]
        public int RoleId { get; set; }

        public UserPrincipal User { get; set; }
    }

    public class EmployeeGet : EmployeePrincipal
    {
        public int Id { get; set; }

        public RoleGet Role { get; set; }

        public DateTime CreatedAt { get; set; }

        public UserPrincipal User { get; set; }
    }
    public class EmployeGetCommand : EmployeePrincipal
    {
        public int Id { get; set; }
    }
}
