using System.ComponentModel.DataAnnotations;

namespace project_backend.Schemas
{
    public class CommandPrincipal
    {
        [Range(1, 9, ErrorMessage = "El campo 'Cantidad de Asientos' debe ser un número entero positivo no mayor a 9")]
        public int? SeatCount { get; set; }
    }

    public class CommandCreate : CommandPrincipal
    {
        public int? TableRestaurantId { get; set; }

        [Required(ErrorMessage = "Los platos de la comanda son requeridas")]
        [MinLength(1, ErrorMessage = "La comanda debe tener al menos un plato")]
        public List<CommandDetailsCreate> CommandDetailsCollection { get; set; }
    }

    public class CommandUpdate : CommandPrincipal
    {
        [Required(ErrorMessage = "Los platos de la comanda son requeridas")]
        [MinLength(1, ErrorMessage = "La comanda debe tener al menos un plato")]
        public List<CommandDetailsCreate> CommandDetailsCollection { get; set; }
    }

    public class CommandGet : CommandPrincipal
    {
        public int Id { get; set; }
        public decimal TotalOrderPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public TableRestaurantGet? TableRestaurant { get; set; }
        public CommandStateGet CommandState { get; set; }
        public EmployeeGet Employee { get; set; }
        public List<CommandDetailsGet> CommandDetailsCollection { get; set; } = new List<CommandDetailsGet>();
    }

    public class CommandForTable : CommandPrincipal
    {
        public int Id { get; set; }
        public decimal TotalOrderPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public CommandStateGet CommandState { get; set; }
        public EmployeeGet Employee { get; set; }
        public decimal QuantityOfDish { get; set; }
    }
}
