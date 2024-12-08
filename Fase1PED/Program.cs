using Fase1PED.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
        options.SlidingExpiration = false; // No renueva automáticamente
        // No se especifica ExpireTimeSpan, la cookie dependerá de la sesión del navegador
    });

// Configurar la conexión a la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar controladores con vistas
builder.Services.AddControllersWithViews();

var app = builder.Build(); // Aquí se marca la colección como de solo lectura

// Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Cache-Control", "no-store, no-cache, must-revalidate, max-age=0");
    context.Response.Headers.Add("Pragma", "no-cache");
    context.Response.Headers.Add("Expires", "0");
    await next();
});

app.UseAuthentication(); // Middleware de autenticación
app.UseAuthorization();  // Middleware de autorización

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}");

app.Run();