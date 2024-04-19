using Microsoft.EntityFrameworkCore;
using WebMvc.Service;

namespace WebMvc;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddDbContext<BusContext>(Options => Options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddControllersWithViews();
        builder.Services.AddScoped<IEntryService,EntryService>();
        builder.Services.AddScoped<ILoopService,LoopService>();
        builder.Services.AddScoped<IDriverService,DriverService>();
        builder.Services.AddScoped<IBusService,BusService>();
        builder.Services.AddScoped<IStopService,StopService>();
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
    }
}
