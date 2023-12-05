using BarbieQ.Models.Entities;
using BarbieQ.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<Repository<Producto>>();
builder.Services.AddTransient<Repository<Categoria>>();
builder.Services.AddTransient<ProductosRepository>();   


builder.Services.AddDbContext<Sistem21BarbieQcosmeticsContext>(x =>
x.UseMySql("server=sistemas19.com;database=sistem21_BarbieQCosmetics;user=sistem21_barbieqcosmetics;password=sistemas19_",
Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql")));
builder.Services.AddMvc();
var app = builder.Build();
app.UseStaticFiles();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );
app.MapDefaultControllerRoute();

app.Run();
