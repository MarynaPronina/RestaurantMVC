using Microsoft.EntityFrameworkCore;
using RestaurantDomain.Model;
using RestaurantInfrastructure;

var builder = WebApplication.CreateBuilder(args);

// ?? 1. Add MVC and Razor support
builder.Services.AddControllersWithViews();

// ?? 2. Add EF Core DbContext
builder.Services.AddDbContext<RestaurantContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// ?? 3. Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // For HTTPS Strict Transport Security
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // To serve wwwroot and static files (e.g., CSS/JS/images)

app.UseRouting();
app.UseAuthorization();

// ?? 4. Setup default routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
