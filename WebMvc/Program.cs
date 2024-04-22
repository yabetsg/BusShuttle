using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebMvc.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebIdentity.Data;

namespace WebMvc;

public class Program
{
    public static async Task CreateRoles(IServiceProvider serviceProvider)
{
    using (var scope = serviceProvider.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roleNames = { "driver", "manager" };

        foreach (var roleName in roleNames)
        {
            bool roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                IdentityRole role = new IdentityRole(roleName);
                await roleManager.CreateAsync(role);
            }
        }
    }
}

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(connectionString));
        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddSession();
        builder.Services.AddDbContext<BusContext>(Options => Options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddControllersWithViews();
        builder.Services.AddScoped<IEntryService, EntryService>();
        builder.Services.AddScoped<ILoopService, LoopService>();
        builder.Services.AddScoped<IDriverService, DriverService>();
        builder.Services.AddScoped<IBusService, BusService>();
        builder.Services.AddScoped<IStopService, StopService>();
        builder.Services.ConfigureApplicationCookie(options =>
         {
             options.LoginPath = "/Home/Login";
             options.AccessDeniedPath = "/Home/AccessDenied";
         });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }


        app.UseSession();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        CreateRoles(app.Services).Wait();

        app.Run();
    }
}
