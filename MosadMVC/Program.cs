using Microsoft.EntityFrameworkCore;
using MosadMVC.Controllers;
using MosadMVC.DataContext;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<D_DbContext>(options =>
    options.UseNpgsql("Host=localhost;Port=5432;Database=mosad;Username=postgres;Password=1234"));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<HomeController>();


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=main_page}/{id?}");

app.Run();