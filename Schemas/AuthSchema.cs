using System.ComponentModel.DataAnnotations;

namespace project_backend.Schemas
{
    public class AuthRequest
    {
        [Required(ErrorMessage = "El campo 'Correo Electrónico' es requerido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo 'Contraseña' es requerido")]
        public string Password { get; set; }
    }

    public class AuthResponse
    {
        public string AccessToken { get; set; }
    }
}
