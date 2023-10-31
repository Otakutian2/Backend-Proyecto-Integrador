using System.ComponentModel.DataAnnotations;

namespace proyecto_backend.Schemas
{
    public class ReceiptDetailsPrincipal
    {
        [Required(ErrorMessage = "El campo 'Monto' es requerido")]
        [Range(0.1, 999, ErrorMessage = "El campo 'Monto' debe ser un número decimal positivo no mayor a 999")]
        [RegularExpression(@"^\d{1,3}(.\d{1,2})?$", ErrorMessage = "El campo 'Monto' no tiene el formato correcto. Debe ser un número decimal de hasta 3 dígitos en la parte entera y hasta dos dígitos en la parte decimal")]
        public decimal Amount { get; set; }
    }

    public class ReceiptDetailsCreate : ReceiptDetailsPrincipal
    {
        [Required(ErrorMessage = "El campo 'Id Método de Pago' es requerido")]
        public int PaymentMethodId { get; set; }
    }

    public class ReceiptDetailsGet : ReceiptDetailsPrincipal
    {
        public int Id { get; set; }
        public PaymentMethodGet PaymentMethod { get; set; }
    }
}
