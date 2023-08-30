using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using eShopApp.DataAccessLayer.Data;
using eShopApp.DataAccessLayer.UOF;
using eShopApp.Models;
using eShopApp.Utility;
using eShopApp.Utility.DataSeeding;
using eShopApp.Web.Client;
using eShopApp.Web.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 4;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.Configure<PaypalSetting>(builder.Configuration.GetSection(nameof(PaypalSetting)));
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        var area = context.Request.RouteValues["area"]?.ToString();
        var queryString = context.HttpContext.Request.QueryString.ToString();
        var returnUrl = context.Request.Path + queryString;
        if (area != null && area.ToLower() == "admin")
        {
            context.Response.Redirect($"/Admin/AdminAccount/Login?returnUrl={returnUrl}");
        }
        else
        {
            context.Response.Redirect($"/Account/Login?returnUrl={returnUrl}");
        }
        return Task.CompletedTask;
    };
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddNotyf(config => { config.DurationInSeconds = 6; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });

//DI
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<FileUploadHelper>();
builder.Services.AddTransient<PaypalClient>();

var app = builder.Build();

SeedData();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseNotyf();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});


app.Run();

void SeedData()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}
