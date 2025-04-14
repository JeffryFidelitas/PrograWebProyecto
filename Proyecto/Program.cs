using CoreLibrary.Data;
using CoreLibrary.Services;
using CoreLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Agregamos las interfaces
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<ICitaService, CitaService>();
builder.Services.AddScoped<IServicioService, ServicioService>();
builder.Services.AddScoped<IVehiculoService, VehiculoService>();

// Agregamos los servicios de autenticacion
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Login";
        options.AccessDeniedPath = "/Accesos/Error403";
    });

// Configuraci�n de DBContext
builder.Services.AddDbContext<ProyectDBContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("Proyecto"));
});

var app = builder.Build();


// Usamos el middleware para manejar errores y redirigir a la acci�n correspondiente
app.UseStatusCodePagesWithReExecute("/Accesos/Error", "?statusCode={0}");
app.UseHsts();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  // Aseg�rate de usar la autenticaci�n
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
