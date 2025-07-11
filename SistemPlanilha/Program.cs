using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;  // Para RelationalEventId
using Rotativa.AspNetCore;
using SistemPlanilha.Data;
using SistemPlanilha.Repositorio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BancoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database"))
        .LogTo(Console.WriteLine, LogLevel.Information)
           .ConfigureWarnings(warnings =>
               warnings.Ignore(RelationalEventId.PendingModelChangesWarning)));

builder.Services.AddScoped<IInventarioRepositorio, InventarioRepositorio>();

var app = builder.Build();

// Caminho da pasta que contém o wkhtmltopdf.exe
var wkhtmlPath = Path.Combine(app.Environment.ContentRootPath);
RotativaConfiguration.Setup(wkhtmlPath);

// Configura o Rotativa com o caminho correto
RotativaConfiguration.Setup(wkhtmlPath);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
