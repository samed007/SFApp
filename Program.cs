using Microsoft.AspNetCore.Authentication.Cookies;
using SFApp.DAOs;
using SFApp.Mapping;
using SFApp.Services;

var builder = WebApplication.CreateBuilder(args);

// -----------------------
// Add services to the container
// -----------------------
builder.Services.AddControllersWithViews();

// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "SFAppAuthCookie";     // Nombre de la cookie
        options.LoginPath = "/Auth/Login";           // Ruta del login
        options.LogoutPath = "/Auth/Logout";         // Ruta del logout
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.SlidingExpiration = true;         // Renueva la cookie al usarla
        options.Cookie.HttpOnly = true;             // No accesible desde JS
        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
    });


builder.Services.AddAuthorization();

// -----------------------
// Registrar servicios y DAOs
// -----------------------
builder.Services.AddScoped<IUsuariosService, UsuariosService>();
builder.Services.AddScoped<IUsuariosDAO, UsuariosDAO>();

builder.Services.AddScoped<IProductosService, ProductosService>();
builder.Services.AddScoped<IProductosDAO, ProductosDAO>();


builder.Services.AddScoped<IClientesService, ClientesService>();
builder.Services.AddScoped<IClientesDAO, ClientesDAO>();

builder.Services.AddScoped<ITransaccionesService, TransaccionesService>();
builder.Services.AddScoped<ITransaccionesDAO, TransaccionesDAO>();

builder.Services.AddScoped<IInventarioService, InventarioService>();
builder.Services.AddScoped<IInventarioDAO, InventarioDAO>();

// -----------------------
// Construir la aplicación
// -----------------------
var app = builder.Build();

// -----------------------
// Middleware
// -----------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  
app.UseAuthorization();

// -----------------------
// Rutas
// -----------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");


app.Run();
