using BIT240219_Mid.Data;
using BIT240219_Mid.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BIT240219_Mid.Controllers;
public class CategoriesController(MenuDbContext db):Controller
{
 public async Task<IActionResult> Index()=>View(await db.DishCategories.AsNoTracking().Include(x=>x.Dishes).OrderBy(x=>x.Name).ToListAsync());
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id){var c=await db.DishCategories.Include(x=>x.Dishes).FirstOrDefaultAsync(x=>x.Id==id);if(c is null)return NotFound();if(c.Dishes.Any()){TempData["Error"]="This category still has dishes and cannot be deleted.";return RedirectToAction(nameof(Index));}db.Remove(c);await db.SaveChangesAsync();return RedirectToAction(nameof(Index));}
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(string name,string? description){if(string.IsNullOrWhiteSpace(name)){TempData["Error"]="Category name is required.";return RedirectToAction(nameof(Index));}name=name.Trim();if(await db.DishCategories.AnyAsync(c=>c.Name==name)){TempData["Error"]="A category with this name already exists.";return RedirectToAction(nameof(Index));}db.DishCategories.Add(new DishCategory_BIT240219{Name=name,Description=description});await db.SaveChangesAsync();return RedirectToAction(nameof(Index));}
}
