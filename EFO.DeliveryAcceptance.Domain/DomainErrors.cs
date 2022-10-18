namespace EFO.DeliveryAcceptance.Domain;

public static class DomainErrors
{
    public static readonly string ComponentInspectionAlreadyCompleted = nameof(ComponentInspectionAlreadyCompleted);
    public static readonly string ComponentInspectorDoesNotHaveRequiredCertification = nameof(ComponentInspectorDoesNotHaveRequiredCertification);
    public static readonly string ComponentNotMeasured = nameof(ComponentNotMeasured);
    public static readonly string ComponentNotWeighed = nameof(ComponentNotWeighed);

    public static void AddIf(this IList<string> domainErrors, string domainError, bool condition)
    {
        if (condition)
        {
            domainErrors.Add(domainError);
        }
    }
}
