using FreshHarvestMarket.Data;
using FreshHarvestMarket.Models;
using FreshHarvestMarket.OtherServices;
using FreshHarvestMarket.Repositories;
using FreshHarvestMarket.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IRepository<Discount>, FreshHarvestRepository<Discount>>();
builder.Services.AddScoped<IRepository<Produce>, FreshHarvestRepository<Produce>>();
builder.Services.AddScoped<IRepository<Favorite>, FreshHarvestRepository<Favorite>>();
builder.Services.AddScoped<IRepository<Order>, FreshHarvestRepository<Order>>();
builder.Services.AddScoped<IRepository<OrderItem>, FreshHarvestRepository<OrderItem>>();
builder.Services.AddScoped<IOrderFiltersSession, OrderFiltersSession>();
builder.Services.AddScoped<ICartService, CartService>();

//Add dependency injection for DbContext
builder.Services.AddDbContext<FreshHarvestContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("FreshHarvest")
));

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
})
    .AddEntityFrameworkStores<FreshHarvestContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

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


app.MapControllerRoute(
    name:"browseorder",
    pattern: "{controller=Order}/{action=Index}/IncludeActive/{IncludeActiveOrders=true}/IncludePast/{IncludePastOrders=true}/IncludeCancelled/{IncludeCancelledOrders=true}"
    );
    

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await FreshHarvestContext.CreateAdminUser(app.Services);

app.Run();
