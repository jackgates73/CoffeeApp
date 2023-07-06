using Ecommerce.Interfaces;
using Ecommerce.Models;
using Ecommerce.Repository;
using Ecommerce.Services;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddScoped(sp => BasketService.GetBasket(sp));
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
//builder.Services.AddScoped<IBasketRepository, basketRepository>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IRepository<Product>, SQLRepository<Product>>();
builder.Services.AddScoped<IRepository<BasketItem>, SQLRepository<BasketItem>>();
builder.Services.AddScoped<IRepository<Basket>, SQLRepository<Basket>>();
builder.Services.AddScoped<BasketSummaryViewComponent>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Set secure flag
    options.Cookie.HttpOnly = true; // Set httpOnly flag, this seems to be set by default
    options.Cookie.Name = "X-CSRF-TOKEN";
    options.Cookie.Expiration = TimeSpan.FromDays(1);
});
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "Session-Cookie";
    options.Cookie.MaxAge = TimeSpan.FromDays(1);
});
builder.Services.Configure<IdentityOptions>(options =>
{
    // Modify the password requirements
    options.Password.RequiredLength = 8; // Set your desired minimum password length
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false; // Set to false to remove the requirement for a special character
});

builder.Services.AddAuthentication(options =>
{
    // Access the AuthenticationOptions object
    var authenticationOptions = options;
    // Use authenticationOptions to inspect the configured schemes
    // ...
});

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    await Seed.SeedUsersAndRolesAsync(app);
     Seed.SeedData(app);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' https://cdnjs.cloudflare.com; img-src 'self'  https: data:; font-src 'self' https://cdnjs.cloudflare.com;");
    await next.Invoke();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
