// ReSharper disable once CheckNamespace

namespace EFO.WebUi.Components;

public sealed record ProductAddedToOrderEventArgs(Guid ProductId, int Quantity);
