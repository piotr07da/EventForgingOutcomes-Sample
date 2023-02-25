using EFO.Sales.Application.Commands.Products;
using EFO.Sales.Application.Queries.Products;
using EFO.Sales.Application.ReadModel.Products;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace EFO.WebApi.Controllers.Products;

[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet("")]
    public async Task<ProductsDto> GetProducts(CancellationToken cancellationToken = default)
    {
        var query = new GetProducts();

        var client = _mediator.CreateRequestClient<GetProducts>();
        var orderResponse = await client.GetResponse<ProductsDto>(query, cancellationToken);
        return orderResponse.Message;
    }

    [HttpPost("")]
    public async Task<CreatedResult> IntroduceProduct([FromBody] IntroduceProductBody body, CancellationToken cancellationToken = default)
    {
        var command = new IntroduceProduct(Guid.NewGuid(), body.ProductName);

        await _mediator.Send(command, cancellationToken);

        return Created($"api/products/{command.ProductId}", command.ProductId);
    }

    [HttpPut("{productId}/prices")]
    public async Task<NoContentResult> PriceProduct([FromRoute] Guid productId, [FromBody] PriceProductBody body, CancellationToken cancellationToken = default)
    {
        var command = new PriceProduct(productId, body.QuantityThreshold, body.UnitPrice);

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPost("samples")]
    public async Task<NoContentResult> CreateSamples(CancellationToken cancellationToken = default)
    {
        for (var pIx = 0; pIx < 100; ++pIx)
        {
            var productId = Guid.NewGuid();
            await _mediator.Send(new IntroduceProduct(productId, $"Product {productId.ToString()[..6]}"), cancellationToken);
            for (var priceIx = 0; priceIx < 3; ++priceIx)
            {
                await _mediator.Send(new PriceProduct(productId, (priceIx + 1) * 10, 100 - priceIx * 10), cancellationToken);
            }
        }

        return NoContent();
    }
}
