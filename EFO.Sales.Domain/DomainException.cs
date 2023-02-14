namespace EFO.Sales.Domain;

public class DomainException : Exception
{
    public DomainException(params DomainError[] errors)
    {
        Errors = errors;
    }

    public DomainError[] Errors { get; }

    public static void ThrowIfErrors(IEnumerable<DomainError> errors)
    {
        var errorsArray = errors as DomainError[] ?? errors.ToArray();
        if (errorsArray.Any())
        {
            throw new DomainException(errorsArray);
        }
    }
}
