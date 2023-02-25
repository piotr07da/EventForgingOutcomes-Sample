using System.Reflection;
using System.Text;
using EFO.Shared.Domain;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace EFO.Shared.Application.MassTransit;

public sealed class DomainExceptionLocalizationFilter<TConsumer, TMessage> : IFilter<ConsumerConsumeContext<TConsumer, TMessage>>
    where TConsumer : class
    where TMessage : class
{
    private readonly IStringLocalizer[] _localizers;

    public DomainExceptionLocalizationFilter(IServiceProvider serviceProvider)
    {
        _localizers = (serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider))).GetRequiredService<IEnumerable<IStringLocalizer>>().ToArray();
    }

    public void Probe(ProbeContext context)
    {
    }

    public async Task Send(ConsumerConsumeContext<TConsumer, TMessage> context, IPipe<ConsumerConsumeContext<TConsumer, TMessage>> next)
    {
        try
        {
            await next.Send(context);
        }
        catch (DomainException de)
        {
            var messageBuilder = new StringBuilder();
            foreach (var error in de.Errors)
            {
                var localizedMessage = TryFindValue(error.Name);
                if (localizedMessage is null)
                {
                    messageBuilder.AppendLine(error.Name);
                }
                else
                {
                    foreach (var (key, value) in error.Data)
                    {
                        localizedMessage = localizedMessage.Replace($"{{{key}}}", value.ToString());
                    }

                    messageBuilder.AppendLine(localizedMessage);
                }

                messageBuilder.AppendLine();
            }

            ChangeMessage(de, messageBuilder.ToString());

            throw;
        }
    }

    private string? TryFindValue(string key)
    {
        foreach (var localizer in _localizers)
        {
            var localizerResult = localizer[key];
            if (!localizerResult.ResourceNotFound)
            {
                return localizerResult.Value;
            }
        }

        return null;
    }

    private static void ChangeMessage(Exception exception, string message)
    {
        var messageFiled = exception.GetType().GetField("_message", BindingFlags.Instance | BindingFlags.NonPublic);
        messageFiled!.SetValue(exception, message);
    }
}
