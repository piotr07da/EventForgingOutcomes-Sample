using System.Reflection;
using System.Text;
using EFO.Sales.Domain.Localization;
using EFO.Shared.Domain;
using MassTransit;
using Microsoft.Extensions.Localization;

namespace EFO.Sales.Application.MassTransit;

public sealed class DomainExceptionLocalizationFilter<TConsumer, TMessage> : IFilter<ConsumerConsumeContext<TConsumer, TMessage>>
    where TConsumer : class
    where TMessage : class
{
    private readonly IStringLocalizer<SalesLocalizationResource> _localizer;

    public DomainExceptionLocalizationFilter(IStringLocalizer<SalesLocalizationResource> localizer)
    {
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
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
                var localizerResult = _localizer[error.Name];
                if (localizerResult.ResourceNotFound)
                {
                    messageBuilder.AppendLine(error.Name);
                }
                else
                {
                    var localizedMessage = localizerResult.Value;
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

    private static void ChangeMessage(Exception exception, string message)
    {
        var messageFiled = exception.GetType().GetField("_message", BindingFlags.Instance | BindingFlags.NonPublic);
        messageFiled!.SetValue(exception, message);
    }
}
