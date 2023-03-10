using EFO.WebUi.Data;

namespace EFO.WebUi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var services = builder.Services;

        // Add services to the container.
        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddSingleton<IProductCategoriesService, ProductCategoriesService>();
        services.AddSingleton<IProductService, ProductService>();
        services.AddSingleton<IOrderService, OrderService>();

        services.AddHttpClient("EFO", (sp, client) =>
        {
            var c = sp.GetRequiredService<IConfiguration>();
            client.BaseAddress = new Uri(c["Services:EFO"] ?? throw new Exception("Undefined configuration for EFO Service."));
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }
}
