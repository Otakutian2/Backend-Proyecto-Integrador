using System.ComponentModel.DataAnnotations;

namespace proyecto_backend.Schemas
{
    public class PaymentMethodPrincipal
    {
        [Required(ErrorMessage = "El campo 'Método de Pago' es requerido")]
        [MinLength(3, ErrorMessage = "El campo Método de Pago' debe tener una longitud mínima de 3 caracteres")]
        [MaxLength(50, ErrorMessage = "El campo 'Método de Pago' debe tener una longitud máxima de 50 caracteres")]
        public string Name { get; set; }
    }

    public class PaymentMethodGet : PaymentMethodPrincipal
    {
        public int Id { get; set; }
    }
}
