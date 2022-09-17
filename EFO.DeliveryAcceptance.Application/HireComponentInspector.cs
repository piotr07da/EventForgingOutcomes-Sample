using EFO.DeliveryAcceptance.Domain;

namespace EFO.DeliveryAcceptance.Application;

public record HireComponentInspector(Guid InspectorId, ComponentInspectorCertificationLevel CertificationLevel);
