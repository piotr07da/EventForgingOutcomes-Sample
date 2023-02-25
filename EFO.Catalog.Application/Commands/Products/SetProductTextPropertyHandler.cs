﻿using EFO.Catalog.Domain.Products;
using EventForging;
using MassTransit;

namespace EFO.Catalog.Application.Commands.Products;

public sealed class SetProductTextPropertyHandler : IConsumer<SetProductTextProperty>
{
    private readonly IRepository<Product> _productRepository;

    public SetProductTextPropertyHandler(IRepository<Product> productRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task Consume(ConsumeContext<SetProductTextProperty> context)
    {
        var command = context.Message;

        var product = await _productRepository.GetAsync(command.ProductId, context);

        product.SetProperty(command.PropertyId, command.PropertyText);

        await _productRepository.SaveAsync(command.ProductId, product, context);
    }
}
