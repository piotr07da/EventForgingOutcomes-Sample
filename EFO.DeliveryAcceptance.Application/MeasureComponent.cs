namespace EFO.DeliveryAcceptance.Application;

public sealed record MeasureComponent(Guid ComponentId, Guid ComponentInspectorId, double Width, double Height, double Depth);
