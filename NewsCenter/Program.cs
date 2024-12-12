using Microsoft.EntityFrameworkCore;
using NewsCenter.Models.Entities;
using NewsCenter.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();

string CadenaConexion = "server=localhost;user=root;database=newscenter;password=root";


builder.Services.AddDbContext<NewscenterContext>(
    optionBuilder => optionBuilder.UseMySql(CadenaConexion,
    ServerVersion.AutoDetect(CadenaConexion)));

builder.Services.AddTransient<NoticiaRepository>();
builder.Services.AddTransient<Repository<Ciudades>>();
builder.Services.AddTransient<Repository<Autores>>();


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
