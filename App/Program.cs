// Services
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// Configure
WebApplication app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseRouting();

app.MapDefaultControllerRoute();
app.Run();
