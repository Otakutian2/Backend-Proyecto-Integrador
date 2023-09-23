using System.ComponentModel.DataAnnotations;

namespace project_backend.Schemas
{
    public class EstablishmentPrincipal
    {
        [Required(ErrorMessage = "El campo 'Nombre' es requerido")]
        [MinLength(3, ErrorMessage = "El campo 'Nombre' debe tener una longitud mínima de 3 caracteres")]
        [MaxLength(50, ErrorMessage = "El campo 'Nombre' debe tener una longitud máxima de 50 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo 'Teléfono' es requerido")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "El campo 'Dirección' es requerido")]
        public string Address { get; set; }

        [Required(ErrorMessage = "El campo 'RUC' es requerido")]
        public string Ruc { get; set; }
    }

    public class EstablishmentGet : EstablishmentPrincipal
    {
        public int Id { get; set; }
    }
}
