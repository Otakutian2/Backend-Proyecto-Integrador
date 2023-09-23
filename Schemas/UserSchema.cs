using System.ComponentModel.DataAnnotations;

namespace project_backend.Schemas
{
    public class UserPrincipal
    {
        [Required(ErrorMessage = "El campo 'Correo Electrónico' es requerido")]
        [EmailAddress(ErrorMessage = "El campo 'Correo Electrónico' debe tener un formato válido")]
        [MaxLength(50, ErrorMessage = "El campo 'Correo Electrónico' debe tener una longitud máxima de 50 caracteres")]
        public string Email { get; set; }
    }

    public class UserCreate : UserPrincipal
    {
        public string Password { get; set; }
    }
}
