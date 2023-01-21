using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Stackaby.Database;
using Stackaby.Interfaces;
using Stackaby.Portal.Services;
using Stackaby.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
ConfigureServices(services);

var app = builder.Build();
ConfigureApplication(app);

app.Run();

void ConfigureApplication(WebApplication app)
{
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
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
}

void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<DataContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
    services.AddControllersWithViews();
    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/";
        });
    
    // Dependency Injection
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IAuthenticationService, AuthenticationService>();
    services.AddScoped<IProjectService, ProjectService>();
    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
}