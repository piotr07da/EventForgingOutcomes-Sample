using EFO.Catalog.Domain.ProductProperties;
using EventForging;
using MassTransit;

namespace EFO.Catalog.Application.Commands.ProductProperties;

public sealed class DefineTextPropertyHandler : IConsumer<DefineTextProperty>
{
    private readonly IRepository<TextProperty> _propertyRepository;

    public DefineTextPropertyHandler(IRepository<TextProperty> propertyRepository)
    {
        _propertyRepository = propertyRepository ?? throw new ArgumentNullException(nameof(propertyRepository));
    }

    public async Task Consume(ConsumeContext<DefineTextProperty> context)
    {
        var command = context.Message;

        var property = TextProperty.Define(command.PropertyId, command.PropertyName);

        await _propertyRepository.SaveAsync(command.PropertyId, property, context);
    }
}
