using proyecto_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace proyecto_backend.Schemas
{
    public class TableRestaurantPrincipal
    {
        [Required(ErrorMessage = "El campo 'Cantidad de Asientos' es requerido")]
        [Range(1, 9, ErrorMessage = "El campo 'Cantidad de Asientos'  debe ser un número entero positivo no mayor a 9")]
        public int SeatCount { get; set; }
    }

    public class TableRestaurantUpdate : TableRestaurantPrincipal
    {
        [EnumDataType(typeof(TableStateEnum), ErrorMessage = "El campo 'Estado de Mesa' no es válido")]
        public TableStateEnum State { get; set; }
    }

    public class TableRestaurantGet : TableRestaurantPrincipal
    {
        public int Id { get; set; }
        public string State { get; set; }
    }

    public class TableRestaurantWithCommand
    {
        public TableRestaurantGet Table { get; set; }
        public CommandForTable Command { get; set; }
    }
}
