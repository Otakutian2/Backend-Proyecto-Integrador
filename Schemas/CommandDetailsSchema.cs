using System.ComponentModel.DataAnnotations;

namespace proyecto_backend.Schemas
{
    public class CommandDetailsPrincipal
    {
        [Required(ErrorMessage = "El campo 'Cantidad de Plato' es requerido")]
        [Range(1, 99, ErrorMessage = "El campo 'Cantidad de Plato' debe ser un número entero positivo no mayor a 99")]
        public int DishQuantity { get; set; }

        [MaxLength(150, ErrorMessage = "El campo 'Observación' debe tener una longitud máxima de 50 caracteres")]
        public string Observation { get; set; }
    }

    public class CommandDetailsCreate : CommandDetailsPrincipal
    {
        [Required(ErrorMessage = "El campo 'Id Plato' es requerido")]
        public string DishId { get; set; }
    }

    public class CommandDetailsGet : CommandDetailsPrincipal
    {
        public decimal OrderPrice { get; set; }
        public decimal DishPrice { get; set; }
        public DishGet Dish { get; set; }
    }
}
