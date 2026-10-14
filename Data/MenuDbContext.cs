using BIT240219_Mid.Models;
using Microsoft.EntityFrameworkCore;

namespace BIT240219_Mid.Data;

public class MenuDbContext(DbContextOptions<MenuDbContext> options) : DbContext(options)
{
    public DbSet<DishCategory_BIT240219> DishCategories => Set<DishCategory_BIT240219>();
    public DbSet<Dish_BIT240219> Dishes => Set<Dish_BIT240219>();
    public DbSet<DishImage_BIT240219> DishImages => Set<DishImage_BIT240219>();
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<DishCategory_BIT240219>(e => { e.ToTable("BIT240219_DishCategories"); e.HasKey(x=>x.Id); e.Property(x=>x.Id).HasColumnName("BIT240219_Id"); e.Property(x=>x.Name).HasColumnName("BIT240219_Name").HasMaxLength(80).IsRequired(); e.Property(x=>x.Description).HasColumnName("BIT240219_Description").HasMaxLength(300); e.HasIndex(x=>x.Name).IsUnique(); });
        b.Entity<Dish_BIT240219>(e => { e.ToTable("BIT240219_Dishes"); e.HasKey(x=>x.Id); e.Property(x=>x.Id).HasColumnName("BIT240219_Id"); e.Property(x=>x.Name).HasColumnName("BIT240219_Name").HasMaxLength(100).IsRequired(); e.Property(x=>x.Price).HasColumnName("BIT240219_Price").HasPrecision(10,2); e.Property(x=>x.PreparationTime).HasColumnName("BIT240219_PreparationTime"); e.Property(x=>x.IsAvailable).HasColumnName("BIT240219_IsAvailable"); e.Property(x=>x.Description).HasColumnName("BIT240219_Description").HasMaxLength(1000); e.Property(x=>x.DishCategoryId).HasColumnName("BIT240219_DishCategoryId"); e.HasIndex(x=>new{x.DishCategoryId,x.Name}).IsUnique(); e.HasOne(x=>x.DishCategory).WithMany(x=>x.Dishes).HasForeignKey(x=>x.DishCategoryId).OnDelete(DeleteBehavior.Restrict); });
        b.Entity<DishImage_BIT240219>(e => { e.ToTable("BIT240219_DishImages"); e.HasKey(x=>x.Id); e.Property(x=>x.Id).HasColumnName("BIT240219_Id"); e.Property(x=>x.ImageUrl).HasColumnName("BIT240219_ImageUrl").HasMaxLength(1000).IsRequired(); e.Property(x=>x.IsThumbnail).HasColumnName("BIT240219_IsThumbnail"); e.Property(x=>x.DishId).HasColumnName("BIT240219_DishId"); e.HasOne(x=>x.Dish).WithMany(x=>x.Images).HasForeignKey(x=>x.DishId).OnDelete(DeleteBehavior.Cascade); e.HasIndex(x=>x.DishId).IsUnique().HasFilter("[BIT240219_IsThumbnail] = 1"); });
    }
}
