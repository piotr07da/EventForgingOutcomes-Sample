using EventForging;

namespace EFO.Sales.Domain.Customers;

public class Customer : IEventForged
{
    public Customer()
    {
        Events = Events.CreateFor(this);
    }

    public Events Events { get; }

    public CustomerId Id { get; private set; }

    public static Customer Register(CustomerId id)
    {
        var customer = new Customer();
        customer.Events.Apply(new CustomerRegistered(id));
        return customer;
    }

    private void Apply(CustomerRegistered e)
    {
        Id = CustomerId.Restore(e.CustomerId);
    }
}
