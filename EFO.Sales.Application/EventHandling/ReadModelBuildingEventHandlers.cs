﻿using EFO.Sales.Application.ReadModel;
using EFO.Sales.Domain;
using EventForging.EventsHandling;

namespace EFO.Sales.Application.EventHandling;

internal class ReadModelBuildingEventHandlers :
    IEventHandler<OrderStarted>,
    IEventHandler<OrderCustomerAssigned>
{
    private readonly IOrdersReadModel _ordersReadModel;

    public ReadModelBuildingEventHandlers(IOrdersReadModel ordersReadModel)
    {
        _ordersReadModel = ordersReadModel ?? throw new ArgumentNullException(nameof(ordersReadModel));
    }

    public string SubscriptionName => "ReadModelBuilder";

    public Task Handle(OrderStarted e, EventInfo ei, CancellationToken cancellationToken)
    {
        _ordersReadModel.GetOrAdd(e.OrderId);
        return Task.CompletedTask;
    }

    public Task Handle(OrderCustomerAssigned e, EventInfo ei, CancellationToken cancellationToken)
    {
        var order = _ordersReadModel.GetOrAdd(e.OrderId);
        order.CustomerId = e.CustomerId;
        return Task.CompletedTask;
    }
}