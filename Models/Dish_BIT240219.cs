using System.ComponentModel.DataAnnotations;

namespace BIT240219_Mid.Models;

public class Dish_BIT240219
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100)] public string Name { get; set; } = "";
    [Range(0.01, 999999, ErrorMessage = "Price must be greater than zero")]
    public decimal Price { get; set; }
    [Range(1, 600, ErrorMessage = "Preparation time must be greater than zero")]
    public int PreparationTime { get; set; }
    public bool IsAvailable { get; set; } = true;
    [StringLength(1000)] public string? Description { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Choose a category")]
    public int DishCategoryId { get; set; }
    public DishCategory_BIT240219? DishCategory { get; set; }
    public ICollection<DishImage_BIT240219> Images { get; set; } = new List<DishImage_BIT240219>();
}
