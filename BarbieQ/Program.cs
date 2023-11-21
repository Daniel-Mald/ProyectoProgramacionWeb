using BarbieQ.Models.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddDbContext<Sistem21BarbieQcosmeticsContext>(x =>
x.UseMySql("server=sistemas19.com;database=sistem21_BarbieQCosmetics;user=sistem21_barbieqcosmetics;password=sistemas19_",
Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql")));

var app = builder.Build();
app.UseStaticFiles();
app.MapDefaultControllerRoute();
app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );


app.Run();
