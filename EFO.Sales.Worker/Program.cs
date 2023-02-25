using MassTransit;

namespace EFO.Sales.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.RegisterModule
                    services.AddMassTransit(x =>
                    {
                        
                    });
                })
                .Build();

            host.Run();
        }
    }

    public class SalesModule : Module
    {
        override load
    }
}