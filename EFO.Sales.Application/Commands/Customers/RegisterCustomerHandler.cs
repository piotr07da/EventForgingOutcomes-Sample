using EFO.Sales.Domain.Customers;
using EventForging;
using MassTransit;

namespace EFO.Sales.Application.Commands.Customers;

public sealed class RegisterCustomerHandler : IConsumer<RegisterCustomer>
{
    private readonly IRepository<Customer> _repository;

    public RegisterCustomerHandler(IRepository<Customer> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task Consume(ConsumeContext<RegisterCustomer> context)
    {
        var command = context.Message;

        var customer = Customer.Register(command.CustomerId);

        await _repository.SaveAsync(command.CustomerId, customer, context);
    }
}
