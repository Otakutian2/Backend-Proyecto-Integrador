using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyecto_backend.Models
{
    [Table("command_details")]
    public class CommandDetails
    {
        public int Id { get; set; }

        [Column("dish_quantity")]
        public int DishQuantity { get; set; }

        [Column("dish_price")]
        [Precision(5, 2)]
        public decimal DishPrice { get; set; }

        [Column("order_price")]
        [Precision(6, 2)]
        public decimal OrderPrice { get; set; }

        [MaxLength(150)]
        public string Observation { get; set; }

        [Column("command_id")]
        public int CommandId { get; set; }
        public Command Command { get; set; }

        [Column("dish_id")]
        [Required]
        public string DishId { get; set; }
        public Dish Dish { get; set; }
    }
}
