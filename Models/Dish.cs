using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyecto_backend.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Dish
    {
        [MaxLength(5)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Precision(5, 2)]
        public decimal Price { get; set; }

        [Required]
        public string Image { get; set; }

        [Column("category_id")]
        [Required]
        public string CategoryId { get; set; }
        public Category Category { get; set; }

        public List<CommandDetails> CommandDetailsCollection { get; } = new List<CommandDetails>();

        public static string GenerateId(List<Dish> dishCollection)
        {
            if (dishCollection.Count == 0) return "D-001";

            string lastId = dishCollection[^1].Id;

            int number = int.Parse(lastId.Split('-')[1]) + 1;

            return "D-" + number.ToString("000");
        }
    }
}
