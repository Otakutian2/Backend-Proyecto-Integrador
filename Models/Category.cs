using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyecto_backend.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Category
    {
        [MaxLength(5)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public List<Dish> DishCollection { get; } = new List<Dish>();

        public static string GenerateId(List<Category> categoryCollection)
        {
            if (categoryCollection.Count == 0) return "C-001";

            string lastId = categoryCollection[^1].Id;

            int number = int.Parse(lastId.Split('-')[1]) + 1;

            return "C-" + number.ToString("000");
        }
    }
}
