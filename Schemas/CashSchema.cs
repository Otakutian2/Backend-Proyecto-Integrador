using System.ComponentModel.DataAnnotations;

namespace proyecto_backend.Schemas
{
    public class CashPrincipal
    {
        [Required(ErrorMessage = "El campo 'Id Establecimiento' es requerido")]
        public int EstablishmentId { get; set; }
    }

    public class CashGet
    {
        public int Id { get; set; }
        public EstablishmentGet Establishment { get; set; }
    }
}
