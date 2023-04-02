using EFO.Catalog.Application;
using EFO.Sales.Application;
using EFO.SharedReadModel;
using EFO.WebApi.ServiceCollectionExtensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace EFO.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        var services = builder.Services;

        services.AddControllers();
        services.AddMvc(o =>
        {
            o.Filters.Add(new ExceptionFilter());
        });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddSharedReadModel();
        services.AddCatalogApplicationLayer();
        services.AddSalesApplicationLayer();

        services.AddAndConfigureEventForging();

        services.AddAndConfigureMediator();
        services.AddAndConfigureMassTransit();

        services.AddLogging();
        
        services.AddLocalization();
        services.AddSingleton<IStringLocalizer>(sp => sp.GetRequiredService<IStringLocalizer<Catalog.Domain.Localization.CatalogLocalizationResource>>());
        services.AddSingleton<IStringLocalizer>(sp => sp.GetRequiredService<IStringLocalizer<Sales.Domain.Localization.SalesLocalizationResource>>());

        services.AddHostedService<SampleDataInitializingHostedService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
