using EFO.WebUi.Components.ProductList;
using EFO.WebUi.Data;
using EFO.WebUi.Pages;
using Refit;

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

        services.AddTransient<CatalogViewModel>();
        services.AddScoped<IProductListViewModelFactory, ProductListViewModelFactory>();
        services.AddScoped<IProductRowViewModelFactory, ProductRowViewModelFactory>();

        AddAndConfigureEfoHttpClient<IProductCategoryService>(services);
        AddAndConfigureEfoHttpClient<IProductService>(services);
        AddAndConfigureEfoHttpClient<IOrderService>(services);

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

    private static void AddAndConfigureEfoHttpClient<T>(IServiceCollection services)
        where T: class
    {
        services.AddTransient<ErrorHandlingHttpMessageHandler>();

        services
            .AddRefitClient<T>()
            .ConfigureHttpClient((sp, client) =>
            {
                var c = sp.GetRequiredService<IConfiguration>();
                client.BaseAddress = new Uri(c["Services:EFO"] ?? throw new Exception("Undefined configuration for EFO Service."));
            })
            .AddHttpMessageHandler<ErrorHandlingHttpMessageHandler>();
    }
}
