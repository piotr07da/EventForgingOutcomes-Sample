using EFO.Sales.Application;
using EFO.Shared.Application.MassTransit;
using EFO.WebApi.ServiceCollectionExtensions;
using MassTransit;

namespace EFO.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        var services = builder.Services;

        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddMediator(x =>
        {
            x.AddSalesApplicationLayerConsumers();

            x.ConfigureMediator((registrationContext, configurator) =>
            {
                configurator.ConnectConsumerConfigurationObserver(new ConsumerConfigurationObserver(registrationContext));
            });
        });

        services.AddInMemoryEventForging();
        services.AddInMemoryMassTransit();

        services.AddLogging();
        services.AddLocalization();

        services.AddSalesApplicationLayer();

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
