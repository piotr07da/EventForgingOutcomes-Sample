using EFO.Sales.Application.Commands;
using EFO.Sales.Application.Queries;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace EFO.Sales.WebApi.Controllers;

[ApiController]
[Route("orders")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet("{orderId}")]
    public async Task<OrderDto> GetOrder([FromRoute] Guid orderId)
    {
        var query = new GetOrder(orderId);

        var client = _mediator.CreateRequestClient<GetOrder>();
        var orderResponse = await client.GetResponse<OrderDto>(query);
        return orderResponse.Message;
    }

    [HttpPost("")]
    public async Task<CreatedResult> StartOrder([FromBody] StartOrderBody body, CancellationToken cancellationToken = default)
    {
        var command = new StartOrder(Guid.NewGuid(), body.CustomerId);

        await _mediator.Send(command, cancellationToken);

        return Created($"api/orders/{command.OrderId}", command.OrderId);
    }

    [HttpPost("{orderId}/items")]
    public async Task<CreatedResult> AddOrderItem([FromRoute] Guid orderId, [FromBody] AddOrderItemBody body, CancellationToken cancellationToken = default)
    {
        var command = new AddOrderItem(orderId, Guid.NewGuid(), body.ProductId, body.Quantity);

        await _mediator.Send(command, cancellationToken);

        return Created($"orders/{orderId}/items/{command.OrderItemId}", command.OrderItemId);
    }
}
