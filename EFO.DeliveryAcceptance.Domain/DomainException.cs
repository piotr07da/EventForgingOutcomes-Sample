namespace EFO.DeliveryAcceptance.Domain;

public class DomainException : Exception
{
    public DomainException(params string[] errors)
    {
        Errors = errors;
    }

    public string[] Errors { get; }

    public static void ThrowIfErrors(IEnumerable<string> errors)
    {
        var errorsArray = errors as string[] ?? errors.ToArray();
        if (errorsArray.Any())
        {
            throw new DomainException(errorsArray.ToArray());
        }
    }
}
