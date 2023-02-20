using EFO.Sales.Application;
using EFO.Sales.Application.Commands;
using EFO.Sales.Application.EventHandling;
using EFO.Sales.Application.MassTransit;
using EFO.Sales.Domain;
using EventForging;
using EventForging.InMemory;
using EventForging.Serialization;
using MassTransit;

namespace EFO.Sales.WebApi;

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
            x.AddConsumers(typeof(StartOrderHandler).Assembly);

            x.ConfigureMediator((registrationContext, configurator) =>
            {
                configurator.ConnectConsumerConfigurationObserver(new ConsumerConfigurationObserver(registrationContext));
            });
        });

        services.AddEventForging(r =>
        {
            r.ConfigureEventForging(c =>
            {
                c.Serialization.SetEventTypeNameMappers(new DefaultEventTypeNameMapper(typeof(OrderStarted).Assembly));
            });
            r.UseInMemory(c =>
            {
                c.SerializationEnabled = true;
                c.AddEventSubscription("MainPipeline");
            });
            r.AddEventHandlers(typeof(EventHandlers).Assembly);
        });

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
