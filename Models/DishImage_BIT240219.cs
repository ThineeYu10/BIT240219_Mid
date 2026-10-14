using System.ComponentModel.DataAnnotations;

namespace BIT240219_Mid.Models;

public class DishImage_BIT240219
{
    public int Id { get; set; }
    [Required, Url] public string ImageUrl { get; set; } = "";
    public bool IsThumbnail { get; set; }
    public int DishId { get; set; }
    public Dish_BIT240219? Dish { get; set; }
}
