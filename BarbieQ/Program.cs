using BarbieQ.Models.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddDbContext<BarbieAndCosmeticsContext>(x=>
x.UseMySql("server=localhost;password=root;user=root;database=BarbieAndCosmetics",
Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql")));

var app = builder.Build();
app.UseStaticFiles();
app.MapDefaultControllerRoute();
app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );


app.Run();
