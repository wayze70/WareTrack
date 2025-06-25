using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using WareTrack.Web.Components;
using WareTrack.Web.Data;
using WareTrack.Web.Services;

namespace WareTrack.Web;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        builder.Services.AddMudServices();
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
                o => o.SetPostgresVersion(17, 2)));
        builder.Services.AddScoped<IWarehouseService, WarehouseService>();
        builder.Services.AddScoped<IShelfService, ShelfService>();
        builder.Services.AddScoped<ILocationService, LocationService>();
        builder.Services.AddScoped<IInventoryItemService, InventoryItemService>();
        builder.Services.AddScoped<ITransferService, TransferService>();
        builder.Services.AddScoped<DatabaseSeeder>();
        builder.Services.AddScoped<FilterStateService>();
        
        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}