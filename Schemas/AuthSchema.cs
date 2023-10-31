using System.ComponentModel.DataAnnotations;

namespace proyecto_backend.Schemas
{
    public class AuthRequest
    {
        [Required(ErrorMessage = "El campo 'Correo Electrónico' es requerido")]
        [EmailAddress(ErrorMessage = "El campo 'Correo Electrónico' no es una dirección de correo electrónico válida")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo 'Contraseña' es requerido")]
        public string Password { get; set; }
    }

    public class AuthResponse
    {
        public string AccessToken { get; set; }
    }
}
