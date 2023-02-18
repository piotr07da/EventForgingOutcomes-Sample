﻿using EFO.Sales.Domain.Localization;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace EFO.Sales.Application.MassTransit;

public class ConsumerConfigurationObserver : IConsumerConfigurationObserver
{
    private readonly IServiceProvider _serviceProvider;

    public ConsumerConfigurationObserver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public void ConsumerConfigured<TConsumer>(IConsumerConfigurator<TConsumer> configurator)
        where TConsumer : class
    {
    }

    public void ConsumerMessageConfigured<TConsumer, TMessage>(IConsumerMessageConfigurator<TConsumer, TMessage> configurator)
        where TConsumer : class where TMessage : class
    {
        configurator.UseFilter(new DomainExceptionLocalizationFilter<TConsumer, TMessage>(_serviceProvider.GetRequiredService<IStringLocalizer<SalesLocalizationResource>>()));
    }
}
