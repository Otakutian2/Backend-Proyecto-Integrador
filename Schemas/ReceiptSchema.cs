using System.ComponentModel.DataAnnotations;

namespace project_backend.Schemas
{
    public class ReceiptPrincipal
    {
        [Required(ErrorMessage = "El campo 'Descuento' es requerido")]
        [Range(0, 999, ErrorMessage = "El campo 'Descuento' debe ser un número decimal positivo no mayor a 999")]
        [RegularExpression(@"^\d{1,3}(.\d{1,2})?$", ErrorMessage = "El campo 'Descuento' no tiene el formato correcto. Debe ser un número decimal de hasta 3 dígitos en la parte entera y hasta dos dígitos en la parte decimal")]
        public decimal Discount { get; set; }

        [Required(ErrorMessage = "El campo 'Monto Adicional' es requerido")]
        [Range(0, 999, ErrorMessage = "El campo 'Monto Adicional' debe ser un número decimal positivo no mayor a 999")]
        [RegularExpression(@"^\d{1,3}(.\d{1,2})?$", ErrorMessage = "El campo 'Monto Adicional' no tiene el formato correcto. Debe ser un número decimal de hasta 3 dígitos en la parte entera y hasta dos dígitos en la parte decimal")]
        public decimal AdditionalAmount { get; set; }
    }

    public class ReceiptCreate : ReceiptPrincipal
    {
        [Required(ErrorMessage = "El campo 'Id Comanda' es requerido")]
        public int CommandId { get; set; }

        [Required(ErrorMessage = "El campo 'Id Tipo de Comprobante' es requerido")]
        public int ReceiptTypeId { get; set; }

        public int? CustomerId { get; set; }

        [Required(ErrorMessage = "El campo 'Id Caja' es requerido")]
        public int CashId { get; set; }

        [Required(ErrorMessage = "Los detalles del comprobante son requeridos")]
        [MinLength(1, ErrorMessage = "El detalle del comprobante debe tener al menos un elemento")]
        public List<ReceiptDetailsCreate> ReceiptDetailsCollection { get; set; }
    }

    public class ReceiptGet : ReceiptPrincipal
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Igv { get; set; }
        public DateTime CreatedAt { get; set; }
        public CommandGet Command { get; set; }
        public CustomerGet Customer { get; set; }
        public ReceiptTypeGet ReceiptType { get; set; }
        public EmployeeGet Employee { get; set; }
        public CashGet Cash { get; set; }

        public List<ReceiptDetailsGet> ReceiptDetailsCollection { get; set; }
    }
}
