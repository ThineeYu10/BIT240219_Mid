using BIT240219_Mid.Data;
using BIT240219_Mid.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MenuDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Menu") ??
        "Server=(localdb)\\MSSQLLocalDB;Database=BIT240219_MID;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"));
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MenuDbContext>();
    db.Database.EnsureCreated();
    if (!db.DishCategories.Any())
    {
        var cats = new[] { new DishCategory_BIT240219 { Name="Main Course", Description="Signature meals" }, new DishCategory_BIT240219 { Name="Dessert", Description="Sweet finishes" }, new DishCategory_BIT240219 { Name="Beverage", Description="Fresh drinks" } };
        db.DishCategories.AddRange(cats); db.SaveChanges();
        var dishes = new[] {
            new Dish_BIT240219 { Name="Grilled Salmon Bowl", Price=12.5m, PreparationTime=20, IsAvailable=true, Description="Charred salmon with jasmine rice and seasonal greens.", DishCategoryId=cats[0].Id },
            new Dish_BIT240219 { Name="Lemongrass Chicken", Price=9.9m, PreparationTime=15, IsAvailable=true, Description="Fragrant lemongrass chicken served with rice.", DishCategoryId=cats[0].Id },
            new Dish_BIT240219 { Name="Mango Sticky Rice", Price=6.5m, PreparationTime=10, IsAvailable=true, Description="Ripe mango, coconut cream and warm sticky rice.", DishCategoryId=cats[1].Id },
            new Dish_BIT240219 { Name="Chocolate Fondant", Price=7.25m, PreparationTime=18, IsAvailable=false, Description="Warm chocolate cake with a molten center.", DishCategoryId=cats[1].Id },
            new Dish_BIT240219 { Name="Vietnamese Iced Coffee", Price=4.5m, PreparationTime=5, IsAvailable=true, Description="Slow-dripped coffee with condensed milk.", DishCategoryId=cats[2].Id }
        }; db.Dishes.AddRange(dishes); db.SaveChanges();
        db.DishImages.AddRange(dishes.Select((d,i) => new DishImage_BIT240219 { DishId=d.Id, ImageUrl=$"https://images.unsplash.com/photo-{new[]{"1467003909585-2f8a72700288","1604908176997-125f25cc6f3d","1490645935967-10de6ba17061","1606313564200-e75d5e30476c","1517701604599-bb29b565090c"}[i]}?auto=format&fit=crop&w=900&q=85", IsThumbnail=true })); db.SaveChanges();
    }
}
if (!app.Environment.IsDevelopment()) { app.UseExceptionHandler("/Home/Error"); app.UseHsts(); }
app.UseHttpsRedirection(); app.UseStaticFiles(); app.UseRouting(); app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
