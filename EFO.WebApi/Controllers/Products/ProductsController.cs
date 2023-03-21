using EFO.Sales.Application.Commands.Products;
using EFO.SharedReadModel.Queries;
using EFO.SharedReadModel.ReadModel.Products;
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

    [HttpPost("search")]
    public async Task<ProductsDto> SearchProducts([FromQuery] Guid categoryId, [FromBody] SearchProductsBody body, CancellationToken cancellationToken = default)
    {
        var query = new GetProducts(categoryId, body.NumericPropertiesFilters, body.TextPropertiesFilters);

        var client = _mediator.CreateRequestClient<GetProducts>();
        var orderResponse = await client.GetResponse<ProductsDto>(query, cancellationToken);
        return orderResponse.Message;
    }

    [HttpPost("")]
    public async Task<CreatedResult> IntroduceProduct([FromBody] IntroduceProductBody body, CancellationToken cancellationToken = default)
    {
        var command = new IntroduceProduct(Guid.NewGuid(), body.ProductName);

        await _mediator.Send(command, cancellationToken);

        return Created($"products/{command.ProductId}", command.ProductId);
    }

    [HttpPut("{productId}/prices")]
    public async Task<NoContentResult> PriceProduct([FromRoute] Guid productId, [FromBody] PriceProductBody body, CancellationToken cancellationToken = default)
    {
        var command = new PriceProduct(productId, body.QuantityThreshold, body.UnitPrice);

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
