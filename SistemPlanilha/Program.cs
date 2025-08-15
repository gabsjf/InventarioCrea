using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemPlanilha.Application;
using SistemPlanilha.Data;
using SistemPlanilha.Domain.Services;
using SistemPlanilha.Repositorio;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<BancoContext>(options => options.UseSqlServer(connectionString));


builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4; // Apenas para desenvolvimento!
})
    .AddEntityFrameworkStores<BancoContext>();

// --- CONFIGURA��O DAS ROTAS DE LOGIN CUSTOMIZADAS ---
builder.Services.ConfigureApplicationCookie(options =>
{
    // Informa ao sistema qual � a nossa p�gina de login customizada
    options.LoginPath = "/Conta/Login";
    options.LogoutPath = "/Conta/Sair";
    options.AccessDeniedPath = "/Conta/AcessoNegado"; // Rota para acesso negado
});


// --- INJE��O DE DEPEND�NCIA DAS NOSSAS CAMADAS ---
// Reposit�rios
builder.Services.AddScoped<IInventarioRepositorio, InventarioRepositorio>();

// Camada de Dom�nio (Especialistas)
builder.Services.AddScoped<IInventarioService, InventarioService>();
builder.Services.AddScoped<IManutencaoService, ManutencaoService>();

// Camada de Aplica��o (Gerentes)
builder.Services.AddScoped<IInventarioApp, InventarioApp>();
builder.Services.AddScoped<IManutencaoApp, ManutencaoApp>();

// Servi�os de Suporte
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 2. CONFIGURA��O DO PIPELINE HTTP ---
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

app.UseRouting();

// Ordem correta e crucial para o login funcionar
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();