# WebApplication2

ASP.NET Core MVC (.NET 8) sample app with Products and Categories CRUD, EF Core migrations, and Bootstrap UI.

## Requirements
- .NET SDK 8.0+
- SQL Server LocalDB (or update connection string)

## Getting Started
```bash
# Restore dependencies
 dotnet restore

# Apply migrations and run
 dotnet ef database update
 dotnet run
```

## Project Structure
- `Models/` domain entities and `AppDbContext`
- `Controllers/` MVC controllers
- `Views/` Razor views
- `ViewModels/` DTOs for forms
- `wwwroot/` static assets

## Migrations
```bash
# Add a migration
 dotnet ef migrations add <Name>

# Update database
 dotnet ef database update
```

## License
MIT
