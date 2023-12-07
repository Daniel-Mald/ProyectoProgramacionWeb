using BarbieQ.Models.Entities;
using BarbieQ.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<Repository<Producto>>();
builder.Services.AddTransient<Repository<Categoria>>();
builder.Services.AddTransient<Repository<Cliente>>();
builder.Services.AddTransient<ProductosRepository>();   
builder.Services.AddTransient<CategoriaRepository>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.AccessDeniedPath = "/Home/Denied";
    x.LoginPath = "/Home/Login";
    x.LogoutPath = "/Home/Logout";
    x.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    x.Cookie.Name = "BarbieQGalleta";
});


builder.Services.AddDbContext<Sistem21BarbieQcosmeticsContext>(x =>
x.UseMySql("server=sistemas19.com;database=sistem21_BarbieQCosmetics;user=sistem21_barbieqcosmetics;password=sistemas19_",
Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql")));
builder.Services.AddMvc();
var app = builder.Build();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );
app.MapDefaultControllerRoute();

app.Run();
