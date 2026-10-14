using System.ComponentModel.DataAnnotations;

namespace BIT240219_Mid.Models;

public class DishCategory_BIT240219
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    [StringLength(80)] public string Name { get; set; } = "";
    [StringLength(300)] public string? Description { get; set; }
    public ICollection<Dish_BIT240219> Dishes { get; set; } = new List<Dish_BIT240219>();
}
