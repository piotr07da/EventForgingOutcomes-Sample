using EFO.Sales.Domain.Customers;
using EFO.Sales.Domain.Orders;
using EventForging;
using MassTransit;

namespace EFO.Sales.Application.Commands.Orders;

public sealed class StartOrderHandler : IConsumer<StartOrder>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Customer> _customerRepository;

    public StartOrderHandler(IRepository<Order> orderRepository, IRepository<Customer> customerRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }

    public async Task Consume(ConsumeContext<StartOrder> context)
    {
        var command = context.Message;

        var customer = await _customerRepository.GetAsync(command.CustomerId, context);

        var order = Order.Start(command.OrderId, customer);
        await _orderRepository.SaveAsync(command.OrderId, order, context);
    }
}
