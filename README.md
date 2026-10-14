# Savor | BIT240219

ASP.NET Core MVC + Entity Framework Core web app for managing dishes, categories and dish photos.

## Requirements

- .NET 10 SDK
- SQL Server LocalDB (the default Visual Studio installation) or a SQL Server instance

## Run

1. Open this folder in Visual Studio, or run `dotnet restore` and `dotnet run` from this folder.
2. On first launch the app creates the `BIT240219_MID` database and the three MSSV-prefixed tables, then inserts 3 categories and 5 example dishes.
3. Browse to the HTTPS URL shown in the terminal. The first visit uses English; use the language button in the header to switch to Vietnamese. The choice stays for the browser session.

To use another SQL Server, update `ConnectionStrings:Menu` in `appsettings.json`. The database name and every physical table/column name stay prefixed with `BIT240219`.

## Included

- Models `DishCategory_BIT240219`, `Dish_BIT240219`, `DishImage_BIT240219` and EF relationships with prefixed SQL identifiers.
- Search, category and availability filters, price range and sorting composed as EF queries, so filtering runs in SQL.
- Create/edit validation, unique dish name within category, active/inactive status, and category deletion protection.
- Multiple image URLs per dish, cover selection, and empty-image handling.
- English default and Vietnamese/English interface toggle with session persistence.

## Demo checklist

1. Show database `BIT240219_MID`, tables `BIT240219_DishCategories`, `BIT240219_Dishes`, `BIT240219_DishImages`, their column names, and the three MSSV-suffixed C# models.
2. Demonstrate combined search, category, status, price and sort filters; preserve filters on Apply.
3. Create/edit a dish and show Quick (15 minutes or less) / Regular classification.
4. Add a second photo, change the cover photo, and verify all photos on Details.
5. Attempt to delete a category that has dishes and open a nonexistent dish URL.
6. Start a fresh browser session in English, switch to Vietnamese, and visit List, Create, Edit and Details; submit invalid form values to show validation.
