using AMINICAD.DAL;
using AMINICAD.Data.Ingresos;
using AMINICAD.Data.Misioneros;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IDashboardDAL, DashboardDAL>();
builder.Services
    .AddScoped<IngresoControlOperativoRepository>();
builder.Services
    .AddScoped<MisioneroRepository>();


builder.Services
    .AddScoped<MisioneroDetalleRepository>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
