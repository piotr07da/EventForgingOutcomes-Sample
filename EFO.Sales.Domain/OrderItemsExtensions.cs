﻿namespace EFO.Sales.Domain;

public static class OrderItemsExtensions
{
    public static Money SumUpPrices(this OrderItems items)
    {
        var totalPrice = Money.Zero;
        foreach (var item in items)
        {
            totalPrice += item.Price;
        }

        return totalPrice;
    }
}
