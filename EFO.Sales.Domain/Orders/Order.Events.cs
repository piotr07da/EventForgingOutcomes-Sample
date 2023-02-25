namespace EFO.Sales.Domain.Orders;

public sealed record OrderStarted(Guid OrderId);

public sealed record OrderCustomerAssigned(Guid OrderId, Guid CustomerId);

public sealed record OrderDiscountApplied(Guid OrderId, Guid DiscountApplicationId); // TODO

public sealed record OrderDiscountCanceled(Guid OrderId, Guid DiscountApplicationId);

public sealed record OrderPriced(Guid OrderId, decimal Price);

public sealed record OrderItemAdded(Guid OrderId, Guid OrderItemId, Guid ProductId);

public sealed record OrderItemRemoved(Guid OrderId, Guid OrderItemId);

public sealed record OrderItemQuantityChanged(Guid OrderId, Guid OrderItemId, int Quantity);

public sealed record OrderItemPriced(Guid OrderId, Guid OrderItemId, decimal Price);
