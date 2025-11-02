
using SFApp.DAOs;
using SFApp.Mapping;
using SFApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

// Registrar servicios y DAOs
builder.Services.AddScoped<IProductosService, ProductosService>();
builder.Services.AddScoped<IProductosDAO, ProductosDAO>();
// Registrar servicios y DAOs
builder.Services.AddScoped<ITransaccionesService, TransaccionesService>();
builder.Services.AddScoped<ITransaccionesDAO, TransaccionesDAO>();

builder.Services.AddScoped<IInventarioService, InventarioService>();
builder.Services.AddScoped<IInventarioDAO, InventarioDAO>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
