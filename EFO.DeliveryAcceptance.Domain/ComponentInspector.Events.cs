namespace EFO.DeliveryAcceptance.Domain;

public sealed record ComponentInspectorHired(Guid Id);

public sealed record ComponentInspectorCertified(Guid Id, ComponentClass ComponentClass);
