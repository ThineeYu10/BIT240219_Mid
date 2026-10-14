namespace BIT240219_Mid.Models;
public class DishListViewModel
{
    public List<Dish_BIT240219> Dishes { get; set; } = [];
    public List<DishCategory_BIT240219> Categories { get; set; } = [];
    public string? Search { get; set; }
    public int? CategoryId { get; set; }
    public string? Availability { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string Sort { get; set; } = "";
}
