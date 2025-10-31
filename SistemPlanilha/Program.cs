using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemPlanilha.Application;
using SistemPlanilha.Data;
using SistemPlanilha.Domain.Services;
using SistemPlanilha.Models;
using SistemPlanilha.Repositorio;
using Microsoft.Extensions.Logging;
using System;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<BancoContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<ChecarSenhaExpirada>();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ChecarSenhaExpirada>();
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;           // Exige um número (0-9)
    options.Password.RequireLowercase = true;       // Exige uma letra minúscula
    options.Password.RequireUppercase = true;       // Exige uma letra maiúscula
    options.Password.RequireNonAlphanumeric = true; // Exige um caractere especial (!@#$%^&*)
    options.Password.RequiredLength = 8;            // Define o tamanho mínimo (ex: 8 caracteres)
    options.Password.RequiredUniqueChars = 1;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Tempo de bloqueio após falhas
    options.Lockout.MaxFailedAccessAttempts = 5;                      // Tentativas antes de bloquear
    options.Lockout.AllowedForNewUsers = true;

    // --- OPÇÕES DE USUÁRIO ---
    options.User.RequireUniqueEmail = true; // Garante que e-mails sejam únicos
})
    .AddEntityFrameworkStores<BancoContext>()
    .AddDefaultTokenProviders()
    .AddPasswordValidator<PreviousPasswordValidator<ApplicationUser>>();


builder.Services.ConfigureApplicationCookie(options =>
{
    
    options.LoginPath = "/Conta/Login";
    options.LogoutPath = "/Conta/Sair";
    options.AccessDeniedPath = "/Conta/AcessoNegado";
});


builder.Services.AddScoped<IInventarioRepositorio, InventarioRepositorio>();


builder.Services.AddScoped<IInventarioService, InventarioService>();
builder.Services.AddScoped<IManutencaoService, ManutencaoService>();


builder.Services.AddScoped<IInventarioApp, InventarioApp>();
builder.Services.AddScoped<IManutencaoApp, ManutencaoApp>();


builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

RotativaConfiguration.Setup(app.Environment.WebRootPath, "rotativa"); 
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await InicializadorDeDados.SeedData(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Um erro ocorreu ao popular os dados iniciais (Roles e Admin).");
    }
}
app.Run();