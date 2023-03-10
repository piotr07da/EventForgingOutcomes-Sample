using EFO.Catalog.Domain.ProductProperties;
using EventForging;
using MassTransit;

namespace EFO.Catalog.Application.Commands.ProductProperties;

public sealed class DefineNumericPropertyHandler : IConsumer<DefineNumericProperty>
{
    private readonly IRepository<NumericProperty> _propertyRepository;

    public DefineNumericPropertyHandler(IRepository<NumericProperty> propertyRepository)
    {
        _propertyRepository = propertyRepository ?? throw new ArgumentNullException(nameof(propertyRepository));
    }

    public async Task Consume(ConsumeContext<DefineNumericProperty> context)
    {
        var command = context.Message;

        var property = NumericProperty.Define(command.PropertyId, command.PropertyName, command.PropertyUnit);

        await _propertyRepository.SaveAsync(command.PropertyId, property, context);
    }
}
