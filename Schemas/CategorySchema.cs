using System.ComponentModel.DataAnnotations;

namespace project_backend.Schemas
{
    public class CategoryPrincipal
    {
        [Required(ErrorMessage = "El campo 'Nombre' es requerido")]
        [MinLength(3, ErrorMessage = "El campo 'Nombre' debe tener una longitud mínima de 3 caracteres")]
        [MaxLength(50, ErrorMessage = "El campo 'Nombre' debe tener una longitud máxima de 50 caracteres")]
        public string Name { get; set; }
    }

    public class CategoryGet : CategoryPrincipal
    {
        public string Id { get; set; }
    }
}
