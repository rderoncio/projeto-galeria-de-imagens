// Services
using App.Context;
using App.Services;
using App.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Web.Caching;
using SixLabors.ImageSharp.Web.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddSingleton<IProcessadorImagem, ProcessadorImagemService>();

builder.Services.AddDbContext<DatabaseContext>(
    options => 
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection"));
    });

builder.Services.AddImageSharp(
    options =>
    {
        options.BrowserMaxAge = TimeSpan.FromDays(7); // Duração do cache de imagem no nagevador
        options.CacheMaxAge = TimeSpan.FromDays(365); // Duração do cache de imagem no servidor
        options.CacheHashLength = 8; // Nome do arquivos em cache com no máximo 8 caracteres 
    }
    ).Configure<PhysicalFileSystemCacheOptions>(
    options =>
    {
        options.CacheFolder = "img/cache"; // default: wwwroot/ + CacheFolder
    });

// Configure
WebApplication app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseImageSharp();
app.UseStaticFiles();
app.UseRouting();

app.MapDefaultControllerRoute();
app.Run();
