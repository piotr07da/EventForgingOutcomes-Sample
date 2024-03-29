using EFO.Sales.Application.Commands.Orders;
using EFO.Sales.Application.Queries.Orders;
using EFO.Sales.Application.ReadModel.Orders;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace EFO.WebApi.Controllers.Orders;

[ApiController]
[Route("orders")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet()]
    public async Task<OrderDto[]> GetOrders(CancellationToken cancellationToken = default)
    {
        var query = new GetOrders();

        var client = _mediator.CreateRequestClient<GetOrders>();
        var orderResponse = await client.GetResponse<OrderDto[]>(query, cancellationToken);
        return orderResponse.Message;
    }

    [HttpGet("{orderId}")]
    public async Task<OrderDto> GetOrder([FromRoute] Guid orderId, CancellationToken cancellationToken = default)
    {
        var query = new GetOrder
        {
            OrderId = orderId,
        };

        var client = _mediator.CreateRequestClient<GetOrder>();
        var orderResponse = await client.GetResponse<OrderDto>(query, cancellationToken);
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
