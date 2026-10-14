using BIT240219_Mid.Data;
using BIT240219_Mid.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BIT240219_Mid.Controllers;
public class DishesController(MenuDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? search, int? categoryId, string? availability, decimal? minPrice, decimal? maxPrice, string sort="")
    {
        if(minPrice<0 || maxPrice<0 || (minPrice.HasValue&&maxPrice.HasValue&&minPrice>maxPrice)) ModelState.AddModelError("PriceRange","Enter a valid price range.");
        var q=db.Dishes.AsNoTracking().Include(d=>d.DishCategory).Include(d=>d.Images).AsQueryable();
        if(!string.IsNullOrWhiteSpace(search)) q=q.Where(d=>d.Name.Contains(search));
        if(categoryId.HasValue) q=q.Where(d=>d.DishCategoryId==categoryId);
        if(availability=="available") q=q.Where(d=>d.IsAvailable);
        if(availability=="unavailable") q=q.Where(d=>!d.IsAvailable);
        if(minPrice.HasValue) q=q.Where(d=>d.Price>=minPrice); if(maxPrice.HasValue) q=q.Where(d=>d.Price<=maxPrice);
        q=sort switch {"price-asc"=>q.OrderBy(d=>d.Price),"price-desc"=>q.OrderByDescending(d=>d.Price),"time-asc"=>q.OrderBy(d=>d.PreparationTime),_=>q.OrderBy(d=>d.Name)};
        return View(new DishListViewModel { Dishes=await q.ToListAsync(),Categories=await db.DishCategories.AsNoTracking().OrderBy(c=>c.Name).ToListAsync(),Search=search,CategoryId=categoryId,Availability=availability,MinPrice=minPrice,MaxPrice=maxPrice,Sort=sort });
    }
    public async Task<IActionResult> Details(int? id) { if(id is null)return NotFound(); var d=await db.Dishes.AsNoTracking().Include(x=>x.DishCategory).Include(x=>x.Images).FirstOrDefaultAsync(x=>x.Id==id); return d is null?NotFound():View(d); }
    public async Task<IActionResult> Create(){await Categories();return View(new Dish_BIT240219());}
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(Dish_BIT240219 dish,string? firstImage)
    { await ValidateDish(dish); if(!string.IsNullOrWhiteSpace(firstImage)&&(!Uri.TryCreate(firstImage,UriKind.Absolute,out var uri)||!(uri.Scheme=="https"||uri.Scheme=="http")))ModelState.AddModelError("firstImage","Enter a valid image URL."); if(!ModelState.IsValid){await Categories(dish.DishCategoryId);return View(dish);} db.Dishes.Add(dish); await db.SaveChangesAsync(); if(!string.IsNullOrWhiteSpace(firstImage)){db.DishImages.Add(new(){DishId=dish.Id,ImageUrl=firstImage,IsThumbnail=true});await db.SaveChangesAsync();} return RedirectToAction(nameof(Details),new{id=dish.Id}); }
    public async Task<IActionResult> Edit(int? id){if(id is null)return NotFound();var d=await db.Dishes.FindAsync(id);if(d is null)return NotFound();await Categories(d.DishCategoryId);return View(d);}
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id,Dish_BIT240219 dish)
    { if(id!=dish.Id)return NotFound(); if(!await db.Dishes.AnyAsync(x=>x.Id==id))return NotFound(); await ValidateDish(dish); if(!ModelState.IsValid){await Categories(dish.DishCategoryId);return View(dish);} db.Update(dish);await db.SaveChangesAsync();return RedirectToAction(nameof(Details),new{id}); }
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> AddImage(int id,string imageUrl){var d=await db.Dishes.Include(x=>x.Images).FirstOrDefaultAsync(x=>x.Id==id);if(d is null)return NotFound();if(!Uri.TryCreate(imageUrl,UriKind.Absolute,out var uri)||!(uri.Scheme=="https"||uri.Scheme=="http")){TempData["Error"]="Enter a valid image URL.";return RedirectToAction(nameof(Details),new{id});}db.DishImages.Add(new(){DishId=id,ImageUrl=imageUrl,IsThumbnail=!d.Images.Any(x=>x.IsThumbnail)});await db.SaveChangesAsync();return RedirectToAction(nameof(Details),new{id});}
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> SetThumbnail(int id,int imageId){var image=await db.DishImages.FirstOrDefaultAsync(x=>x.Id==imageId&&x.DishId==id);if(image is null)return NotFound();var images=await db.DishImages.Where(x=>x.DishId==id).ToListAsync();foreach(var x in images)x.IsThumbnail=x.Id==imageId;await db.SaveChangesAsync();return RedirectToAction(nameof(Details),new{id});}
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> DeleteImage(int id,int imageId){var im=await db.DishImages.FirstOrDefaultAsync(x=>x.Id==imageId&&x.DishId==id);if(im is null)return NotFound();db.DishImages.Remove(im);var next=await db.DishImages.FirstOrDefaultAsync(x=>x.DishId==id&&x.Id!=imageId);if(im.IsThumbnail&&next!=null)next.IsThumbnail=true;await db.SaveChangesAsync();return RedirectToAction(nameof(Details),new{id});}
    private async Task Categories(int? selected=null)=>ViewBag.Categories=new SelectList(await db.DishCategories.AsNoTracking().OrderBy(x=>x.Name).ToListAsync(),"Id","Name",selected);
    private async Task ValidateDish(Dish_BIT240219 d){if(!await db.DishCategories.AnyAsync(x=>x.Id==d.DishCategoryId))ModelState.AddModelError(nameof(d.DishCategoryId),"Choose an existing category.");if(await db.Dishes.AnyAsync(x=>x.DishCategoryId==d.DishCategoryId&&x.Name==d.Name&&x.Id!=d.Id))ModelState.AddModelError(nameof(d.Name),"A dish with this name already exists in this category.");}
}
