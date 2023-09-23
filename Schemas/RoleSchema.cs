using System.ComponentModel.DataAnnotations;

namespace project_backend.Schemas
{
    public class RolePrincipal
    {
        [Required(ErrorMessage = "El campo 'Rol' es requerido")]
        [MinLength(3, ErrorMessage = "El campo 'Rol' debe tener una longitud mínima de 3 caracteres")]
        [MaxLength(10, ErrorMessage = "El campo 'Rol' debe tener una longitud máxima de 10 caracteres")]
        public string RoleName { get; set; }
    }

    public class RoleGet : RolePrincipal
    {
        public int Id { get; set; }
    }
}
