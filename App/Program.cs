// Services
using SixLabors.ImageSharp.Web.Caching;
using SixLabors.ImageSharp.Web.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddImageSharp(
    options =>
    {
        options.BrowserMaxAge = TimeSpan.FromDays(7); // Duração do cache de imagem no nagevador
        options.CacheMaxAge = TimeSpan.FromDays(365.25); // Duração do cache de imagem no servidor
        options.CacheHashLength = 8; // Nome do arquivos em cache com no máximo 8 caracteres 
    }
    ).Configure<PhysicalFileSystemCacheOptions>(
    options =>
    {
        options.CacheFolder = "img/cache"; // default: wwwroot/ 
    });

// Configure
WebApplication app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseImageSharp();
app.UseStaticFiles();
app.UseRouting();

app.MapDefaultControllerRoute();
app.Run();
