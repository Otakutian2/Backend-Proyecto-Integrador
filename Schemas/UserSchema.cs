using System.ComponentModel.DataAnnotations;

namespace proyecto_backend.Schemas
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

    public class VerifyEmail
    {
        [Required(ErrorMessage = "El campo 'Correo Electrónico' es requerido")]
        [EmailAddress(ErrorMessage = "El campo 'Correo Electrónico' debe tener un formato válido")]
        [MaxLength(50, ErrorMessage = "El campo 'Correo Electrónico' debe tener una longitud máxima de 50 caracteres")]
        public string Email { get; set; }
    }

    public class VerifyCode : VerifyEmail
    {
        [Required(ErrorMessage = "El campo 'Código' es requerido")]
        public int Code { get; set; }
    }

    public class ChangePassword : VerifyCode
    {
        [Required(ErrorMessage = "El campo 'Nueva Contraseña' es requerida")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])([A-Za-z\d$@$!%*?&]|[^ ]){7,25}$", ErrorMessage = "Debe ser una contraseña segura")]
        public string NewPassword { get; set; }
    }
}
