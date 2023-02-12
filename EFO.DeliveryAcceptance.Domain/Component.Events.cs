namespace EFO.DeliveryAcceptance.Domain;

public sealed record ComponentArrived(Guid Id);

public sealed record ComponentNamed(Guid Id, string Name);

public sealed record ComponentClassified(Guid Id, ComponentClass Class);

public sealed record ComponentMeasured(Guid Id, double Width, double Height, double Depth);

public sealed record ComponentWeighed(Guid Id, double Weight);

public sealed record ComponentInspectionCompleted(Guid Id);
