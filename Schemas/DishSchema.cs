using System.ComponentModel.DataAnnotations;

namespace project_backend.Schemas
{
    public class DishPrincipal
    {
        [Required(ErrorMessage = "El campo 'Nombre Plato' es requerido")]
        [MinLength(3, ErrorMessage = "El campo 'Nombre Plato' debe tener una longitud mínima de 3 caracteres")]
        [MaxLength(50, ErrorMessage = "El campo 'Nombre Plato' debe tener una longitud máxima de 50 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo 'Precio Plato' es requerido")]
        [Range(0.1, 999, ErrorMessage = "El campo 'Precio Plato' debe ser un número decimal positivo no mayor a 999")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "El campo 'Imagen' es requerido")]
        public string Image { get; set; }
    }

    public class DishCreate : DishPrincipal
    {
        [Required(ErrorMessage = "El campo 'Id Categoría' es requerido")]
        public string CategoryId { get; set; }
    }

    public class DishGet : DishPrincipal
    {
        public string Id { get; set; }
        public CategoryGet Category { get; set; }
    }
}
