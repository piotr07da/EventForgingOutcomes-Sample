using EFO.Sales.Application;
using EFO.Sales.Application.MassTransit;
using EventForging.DependencyInjection;
using EventForging.InMemory.DependencyInjection;
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
            x.AddConsumers(typeof(StartOrderConsumer).Assembly);

            x.ConfigureMediator((registrationContext, configurator) =>
            {
                configurator.ConnectConsumerConfigurationObserver(new ConsumerConfigurationObserver(registrationContext));
            });
        });

        services.AddEventForging(c =>
        {
            c.UseInMemory();
        });

        services.AddLogging();
        services.AddLocalization();

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
